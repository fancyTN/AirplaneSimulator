using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class Attitudemeter : MonoBehaviour, AirplaneUI
    {
        #region Variables
        [Header("Sztuczny horyzont")]
        public RectTransform horizon;
        public RectTransform arrowPointer;
        public AirplaneController airplaneController;

        #endregion

        #region InterfaceImplements
        public void InitAirplaneUI()
        {
            if (airplaneController && horizon && arrowPointer)
            {
                //Wyznacznie przechyłu (wartosci kata nachylenia obiektu samolotu) 
                //wzgledem osi Z, czyli przechylu dziobu w gore/dol do wektora bezwzględnego w górę
                //I pomnożenie w celu konwersji kata w radianach na stopnie
                float pitchAngle = Vector3.Dot(airplaneController.transform.forward, Vector3.up) * Mathf.Rad2Deg;

                //Wyznacznie przechyłu (wartosci obrotu obiektu samolotu) 
                //wzgledem osi X, czyli przechylu na boki
                //I pomnożenie w celu konwersji kata w radianach na stopnie
                float rollAngle = Vector3.Dot(airplaneController.transform.right, Vector3.up) * Mathf.Rad2Deg;

                //Obrocenie wskaznika horyzontu oraz strzalki wartosci przechylu
                horizon.transform.rotation = arrowPointer.transform.rotation = Quaternion.Euler(0f, 0f, rollAngle);

                //przesuniecie wskaznika horyzontu
                horizon.anchoredPosition = new Vector3(0f, pitchAngle, 0f);
            }
            else
            {
                Debug.LogError("Brak podlinkowanych elementow do skryptu");
            }
        }
        #endregion
    }
}