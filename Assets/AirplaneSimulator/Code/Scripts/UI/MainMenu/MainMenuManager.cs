using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AirPlaneSimulator
{
    public enum AIRPLANE_MODEL { CESSNA_172, ISKIERKA }

    public class MainMenuManager : MonoBehaviour
    {
        #region Variables
        [Header("Właściwości Menu")]
        public Canvas airplaneInterface;
        public Canvas mainMenu;
        public AirplaneController airplaneController;
        public AirplaneEngine airplaneEngine;
        public FuelManager fuel;
        public AirplaneFlightPhysics airplanePhysics;

        public UnityEngine.UI.Slider weightOfAirplaneSlider;
        public UnityEngine.UI.Text weightOfAirplaneText;
        public UnityEngine.UI.Slider engineForceSlider;
        public UnityEngine.UI.Text engineForceText;
        public UnityEngine.UI.Slider fuelConsumptionSlider;
        public UnityEngine.UI.Text fuelConsumptionText;
        public UnityEngine.UI.Slider fuelCapacitySlider;
        public UnityEngine.UI.Text fuelCapacityText;
        public UnityEngine.UI.Slider liftForceSlider;
        public UnityEngine.UI.Text liftForceText;
        public UnityEngine.UI.Slider steeringForceSlider;
        public UnityEngine.UI.Text steeringForceText;

        private bool isGamePaused;
        #endregion


        #region BuiltIn Methods
        // Use this for initialization
        void Start()
        {
            StartSimulation(false);
            //airplaneInterface.enabled = false;
        }

        // Update is called once per frame
        void Update()
        {

            MenuHandler();

            mainMenu.enabled = isGamePaused ? true : false;

            if (airplaneController)
            {

                SliderWeightUpdate();
                if (airplaneEngine)
                    SliderEngineForceUpdate();
                if (fuel)
                {
                    SliderFuelUpdate();
                    SliderFuelCapacityUpdate();
                }
                if (airplanePhysics)
                {
                    SliderLiftUpdate();
                    SliderSteeringForceUpdate();
                }
            }
        }

        #endregion


        #region MyOwnImplements

        public void ExitSimulation()
        {
            Application.Quit();
        }
        private void MenuHandler()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StartSimulation(isGamePaused);
            }

            if (airplaneController)
            {
                if (airplaneController.IsAirplaneDestroyed)
                {
                    airplaneInterface.enabled = false;
                }
                else if (!isGamePaused)
                {
                    airplaneInterface.enabled = true;
                }
            }
        }

        public void StartSimulation(bool pause)
        {
            if (pause)
            {
                //Debug.Log("Gra");
                Time.timeScale = 1f;
                isGamePaused = false;
                AudioListener.volume = 1f;
                mainMenu.enabled = false;
                airplaneInterface.enabled = true;
            }
            else
            {
                //Debug.Log("Pauza");
                Time.timeScale = 0f;
                isGamePaused = true;
                AudioListener.volume = 0f;
                mainMenu.enabled = true;
                airplaneInterface.enabled = false;
            }
        }

        public void SliderWeightUpdate()
        {
            if (weightOfAirplaneSlider && weightOfAirplaneText)
            {
                airplaneController.WeightOfAirplane = weightOfAirplaneSlider.value;
                weightOfAirplaneText.text = "Waga: " + weightOfAirplaneSlider.value.ToString() + " Kg";
            }
        }

        public void SliderEngineForceUpdate()
        {
            if (engineForceSlider && engineForceText)
            {
                airplaneEngine.MaxEngineForce = engineForceSlider.value * 60f;
                engineForceText.text = "Moc silnika: " + engineForceSlider.value.ToString() + " KM";
            }
        }

        public void SliderFuelUpdate()
        {
            if (fuelConsumptionSlider && fuelConsumptionText)
            {
                fuel.fuelConsumptionPerHour = fuelConsumptionSlider.value;
                fuelConsumptionText.text = "Spalanie na godzinę: " + fuelConsumptionSlider.value.ToString() + " L";
            }
        }

        private void SliderFuelCapacityUpdate()
        {
            if (fuelCapacitySlider && fuelCapacityText)
            {
                fuel.maxLiterFuel = fuelCapacitySlider.value;
                fuelCapacityText.text = "Pojemność baku: " + fuelCapacitySlider.value.ToString() + " L";
            }
        }


        private void SliderSteeringForceUpdate()
        {
            if (steeringForceSlider && steeringForceText)
            {
                airplanePhysics.bankSensitive =
                airplanePhysics.rollSensitive =
                airplanePhysics.pitchSensitive =
                airplanePhysics.yawSensitive = steeringForceSlider.value;

                steeringForceText.text = "Siły sterujące: " + steeringForceSlider.value.ToString();
            }
        }

        private void SliderLiftUpdate()
        {
            if (liftForceSlider && liftForceText)
            {
                airplanePhysics.MaxLiftPower = liftForceSlider.value * 500;
                liftForceText.text = "Powierzchnia nośna: " + liftForceSlider.value.ToString() + " m2";
            }
        }

        public void LoadPredefineSettings(int model)
        {
            if (weightOfAirplaneSlider &&
                engineForceSlider &&
                fuelConsumptionSlider &&
                fuelCapacitySlider &&
                liftForceSlider &&
                steeringForceSlider)
            {
                
                switch (model)
                {
                    case (int)AIRPLANE_MODEL.CESSNA_172:

                        weightOfAirplaneSlider.value = 757;
                        engineForceSlider.value = 180;
                        fuelConsumptionSlider.value = 40;
                        fuel.maxLiterFuel = fuelCapacitySlider.value = 213;
                        liftForceSlider.value = 16;
                        steeringForceSlider.value = 5500;
                        fuel.Refuel();
                        break;

                    case (int)AIRPLANE_MODEL.ISKIERKA:

                        weightOfAirplaneSlider.value = 960;
                        engineForceSlider.value = 300;
                        fuelConsumptionSlider.value = 91;
                        fuel.maxLiterFuel = fuelCapacitySlider.value = 369;
                        liftForceSlider.value = 14;
                        steeringForceSlider.value = 5500;
                        fuel.Refuel();
                        break;
                }
            }
        }
        #endregion
    }

}