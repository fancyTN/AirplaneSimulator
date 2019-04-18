using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    //Adnotacja wymusza obecnosc komponentu gdy skrypt przypinamy do obiektu
    //Dodajac skrypt automatycznie doda komponent 

    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Rigidbody))]
    public class BaseRigidbodyController : MonoBehaviour
    {

        #region ImportantVariables
        protected Rigidbody rigidBody;
        protected AudioSource audioSource;
        #endregion


        #region BuiltInMethods
        public virtual void Start()
        {
            rigidBody = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();


            if (audioSource != null)
                audioSource.playOnAwake = false;
            else
                Debug.LogError("Brak komponentu AudioSource");

            if (rigidBody == null)
            {
                Debug.LogError("Brak komponentu RigidBody");
            }
        }

        //FixUpdate aproksymuje zdarzenia fizyczne tworząc plynne przejscia transformacji obiektow
        void FixedUpdate()
        {
            if (rigidBody)
                PhysicsHandler();
        }
        #endregion


        #region MyOwnMethods
        protected virtual void PhysicsHandler() { }
        #endregion
    }
}