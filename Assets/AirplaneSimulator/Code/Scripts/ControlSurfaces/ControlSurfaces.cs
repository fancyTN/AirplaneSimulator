using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public enum ControlSurfaceType
    {
        RUDDER,     //STER PIONOWY
        ELEVATOR,   //STATECZNIK POZIOMY NA SKRZYDLE
        FLAP,       //KLAPY
        AILERON     //STATECZNIK POZIOMY NA OGONIE
    }
    public class ControlSurfaces : MonoBehaviour
    {

        #region MyOwnVariables
        //Możliwość wyboru typu statecznika podczas dodawania do obiektu
        [Header("Właściwości Stateczników")]
        public ControlSurfaceType type = ControlSurfaceType.RUDDER;
        public float maxAngle = 40f;
        public float finalAngle = 0f;
        public Transform objToControl;
        public Vector3 axisOfRotation = Vector3.right;
        public float slowSpeedOfRotation = 4f;
        public float offsetAngle;
        #endregion

        #region BuiltInMethods
        void Update()
        {
            if (objToControl)
            {
                Vector3 finalAngleAxis = axisOfRotation * finalAngle;
                //plynna Interpolacja obrotu powierzchni sterującej w czasie
                objToControl.localRotation =
                    Quaternion.Slerp(objToControl.localRotation,
                    Quaternion.Euler(finalAngleAxis), Time.smoothDeltaTime);
            }
        }
        #endregion

        #region MyOwnMethods
        public void ControlSurfaceInit(BaseInput input)
        {
            float inputValue = 0f;
            switch (type)
            {
                case ControlSurfaceType.RUDDER:
                    inputValue = input.Yaw;
                    break;
                case ControlSurfaceType.AILERON:
                    inputValue = input.Roll;
                    break;
                case ControlSurfaceType.ELEVATOR:
                    inputValue = input.Pitch;
                    break;
                case ControlSurfaceType.FLAP:
                    inputValue = input.Flaps;
                    break;
                default:
                    break;
            }
            finalAngle = maxAngle * inputValue + offsetAngle;
        }
        #endregion
    }
}