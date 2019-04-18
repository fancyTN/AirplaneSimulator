using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class WheelUp : MonoBehaviour
    {
        #region Variables
        [Header("Chowanie podwozia")]
        public Transform wheelKit;
        public Transform airplane;
        public float hideAngle;
        public float openAngle;
        public float speedOfHide;
        public float heightOfAirplaneToHide;
        #endregion

        #region BuiltInMethods
        void Update()
        {
            CheckToHideChassis();
        }


        #endregion

        #region MyOwnImplements
        private void CheckToHideChassis()
        {
            if (wheelKit && airplane)
            {
                if (airplane.position.y <= heightOfAirplaneToHide)
                {
                    float rot = Mathf.Lerp(0f, hideAngle, Time.deltaTime * speedOfHide);
                    wheelKit.transform.localRotation = Quaternion.Euler(0f, 0f, rot);

                }
                else
                {
                    float rot = Mathf.Lerp(hideAngle, 0f, Time.deltaTime * speedOfHide);
                    wheelKit.transform.localRotation = Quaternion.Euler(0f, 0f, rot);
                }
            }
            else
            {
                Debug.LogError("Brak podlinkowanych elementow do skryptu");
            }
        }
        #endregion
    }
}