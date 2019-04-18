using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class DamageMeter : MonoBehaviour, AirplaneUI
    {
        #region MyVariables
        [Header("Wskaźnik Uszkodzeń")]
        public AirplaneController airplaneController;
        public RectTransform pointer;
        public float XpositionPointerOnMinDamage = 0;
        public float XpositionPointerOnMaxDamage = 51;
        #endregion

        public void InitAirplaneUI()
        {
            if (pointer && airplaneController)
            {
                pointer.anchoredPosition =
                    new Vector3(XpositionPointerOnMinDamage + XpositionPointerOnMaxDamage * airplaneController.NormalizedAirplaneHealth, 0f, 0f);

                //Debug.Log("Uszkodzenie:" + airplaneController.NormalizedAirplaneHealth);
            }
        }

    }
}
