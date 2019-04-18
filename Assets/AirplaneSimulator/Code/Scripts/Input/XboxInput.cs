using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AirPlaneSimulator
{
    public class XboxInput : BaseInput
    {
        #region MyVariables

        #endregion

        #region MyOwnMethods
        protected override void InputHandler()
        {
            base.InputHandler();

            pitch += Input.GetAxis("XboX_Vertical");
            roll += Input.GetAxis("XboX_Horizontal");
            yaw += Input.GetAxis("X_RH_Grzybek");
            throthle += Input.GetAxis("X_RV_Grzybek");

            brake += Input.GetAxis("XboxBrakeButton");

            //Zamykanie i otwieranie klap
            if (Input.GetButtonDown("X_L_Bumper")) flaps++;
            if (Input.GetButtonDown("X_R_Bumper")) flaps--;

            //Ograniczenie zakresu zmiennej
            flaps = Mathf.Clamp(flaps, 0, MaxFlapUp);

        }
        #endregion

    }
}
