using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    [RequireComponent(typeof(AirplaneFlightPhysics))]
    public class AirplaneController : BaseRigidbodyController
    {
        #region ImportantVariables
        [Header("Podstawowe Elementy")] // Nagłowek Wyswietlany w Inspektorze
        public BaseInput input;
        public Transform massGravityCenter;
        public AirplaneFlightPhysics flightPhysics;

        [Tooltip("Waga w Kilogramach")] // Podpowiedz dla wartosci
        public float WeightOfAirplane;

        [Header("Silniki Samolotu")]
        public List<AirplaneEngine> airplaneEngines = new List<AirplaneEngine>();

        [Header("Koła Samolotu")]
        public List<AirplaneWheel> airplaneWheels = new List<AirplaneWheel>();

        [Header("Stateczniki Poziome i Pionowe Samolotu")]
        public List<ControlSurfaces> controlSurfaces = new List<ControlSurfaces>();

        [Header("Kolizje")]
        protected float airplaneMaxHealth = 100f;

        private float airplaneCurrentHealth;
        public float AirplaneCurrentHealth
        {
            get { return airplaneCurrentHealth; }
        }
        private float normalizedAirplaneHealth;
        public float NormalizedAirplaneHealth
        {
            get { return normalizedAirplaneHealth; }
        }


        public ParticleSystem engineSmoke;
        public List<ParticleSystem> bodyExplosions = new List<ParticleSystem>();

        private float heightOverTheTerrain = 0f;

        public float HeightOverTheTerrain
        {
            get { return heightOverTheTerrain; }
        }


        private bool isAirplaneDestroyed = false;
        public bool IsAirplaneDestroyed
        {
            get { return isAirplaneDestroyed; }
        }

        #endregion

		private float timeStart;

        #region BuiltInMethods

        public override void Start()
        {
            base.Start();

            if (rigidBody)
            {
                rigidBody.mass = WeightOfAirplane;

                if (massGravityCenter)
                    // Przypisanie Środka Masy ze wspolrzednych obiektu Transform;
                    rigidBody.centerOfMass = massGravityCenter.localPosition;
            }

            if (airplaneWheels != null)
            {
                if (airplaneWheels.Count > 0)
                {
                    foreach (AirplaneWheel w in airplaneWheels)
                    {
                        w.UnlockWheel();
                    }
                }
            }

            flightPhysics = GetComponent<AirplaneFlightPhysics>();

            if (flightPhysics)
            {
                flightPhysics.InitFlightPhysics(rigidBody, input);
            }

            airplaneCurrentHealth = airplaneMaxHealth;
            normalizedAirplaneHealth = 1f;
        }

        private void Update()
        {
            AirplaneHealthHandler();
			timeStart += Time.deltaTime;
			//-//////////////////////////////////////
			if (input.Throthle == 0.1f) {
				timeStart = 0f;
				Debug.Log(timeStart);
			}

			if (heightOverTheTerrain > 100f) {
				Debug.Log("Czas startu: " + timeStart);
			}
			//-//////////////////////////////////////

        }
        #endregion


        #region MyOwnOwnMethods 

        protected void AirplaneHealthHandler()
        {
            normalizedAirplaneHealth = airplaneCurrentHealth / airplaneMaxHealth;
            normalizedAirplaneHealth = Mathf.Clamp01(normalizedAirplaneHealth);
        }

        protected override void PhysicsHandler()
        {
            if (input)
            {
                ControlSurfacesHandler();
                EnginesHandler();
                FlightPhysicsHandler();
                AltitudeHandler(); // Wysokosc w powietrzu
                WheelHandler();
            }

            if (rigidBody)
            {
                rigidBody.mass = WeightOfAirplane;
            }
        }

        void EnginesHandler()
        {
            if (airplaneEngines != null)
            {
                if (airplaneEngines.Count > 0)
                {
                    foreach (AirplaneEngine e in airplaneEngines)
                    {
                        rigidBody.AddForce(e.CalcForce(input.CurrentStickyThrothle));
                    }
                }
            }
        }

        void FlightPhysicsHandler()
        {
            if (flightPhysics)
            {
                flightPhysics.UpdateFlightPhysics();
            }
        }

        void AltitudeHandler()
        {
            RaycastHit rayCastHit;

            //Odleglosc poczatku promienia mierzacego, aby nie aktywowal sie na kadlubie samolotu
            float marginRayCastValue = 1f;

            Vector3 AirplanePosition = transform.position;
            AirplanePosition.y -= marginRayCastValue;


            if (Physics.Raycast(AirplanePosition, Vector3.down, out rayCastHit))
            {
                //informacja nad jakim obiektem kolizyjnym sie znajdujemy 
                //Debug.Log(rayCastHit.transform.tag);
                //Jeśli lecimy nad ziemią lub wodą
                if (rayCastHit.transform.tag == "Ground" || rayCastHit.transform.tag == "Water")
                {
                    //Odejmowanie wysokosci bezwzglednej od 0 z dlugoscia promienia do terenu
                    heightOverTheTerrain = transform.position.y - rayCastHit.point.y + marginRayCastValue;

                }
            }
        }

        void ControlSurfacesHandler()
        {
            if (controlSurfaces.Count > 0)
            {
                foreach (ControlSurfaces cs in controlSurfaces)
                {
                    cs.ControlSurfaceInit(input);
                }
            }
        }

        void WheelHandler()
        {
            if (airplaneWheels.Count > 0)
            {
                foreach (AirplaneWheel w in airplaneWheels)
                {
                    w.WheelHandler(input);
                }
            }
        }


        private void OnCollisionEnter(Collision collision)
        {

            if (collision.relativeVelocity.magnitude >= 5f)
            {
                //Odejmowanie wytrzymalosci samolotu
                airplaneCurrentHealth -= collision.relativeVelocity.magnitude;
            }

            if (airplaneCurrentHealth <= 0f && !isAirplaneDestroyed)
            {
                //Wybuch - Koniec lotu
                isAirplaneDestroyed = true;

                if (bodyExplosions.Count > 0)
                {
                    foreach (ParticleSystem ps in bodyExplosions)
                    {
                        ps.Play();
                        ps.Clear();
                    }
                }
            }

            if (engineSmoke)
            {
                //Dym z silnika przy uszkodzeniu 50%towym i nie w wodzie
                if (airplaneCurrentHealth <= airplaneMaxHealth * 0.5 && collision.gameObject.tag == "Ground")
                {
                    engineSmoke.Play();
                }
                if (airplaneCurrentHealth > airplaneMaxHealth * 0.5 || collision.gameObject.tag == "Water")
                {
                    engineSmoke.Stop();
                }
            }
            

        }

        private void OnTriggerEnter(Collider triggerObject)
        {

            switch (triggerObject.tag)
            {
                //Po zanurzeniu w wodzie
                case "Water":
                    {
                        if (engineSmoke)
                        {
                            engineSmoke.Stop();
                        }

                        airplaneCurrentHealth = 0f;
                        isAirplaneDestroyed = true;
                        WeightOfAirplane *= 10;

                    }
                    break;
                    
                default:
                    break;
            }
        }

        #endregion
    }
}