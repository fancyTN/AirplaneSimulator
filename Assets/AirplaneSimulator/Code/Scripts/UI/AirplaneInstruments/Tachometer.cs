using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class Tachometer : MonoBehaviour, AirplaneUI
    {
        #region Variables
        [Header("Właściwości Tachometru")]
        public AirplaneEngine engine;
        public RectTransform pointer;
        public float minRotationDegreeOnPointer;
        public float maxRotationDegreeOnPointer;
        #endregion

        #region InterfaceImplements 
        public void InitAirplaneUI()
        {
            if (engine && pointer)
            {
                float normalizedRPM = Mathf.InverseLerp(0f, engine.MaxEngineRotationPerMinute, engine.RPMS);
                //stopnie ustawiane wzgledem wskazowki interfejsu
                float neededRotation = maxRotationDegreeOnPointer * -normalizedRPM + 180;
                neededRotation = Mathf.Clamp(neededRotation, minRotationDegreeOnPointer, maxRotationDegreeOnPointer);
                pointer.rotation = Quaternion.Euler(0f, 0f, -neededRotation);
            }
            else
            {
                Debug.LogError("Brak podlinkowanych elementow do skryptu");
            }
        }
        #endregion
    }
}
