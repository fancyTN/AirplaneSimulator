using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class AirplaneEngineSound : MonoBehaviour
    {
        #region MyVariables
        [Header("Dźwięki Samolotu")]
        public AudioSource lowSpeedEngineSound;
        public AudioSource fullSpeedEngineSound;
        public AirplaneEngine engine;
        private float factorOfSound;
        #endregion

        #region BuiltIn Methods
        void Start()
        {
            if (fullSpeedEngineSound)
            {
                fullSpeedEngineSound.volume = 0f;
            }
        }
        void Update()
        {
            if (engine && fullSpeedEngineSound && lowSpeedEngineSound)
                SoundEngineHandler();
            else
                Debug.LogError("Brak Podlinkowanych Komponentow");
        }
        #endregion

        #region MyOwnMethods
        protected virtual void SoundEngineHandler()
        {
            factorOfSound = Mathf.Lerp(0f, 1f, engine.DestinationThrothle);
            if (engine.IsStopped)
            {
                fullSpeedEngineSound.volume = Mathf.Lerp(fullSpeedEngineSound.volume, 0f, Time.deltaTime);
                fullSpeedEngineSound.pitch = Mathf.Lerp(fullSpeedEngineSound.pitch, 0f, Time.deltaTime);
                lowSpeedEngineSound.pitch = Mathf.Lerp(lowSpeedEngineSound.pitch, 0f, Time.deltaTime);
                lowSpeedEngineSound.volume = Mathf.Lerp(lowSpeedEngineSound.volume, 0f, Time.deltaTime);
            }
            else
            {
                fullSpeedEngineSound.volume = factorOfSound;
                fullSpeedEngineSound.pitch = factorOfSound + 0.5f; //podwyższenie ogólnego tonu dźwięku
                lowSpeedEngineSound.pitch = 1 + factorOfSound;
                lowSpeedEngineSound.volume = 1 - factorOfSound;
            }
        }
        #endregion
    }
}
