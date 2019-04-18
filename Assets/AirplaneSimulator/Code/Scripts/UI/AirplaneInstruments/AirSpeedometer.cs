using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class AirSpeedometer : MonoBehaviour, AirplaneUI
    {

        #region Variables
        [Header("Prędkościomierz")]
        public AirplaneFlightPhysics flightPhysics;
        public RectTransform pointer;
        public float minAngleDegrees = 142f;
        public float maxAngleDegrees = -145f;
        public float maxKMHOnIndicator = 200f;
        public float angleBetwenMinMaxOnIndicator = 287f;
        #endregion

        #region InterfaceImplements
        public void InitAirplaneUI()
        {
            if (flightPhysics && pointer)
            {
                float normalizedKMH = Mathf.InverseLerp(0f, maxKMHOnIndicator, flightPhysics.kmh);
                float neededRotation = angleBetwenMinMaxOnIndicator * normalizedKMH + maxAngleDegrees;
                pointer.rotation = Quaternion.Euler(0f, 0f, -neededRotation);
				//Debug.Log("Prędkość lotu: " + flightPhysics.kmh);
            }
            else
            {
                Debug.LogError("Brak podlinkowanych elementow do skryptu");
            }
        }
        #endregion
    }
}
