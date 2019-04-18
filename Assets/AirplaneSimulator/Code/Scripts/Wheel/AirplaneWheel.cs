using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    [RequireComponent(typeof(WheelCollider))]
    public class AirplaneWheel : MonoBehaviour
    {
        #region Variables
        [Header("Właściwości Kół Samolotu")]
        public Transform wheel3DObj;
        private WheelCollider wCol;
        private Vector3 positionInWorld;
        private Quaternion rotationInWorld;

        public bool isBraking = false;
        public float brakeStrength = 1f;
        private float finalBrakeStrength;

        public bool isSteering = false;
        public float steerAngle = 25f;
        private float finalSteeringAngle;
        #endregion

        #region BuiltInMethods
        public void Start()
        {
            wCol = GetComponent<WheelCollider>();

            UnlockWheel();
        }
        #endregion

        #region MyOwnMethods
        public void UnlockWheel()
        {
            if (wCol)
            {
                wCol.motorTorque = 0.00000000000001f; // Na starcie kola nie beda zablokowane
            }
        }

        public void WheelHandler(BaseInput input)
        {
            if (wCol)
            {
                wCol.GetWorldPose(out positionInWorld, out rotationInWorld);

                if (wheel3DObj)
                {
                    wheel3DObj.rotation = rotationInWorld;
                    wheel3DObj.position = positionInWorld;
                }

                if (isBraking)
                {
                    if (input.Brake > 0.1f)
                    {
                        finalBrakeStrength = Mathf.Lerp(finalBrakeStrength, input.Brake * brakeStrength, Time.deltaTime);
                        wCol.brakeTorque = finalBrakeStrength;
                    }
                    else
                    {
                        finalBrakeStrength = 0f;
                        wCol.motorTorque = 0.00000000000001f;
                        wCol.brakeTorque = 0f;
                    }
                }

                if (isSteering)
                {
                    finalSteeringAngle = Mathf.Lerp(finalSteeringAngle, input.Yaw * steerAngle, Time.deltaTime);
                    wCol.steerAngle = -finalSteeringAngle; // inversja odwrotu
                }
            }
        }
        #endregion
    }
}