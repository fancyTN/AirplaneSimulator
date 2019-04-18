using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; // Dodanie

namespace AirPlaneSimulator
{
    //Dziedziczenie z edytora - Obiekt w inspektorze pozawalajacy na latwa edycje parametrow

    [CustomEditor(typeof(BaseInput))]
    public class BaseInputEditor : Editor
    {

        #region ImportantVariables
        private BaseInput destinationInput;


        #endregion


        #region BuiltinMethods

        

        private void OnEnable()
        {
            destinationInput = (BaseInput)target; // rzutowanie jako obiekt do Inspekcji
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            //Własny Kod edytora
            EditorGUILayout.Space();
            string DebugParameters = "";
            DebugParameters = "Pitch:\t" + destinationInput.Pitch +
                                    "\nYaw:\t" + destinationInput.Yaw +
                                    "\nThrothle:\t" + destinationInput.Throthle +
                                    "\nRoll:\t" + destinationInput.Roll +
                                    "\nFlaps:\t" + destinationInput.Flaps +
                                    "\nBrake:\t" + destinationInput.Brake;

            EditorGUILayout.TextArea(DebugParameters, GUILayout.Height(100f));
            EditorGUILayout.Space();
            
            Repaint(); //Rysuje od nowa Inspector

        }

        void Start()
        {

        }

        void Update()
        {

        }
        #endregion
    }
}