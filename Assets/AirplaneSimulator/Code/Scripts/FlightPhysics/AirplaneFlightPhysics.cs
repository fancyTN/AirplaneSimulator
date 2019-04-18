using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class AirplaneFlightPhysics : MonoBehaviour
    {
        #region Variables
        private Rigidbody rigidBody;
        private BaseInput input;
        private float startDrag;
        private float startAngularDrag;
        private float MaxMetersSecond;
        private float normalizeKmh;

        // kat natarcia
        private float angleAttack;
        // kat nachlenia przod-tyl
        //private float pitchAngle;
        // kat nachylenia prawo-lewo
        private float rollAngle;


        [Header("Fizyka Lotu")]
        public float forwardSpeed;
        public float kmh;
        public float MaxKmh = 150f;
        public float pitchSensitive = 3000f;
        public float rollSensitive = 3000f;
        public float yawSensitive = 3000f;
        public float bankSensitive = 3000f;
        public float flapDragForce = 0.005f;


        // za pomoca tej krzywej interpoluje ile sily jest przykladanej do ciagu w gore
        public AnimationCurve liftCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);


        [Header("Własciwości Siły nośnej")]
        public float MaxLiftPower = 6000f;

        [Header("Własciwości Siły Oporu (Drag)")]
        public float DragFactor = 0.01f;

        #endregion


        #region BuiltInMethods
        void Start()
        {

        }


        void Update()
        {

        }
        #endregion


        #region MyOwnMethods
        public void InitFlightPhysics(Rigidbody rb, BaseInput input)
        {
            // Inicjowanie podstawowych elementow
            rigidBody = rb;
            this.input = input;


            // Wlasnosc drag moze sluzyc do zwalniania szybkosci ruchu obiektu
            // Ustawiania na przyklad oporu powietrza
            startDrag = rigidBody.drag;

            // Wlasnoscia DragAngular ustawiamy potrzebna sile do obrocenia obiektu
            startAngularDrag = rigidBody.angularDrag;

            // Wyznaczenie maksymalnej predkosci samolotu w metrach na sekunde
            MaxMetersSecond = (MaxKmh / (float)3.6);
        }

        public void UpdateFlightPhysics()
        {
            // Obliczanie Modelu lotu
            if (rigidBody)
            {
                CalcForwardsSpeed();
                CalcLift();
                CalcDrag();
                CalcRigidBodyTransform();

                CalcPitch();    // Pochylanie przod
                CalcRoll();     // Pochylanie na boki
                CalcYaw();      // Skrecanie na boki
                CalcBanking();  // Skrecanie przy pochyleniu
            }
        }

        private void CalcBanking()
        {
            float bankSide = Mathf.InverseLerp(-90f, 90f, rollAngle);
            float bankAmount = Mathf.Lerp(-1f, 1f, bankSide);
            Vector3 bankForqueForce = bankAmount * bankSensitive * transform.up;

            rigidBody.AddTorque(bankForqueForce);


        }

        private void CalcYaw()
        {
            Vector3 yawTorqueForce = input.Yaw * yawSensitive * transform.up;

            rigidBody.AddTorque(yawTorqueForce);
        }

        private void CalcRoll()
        {
            // przez prawostronny wektor nalezy rozumiec os X
            Vector3 rightOfObjectLocalVector = transform.right;

            rightOfObjectLocalVector.y = 0;
            rightOfObjectLocalVector = rightOfObjectLocalVector.normalized;

            //Signed Angle aby zwracac kat ze znakiem
            rollAngle = Vector3.SignedAngle(transform.right, rightOfObjectLocalVector, transform.forward);

            //siła obrotu wzgledem osi Y (transform.forward) czyli nachylenia samolotu lewo-prawo
            Vector3 rollTorqueForce = input.Roll * rollSensitive * transform.forward;

            rigidBody.AddTorque(rollTorqueForce);

        }

        private void CalcPitch()
        {
            // Lokalny Vector Kierunku przodu modelu
            Vector3 forwardObjectLocalVector = transform.forward;

            // ustawienie wartosci Y na zero
            forwardObjectLocalVector.y = 0;

            // normalizacja wektora - rowny zawsze 1
            forwardObjectLocalVector = forwardObjectLocalVector.normalized;

            // znalezienie kata pomiedzy lokalnym Z (Vectorem przodu modelu)
            // pozwoli to okreslic kat nachylenia samolotu wzgledem powierzchni
            //pitchAngle = Vector3.Angle(transform.forward, forwardObjectLocalVector);

            // Debug.Log("KAT NACHYLENIA " + pitchAngle);

            // moment obrotowy

            //siła obrotu wzgledem osi X (transform.right) czyli nachylenia samolotu do przodu i do tylu
            Vector3 pitchTorqueForce = input.Pitch * pitchSensitive * transform.right;
            rigidBody.AddTorque(pitchTorqueForce);

        }

        private void CalcRigidBodyTransform()
        {
            // jesli wielkosc wektora predkosci wieksza niz 
            if (rigidBody.velocity.magnitude > 1f)
            {
                //Dodaje Bardziej Arcade Mode Do Fizyki Lotu

                //Slerp - Smooth Lerp
                //Vector3 recalculatedVelocity = Vector3.Lerp(rigidBody.velocity,
                //                                       transform.forward * forwardSpeed,
                //                                        forwardSpeed * angleAttack * Time.deltaTime);
                //
                //rigidBody.velocity = recalculatedVelocity;
                //*/


                //Slerp - Smooth Lerp
                Quaternion recalculatedRotation = Quaternion.Lerp(rigidBody.rotation,
                                                                    Quaternion.LookRotation(rigidBody.velocity.normalized, transform.up),
                                                                    Time.deltaTime);

                rigidBody.MoveRotation(recalculatedRotation);
            }
        }

        void CalcForwardsSpeed()
        {
            Vector3 localSpeed = transform.InverseTransformDirection(rigidBody.velocity);

            // localSpeed.z - predkosc wzdluz wektora Z lokalnego
            forwardSpeed = Mathf.Max(0f, localSpeed.z);
            forwardSpeed = Mathf.Clamp(forwardSpeed, 0f, MaxMetersSecond);

            //zamiana metrow na sekunde na kilometry na godzine
            kmh = (forwardSpeed * (float)3.6);

            kmh = Mathf.Clamp(kmh, 0f, MaxKmh);
            normalizeKmh = Mathf.InverseLerp(0f, MaxKmh, kmh);

            //Debug.Log("FORWARD SPEED = " + forwardSpeed);
        }

        //Symulowanie cisnienia dzialajacego na skrzydla
        void CalcLift()
        {
            //Wykorzystano wlasnosci prawa Berouliego

            //Uzyskanie kata natarcia
            //pomiedzy wektorem predkosci komponentu rigidbody a wektorem Z
            //normalized - zawsze rowny dlugosci 1
            //Skutkiem tego jest powstanie oporu ciagnacego samolot w tyl jesli kat jest duzy

            angleAttack = Vector3.Dot(rigidBody.velocity.normalized, transform.forward);
            angleAttack *= angleAttack;

            Vector3 liftDirection = transform.up;

            // zaleznie od predkosci poruszania samolotu
            float liftPower = liftCurve.Evaluate(normalizeKmh) * MaxLiftPower;

            Vector3 finalLiftForce = liftDirection * liftPower * angleAttack;
            rigidBody.AddForce(finalLiftForce);

        }

        void CalcDrag()
        {
            //Zwiekszanie oporu podczas otwartych klap samolotu
            float dragOfFlaps = input.Flaps * flapDragForce;

            //Zwiększanie oporu wraz z predkoscia
            float speedDrag = forwardSpeed * DragFactor;

            //suma oporu
            float finalDrag = startDrag + speedDrag + dragOfFlaps;

            //zwiekszanie oporu ruchu oraz oporu nachylenia katowego z predkoscia
            rigidBody.drag = finalDrag;
            rigidBody.angularDrag = startAngularDrag * forwardSpeed;
        }


        #endregion
    }
}