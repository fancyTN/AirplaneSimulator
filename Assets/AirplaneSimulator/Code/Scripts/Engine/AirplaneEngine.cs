using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    [RequireComponent(typeof(FuelManager))]
    public class AirplaneEngine : MonoBehaviour
    {
        #region Variables

        [Header("Specyfikacja Silnika")]
        public float MaxEngineForce = 2500f;
        public float MaxEngineRotationPerMinute = 2500f;

        public AnimationCurve EngineThrothleCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        [Header("Śmigło Dla Silnika")]
        public AirplanePropeller enginePropeller;

        [Header("Paliwo Dla Silnika")]
        public FuelManager fuelManager;

        [Header("Połączenie z kontrolerem")]
        public AirplaneController airplaneController;


        //Okreslenie tempa zatrzymywania sie silnika,
        //tak aby nie zatrzymal sie nagle
        [Header("Prędkości Reakcji Silnika")]
        public float howFastEngineStop = 3f;
        public float howFastEngineBackRPMSToThrothlState = 3f;

        float rpmsWhenNotEngineStop = 0f;


        //Ostatnia wartosc otwarcia przepustnicy przed wylaczeniem silnika
        //po wylaczeniu silnika szybkosc obrotu smigla bedzie powoli malec
        private float savedLastThrothleValue;
        public float SavedLastThrothleValue
        {
            get { return savedLastThrothleValue; }
        }


        private bool isStopped;
        public bool IsStopped
        {
            get { return isStopped; }

            set
            {
                if (isStopped && fuelManager.FuelState > 0f && !airplaneController.IsAirplaneDestroyed)
                    isStopped = false;
                else
                    isStopped = true;
            }
        }

        private float rpms;
        public float RPMS
        {
            get { return rpms; }
        }

        private float destinationThrothle;
        public float DestinationThrothle
        {
            get { return destinationThrothle; }
        }
        #endregion


        #region BuiltInMethods
        void Start()
        {
            if (!fuelManager)
            {
                fuelManager = GetComponent<FuelManager>();
            }

            if (fuelManager)
            {
                fuelManager.InitFuelManager();
            }

        }

        void Update()
        {
            AirplaneDamageStateHandler();

        }


        private void AirplaneDamageStateHandler()
        {
            if (airplaneController)
            {

                if (airplaneController.IsAirplaneDestroyed)
                {
                    IsStopped = true;
                }
            }
        }
        #endregion


        #region MyOwnMethods
        public Vector3 CalcForce(float throthle)
        {
            destinationThrothle = Mathf.Clamp01(throthle);
            
            if (!IsStopped)
            {
                
                //Dorownywanie obrotow do wartosci przepustnicy
                destinationThrothle = Mathf.Lerp(savedLastThrothleValue, destinationThrothle, Time.deltaTime);

                //zapisywanie ostatniej wartosci przepustnicy
                //gdy silnik jeszcze dziala
                savedLastThrothleValue = destinationThrothle;

            }
            else
            {
                //Redukowanie Wartosci ciagu silnika gdy sie wylaczy - pomoznono przez 
                //howFastEngineStop zeby kontrolowac stopniowe zatrzymywanie
                savedLastThrothleValue -= Time.deltaTime * howFastEngineStop;

                //Ograniczenie aby wartosc ciagu nie zmalała ponizej zera
                savedLastThrothleValue = Mathf.Clamp01(savedLastThrothleValue);

                //spadajaca wartosc mocy gdy silnik sie zatrzymuje
                destinationThrothle = savedLastThrothleValue;
            }


            // Obliczanie obrotow silnika i tym samym smigla samolotu
            rpms = destinationThrothle * MaxEngineRotationPerMinute;

            if (enginePropeller)
            {
                if (isStopped)
                {
                    if (rpmsWhenNotEngineStop > 0)
                    {
                        rpmsWhenNotEngineStop -= Time.deltaTime * howFastEngineStop * 10000f;
                        enginePropeller.HandlePropeller(rpmsWhenNotEngineStop);
                    }
                }
                else
                {
                    rpmsWhenNotEngineStop = rpms;
                    enginePropeller.HandlePropeller(rpmsWhenNotEngineStop);
                }
            }

            //Spalanie paliwa przesyłajac do FuelManagera wartosc przepustnicy (od 0 do 1)
            if (fuelManager)
            {
                if (!isStopped)
                    fuelManager.UpdateAmountOfFuel(destinationThrothle);

                if (fuelManager.FuelState <= 0f)
                {
                    isStopped = true;
                }
            }

            // Nadanie wektora sily
            float destinationPower = destinationThrothle * MaxEngineForce;

            Vector3 destinationForce = transform.forward * destinationPower;

            //Debug.DrawRay(transform.position, destinationForce, Color.red);

            //----------------------------------------------------------------------------
            //Debug.Log("RPM = " + currentRPM + "  |  " + "Power = " + destinationPower);
            //Debug.Log("Engine power is:" + destinationPower);
            //----------------------------------------------------------------------------

            return destinationForce;
        }

        public void Refuel()
        {
            fuelManager.Refuel();
        }

        #endregion
    }
}