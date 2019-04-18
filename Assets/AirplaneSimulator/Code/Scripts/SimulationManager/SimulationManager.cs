using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AirPlaneSimulator
{
    public class SimulationManager : MonoBehaviour
    {

        [Header("Sterowanie Zdarzeniami")]
        public AirplaneController airplaneController;

        [Header("Czas oczekiwania po wypadku")]
        public float secondsToReaload = 3f;
        
        void Start()
        {

        }

        void Update()
        {
            if (airplaneController.IsAirplaneDestroyed)
            {
                StartCoroutine("ReloadLevelCoroutine");
            }
        }

        IEnumerator ReloadLevelCoroutine()
        {
            yield return new WaitForSeconds(secondsToReaload);
            SceneManager.LoadScene("AirplaneSimulator");
        }

    }
}