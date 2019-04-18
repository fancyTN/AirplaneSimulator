using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class AirplanePropeller : MonoBehaviour
    {
        #region Variables
        [Header("Dostosowanie Efektów Śmigła")]
        public float MinPropellerRPM = 300f;
        public float MinBlurSwapToHigh = 600f;
        public GameObject basePropeller;
        public GameObject blurPropeller;
        public Material blurMat;
        public Texture2D blur1;
        public Texture2D blur2;
        #endregion

        #region Builtin Methods
        void Start()
        {
            //Zmiana rozmycia śmigła
            if (basePropeller && blurPropeller)
                PropellerSwap(0f);

        }

        void Update()
        {

        }

        #endregion


        #region MyOwnMethods
        public void HandlePropeller(float currentRpm)
        {
            //Obliczanie stopni obrotu na sekunde
            float degreePerSec = ((currentRpm * 360f) / 60f) * Time.deltaTime;

            //Obracanie smigla
            transform.Rotate(Vector3.forward, degreePerSec);

            //Zmiana rozmycia śmigła
            if (basePropeller && blurPropeller)
                PropellerSwap(currentRpm);
        }

        void PropellerSwap(float curRpm)
        {
            if (curRpm > MinPropellerRPM)
            {
                blurPropeller.gameObject.SetActive(true);
                basePropeller.gameObject.SetActive(false);

                if (blurMat && blur1 && blur2)
                {
                    if (curRpm > MinBlurSwapToHigh)
                        blurMat.SetTexture("_MainTex", blur2);
                    else
                        blurMat.SetTexture("_MainTex", blur1);
                }
            }
            else
            {
                blurPropeller.gameObject.SetActive(false);
                basePropeller.gameObject.SetActive(true);
            }
        }

        #endregion
    }
}