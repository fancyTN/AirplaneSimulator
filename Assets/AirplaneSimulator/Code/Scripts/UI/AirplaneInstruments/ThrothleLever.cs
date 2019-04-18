using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class ThrothleLever : MonoBehaviour, AirplaneUI
    {

        #region Variables
        [Header("Przepustnica")]
        public UnityEngine.UI.Scrollbar scrollBar;
        public BaseInput input;

        #endregion

        #region InterfaceImplements
        public void InitAirplaneUI()
        {
            if (input && scrollBar)
            {
                scrollBar.value = input.CurrentStickyThrothle;
				///Debug.Log("Poziom otwarcia przepustnicy: "+input.CurrentStickyThrothle);
            }
            else
            {
                Debug.LogError("Brak podlinkowanych elementow do skryptu");
            }
        }
        #endregion
    }
}