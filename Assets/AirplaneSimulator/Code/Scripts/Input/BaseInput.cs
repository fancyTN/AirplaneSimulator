using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AirPlaneSimulator
{
    public class BaseInput : MonoBehaviour
    {
        // Zmienne przechowujace informacje sterujace
        #region SterringVariables
        protected float pitch = 0f;
        protected float roll = 0f;
        protected float yaw = 0f; // Zmiana kierunku lotu, nachylenie lewo/prawo
        protected float throthle = 0f;
        protected int flaps = 0; // stopnie otwarcia klapy np 15 stopni, 30 stopni
        protected float brake = 0f;
        public int MaxFlapUp = 2;

        protected KeyCode cameraKey = KeyCode.C;
        protected bool cameraSwitch = false;

        // Stopnień Szybkosci zmiany predkosci
        public float throthleSpeed = 0.1f;

        protected float currentStickyThrothle;
        public float CurrentStickyThrothle
        {
            get { return currentStickyThrothle; }
        }

        #endregion


        #region SteeringProperties
        public float Pitch
        {
            get { return pitch; }
        }
        public float Roll
        {
            get { return roll; }
        }
        public float Yaw
        {
            get { return yaw; }
        }
        public float Throthle
        {
            get { return throthle; }
        }
        public int Flaps
        {
            get { return flaps; }
        }
        public float Brake
        {
            get { return brake; }
        }
        public bool CameraSwitch
        {
            get { return cameraSwitch; }
        }
        #endregion


        #region BuiltinMethods
        void Start()
        {

        }

        void Update()
        {
            //Obsługa wejscia
            InputHandler();

            //Obsluga plynnej przepustnicy
            CurrentStickyThrothleControl();

            //Ograniczenie wspolczynnikow sterowania do  wartosci granicznych
            InputValuesClamp();
        }
        #endregion


        #region MyOwnMethods
        protected virtual void InputHandler()
        {
            //Sterowanie 
            pitch = Input.GetAxis("Vertical");
            roll = Input.GetAxis("Horizontal");
            yaw = Input.GetAxis("Yaw");
            throthle = Input.GetAxis("Trothle");

            //Hamulec Kołowy
            brake = Input.GetKey(KeyCode.Space) ? 1f : 0f;

            //Zamykanie i otwieranie klap
            if (Input.GetKeyDown(KeyCode.PageUp)) flaps++;
            if (Input.GetKeyDown(KeyCode.PageDown)) flaps--;

            //Ograniczenie zakresu zmiennej
            flaps = Mathf.Clamp(flaps, 0, MaxFlapUp);

            //Zmiana Kamery
            cameraSwitch = Input.GetKeyDown(cameraKey) || Input.GetButtonDown("CameraSwitchXboxButton");

        }

        //Metoda która utrzymuje przedzial wartosci sterujacych w odpowiednich granicach
        //Ponieważ sterowanie jest polaczone i wartosci mogą byc podwojnie wieksze na skutek
        //Wciskania na przyklad przepustnicy na kontrolerze Xbox oraz klawiaturze
        protected void InputValuesClamp()
        {
            pitch = Mathf.Clamp(pitch, -1f, 1f);
            roll = Mathf.Clamp(roll, -1f, 1f);
            yaw = Mathf.Clamp(yaw, -1f, 1f);
            roll = Mathf.Clamp(roll, -1f, 1f);
            throthle = Mathf.Clamp(throthle, -1f, 1f);
        }

        // Płynna przepustnica
        protected virtual void CurrentStickyThrothleControl()
        {
            currentStickyThrothle = currentStickyThrothle + (-throthle * throthleSpeed * Time.deltaTime);
            currentStickyThrothle = Mathf.Clamp01(currentStickyThrothle);
        }

        #endregion
    }

}