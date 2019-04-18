using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace AirPlaneSimulator
{
    public class InterfaceMainController : MonoBehaviour
    {
        #region Variables
        public List<AirplaneUI> interfaceItems = new List<AirplaneUI>();
        #endregion

        void Start()
        {
            //Wykorzystanie funkcji ToList z Biblioteki Linq
            //Pobranie do listy elementow w glab hierarchii z zaimplementowanym interfejsem AurplaneUI 
            interfaceItems = transform.GetComponentsInChildren<AirplaneUI>().ToList<AirplaneUI>();
        }

        void Update()
        {
            //Aktualizowanie interfejsu
            if (interfaceItems.Count > 0)
            {
                foreach (AirplaneUI uis in interfaceItems)
                {
                    uis.InitAirplaneUI();
                }
            }
        }
    }
}