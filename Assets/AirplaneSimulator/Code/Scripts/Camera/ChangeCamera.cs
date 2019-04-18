using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class ChangeCamera : MonoBehaviour
    {
        #region MyVariables
        [Header("Właściwości Kamery")]
        public BaseInput input;
        public List<Camera> cameras = new List<Camera>();
        public int defaultCamera = 0;
        private int cameraSwitchingCounter = 0;
        #endregion


        #region Built In Methods
        void Start()
        {

            if (defaultCamera > 0 && defaultCamera < cameras.Count)
            {
                ActiveCamera(defaultCamera);
            }
        }

        void Update()
        {
            if (input)
            {
                if (input.CameraSwitch)
                {
                    SwitchCameraHander();
                }
            }
        }
        #endregion


        #region MyOwnMethods
        protected virtual void SwitchCameraHander()
        {
            if (cameras.Count > 0)
            {

                cameraSwitchingCounter++;

                if (cameraSwitchingCounter >= cameras.Count)
                {
                    cameraSwitchingCounter = 0;
                }

                ActiveCamera(cameraSwitchingCounter);

            }
        }

        private void ActiveCamera(int index)
        {
            foreach (Camera c in cameras)
            {
                c.enabled = false;
                if (c.GetComponent<AudioListener>())
                    c.GetComponent<AudioListener>().enabled = false;
            }

            cameras[index].enabled = true;
            if (cameras[index].GetComponent<AudioListener>())
                cameras[index].GetComponent<AudioListener>().enabled = true;
        }

        #endregion
    }
}