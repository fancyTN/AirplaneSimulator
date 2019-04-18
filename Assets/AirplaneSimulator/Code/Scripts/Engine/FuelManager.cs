using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public class FuelManager : MonoBehaviour
    {
        #region Variables
        [Header("Paliwo Samolotu")]
        
        //Pojemnosc Baku
        public float maxLiterFuel = 50f;

        //Srednie spalanie na godzine
        public float fuelConsumptionPerHour = 7f;
        
        private float fuelState;
        public float FuelState
        {
            get { return fuelState; }
        }

        private float normalizedFuelState;
        public float NormalizedFuelState
        {
            get { return normalizedFuelState; }
        }

        #endregion


        #region BuilInMethods
        void Start()
        {

        }
        
        void Update()
        {

        }
        #endregion


        #region Custom Methods
        public void InitFuelManager()
        {
            //na starcie maksymalna ilosc paliwa
            fuelState = maxLiterFuel;
        }

        public void UpdateAmountOfFuel(float throthleOfEngine)
        {
            //Dodanie wspolczynnika spalania dla silnika w spoczynku
            float idleFuelConsumption = 1f;

            //Spalanie na sekunde
            float spendedFuel = ((fuelConsumptionPerHour * throthleOfEngine + idleFuelConsumption) / 3600f) * Time.deltaTime;

            //odejmowanie paliwa
            fuelState -= spendedFuel;

            //nie dostaniemy nigdy ujemnej wartosci paliwa wyznaczajac przedzial wartosci ilosci paliwa
            fuelState = Mathf.Clamp(fuelState, 0f, maxLiterFuel);

            //normalizacja poziomu paliwa na wartosci od 0 do 1
            normalizedFuelState = fuelState / maxLiterFuel;

        }

        public void Refuel()
        {
            fuelState = maxLiterFuel;
        }
        #endregion
    }
}