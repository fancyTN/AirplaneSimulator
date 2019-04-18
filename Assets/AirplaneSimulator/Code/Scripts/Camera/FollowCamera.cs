using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AirPlaneSimulator
{
    public class FollowCamera : MonoBehaviour
    {
        #region Variables
        [Header("Obiekt Śledzony")]
        public Transform objToTrack;
        public float viewDistance = 15f;
        public float viewHeightPos = 5f;
        public float speedOfCameraMovement = 0.5f;
        [Range(1, 360)]
        public float AngleOfRotation = 100;
        public AirplaneController airplaneController;
        [Header("Czy to kamera wewnątrz pojazdu?")]
        public bool isCockpitCamera;
        private Vector3 CameraMovementVectorSmooth;
        private float mouseX = 0.0f;
        #endregion

        void FixedUpdate()
        {
            if (objToTrack && airplaneController)
            {
                if (isCockpitCamera)
                    UpdateFirstPersonCamera();
                else
                    UpdateThirdPersonCamera();
                MouseUpdate();
            }
        }
        private void MouseUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                mouseX += Input.GetAxis("Mouse X");
                mouseX = Mathf.Clamp(mouseX, -45f, 45f);
            }
            else
            {
                mouseX = Mathf.Lerp(mouseX, 0f, Time.deltaTime * 0.5f);
            }
        }

        #region MyOwnImplements
        void UpdateThirdPersonCamera()
        {
            Vector3 finalCameraPos =
                objToTrack.position +
                (-objToTrack.forward * viewDistance) +
                (Vector3.up * viewHeightPos);
            // ref oznacza, ze funkcja zwraca referencje
            if (airplaneController.IsAirplaneDestroyed)
            {
                transform.position =
                    Vector3.SmoothDamp
                    (transform.position, new Vector3(0f, 300f, 0f),
                    ref CameraMovementVectorSmooth, speedOfCameraMovement);
                transform.LookAt(objToTrack);
            }
            else
            {
                transform.position = finalCameraPos;
                transform.LookAt(objToTrack);
                transform.RotateAround
                    (objToTrack.transform.position,
                    new Vector3(0f, 1f, 0f),
                    mouseX * Time.deltaTime * AngleOfRotation);
                transform.position =
                    Vector3.SmoothDamp
                    (transform.position, finalCameraPos,
                    ref CameraMovementVectorSmooth, speedOfCameraMovement);
            }
        }
        void UpdateFirstPersonCamera()
        {
            if (airplaneController.IsAirplaneDestroyed)
            {
                transform.position =
                    Vector3.SmoothDamp(transform.position,
                    new Vector3(0f, 300f, 0f),
                    ref CameraMovementVectorSmooth, speedOfCameraMovement);
                transform.LookAt(objToTrack);
            }
        }
        #endregion
    }
}