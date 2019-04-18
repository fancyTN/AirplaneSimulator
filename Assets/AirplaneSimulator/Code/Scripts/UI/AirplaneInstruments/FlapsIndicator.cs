using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class FlapsIndicator : MonoBehaviour, AirplaneUI
    {
        #region Variables
        [Header("Wskaźnik Stanu Klap")]
        public BaseInput input;
        public RectTransform pointer;
        public float maxIndicatorDegree = 180f;
        public float minIndicatorDegree = 0f;
        public float anglePerFlapsUnit = 90f;
        #endregion

        #region InterfaceImplements
        public void InitAirplaneUI()
        {
            if (input && pointer)
            {
                //stopnie ustawiane wzgledem wskazowki interfejsu
                float neededRotation = anglePerFlapsUnit * input.Flaps;
                neededRotation = Mathf.Clamp(neededRotation, minIndicatorDegree, maxIndicatorDegree);
                pointer.rotation = Quaternion.Lerp(pointer.rotation, Quaternion.Euler(0f, 0f, -neededRotation), Time.deltaTime);
                //Debug.Log("Kąt otwarcia klap: " + 30f*input.Flaps);
            }
            else
            {
                Debug.LogError("Brak podlinkowanych elementow do skryptu");
            }
        }
        #endregion

    }
}