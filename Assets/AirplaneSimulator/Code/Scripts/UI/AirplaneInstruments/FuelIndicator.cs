using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class FuelIndicator : MonoBehaviour, AirplaneUI
    {
        #region MyVariables
        [Header("Wskaźnik Paliwa")]
        public RectTransform fuelPointer;
        public FuelManager fuel;
        public float MinAngleOfPointerOnIndicator = 85f;
        public float MaxAngleOfPointerOnIndicator = -85f;
        #endregion
        
        #region InterfaceImplements 
        public void InitAirplaneUI()
        {
            if (fuel && fuelPointer)
            {
                //stopnie ustawiane wzgledem wskazowki interfejsu
                float neededRotation = MaxAngleOfPointerOnIndicator * fuel.NormalizedFuelState;
                neededRotation = Mathf.Clamp(neededRotation, MaxAngleOfPointerOnIndicator, MinAngleOfPointerOnIndicator);
                fuelPointer.rotation = Quaternion.Euler(0f, 0f, neededRotation);
            }
            else
            {
                Debug.LogError("Brak podlinkowanych elementow do skryptu");
            }
        }
        #endregion
    }
}