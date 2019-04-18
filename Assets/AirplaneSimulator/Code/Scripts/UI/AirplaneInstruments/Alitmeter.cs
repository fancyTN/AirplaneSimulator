using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class Alitmeter : MonoBehaviour, AirplaneUI
    {

        #region Variables
        [Header("Właściwości Wysokościomierza")]
        public AirplaneController airplaneController;
        public RectTransform pointer100Meters;
        public RectTransform pointer1000Meters;
        #endregion


        #region MyOwnImplementations 
        public void InitAirplaneUI()
        {
            if (airplaneController && pointer1000Meters && pointer100Meters)
            {
                float currentHeight = airplaneController.HeightOverTheTerrain;

                float height1000Meters = currentHeight / 1000f;

                //Ograniczenie aby liczba kilometrow wysokosci nie byla wieksza niz 10
                height1000Meters = Mathf.Clamp(height1000Meters, 0f, 10f);

                //Obiciecie czesci tysiecy
                float height100Meters = currentHeight - (Mathf.Floor(height1000Meters) * 1000f);

                //Ograniczenie aby liczba metrow wysokosci nie byla wieksza niz 1000
                height100Meters = Mathf.Clamp(height100Meters, 0f, 1000f);


                float normalized1000 = Mathf.InverseLerp(0f, 10f, height1000Meters);
                float rotation1000Meters = 360f * normalized1000 + 180f;
                pointer1000Meters.rotation = Quaternion.Euler(0f, 0f, rotation1000Meters);

                float normalized100 = Mathf.InverseLerp(0f, 1000f, height100Meters);
                float rotation100Meters = 360f * normalized100 + 180f;
                pointer100Meters.rotation = Quaternion.Euler(0f, 0f, rotation100Meters);
                //Debug.Log("Wysokość nad powierzchnią " + airplaneController.HeightOverTheTerrain);

            }
            else
            {
                Debug.LogError("Brak podlinkowanych elementow do skryptu");
            }
        }
        #endregion
    }
}