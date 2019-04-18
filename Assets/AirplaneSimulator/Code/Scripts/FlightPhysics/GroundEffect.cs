using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class GroundEffect : MonoBehaviour
    {

        #region MyVariables
        private Rigidbody rb;
        public float maxHeightAboveTheGround = 5f;
        public float extraLiftForce = 100f;
        public float maxSpeedForMaxExtraLiftForce = 30f;
        #endregion
        
        #region BuiltInMethods
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            if (rb)
            {
                GroundEffectHandler();
            }
        }
        #endregion

        #region MyOwnMethods
        public virtual void GroundEffectHandler()
        {
            RaycastHit rayCastHit;
            //Mierzenie dystansu od ziemi, przechowywanie długości promienia w zmiennej rayCastHit
            if (Physics.Raycast(transform.position, Vector3.down, out rayCastHit))
                // jeśli kontakt z ziemią i odległosc mniejsza niz maksymalnie ustalona
                if (rayCastHit.transform.tag == "Ground" && rayCastHit.distance < maxHeightAboveTheGround)
                {
                    //Dodatkowa sila ciagu podczas niskiego lotu zalezna rowniez od predkosci
                    float curSpeed = rb.velocity.magnitude;
                    float normalizedSpeed = curSpeed / maxSpeedForMaxExtraLiftForce;
                    normalizedSpeed = Mathf.Clamp01(normalizedSpeed);
                    float height = maxHeightAboveTheGround - rayCastHit.distance;
                    float force = extraLiftForce * height * normalizedSpeed;
                    rb.AddForce(Vector3.up * force);
                }
        }
    }

    #endregion
}
