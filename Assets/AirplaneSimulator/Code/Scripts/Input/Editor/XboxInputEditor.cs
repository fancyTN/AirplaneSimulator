using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AirPlaneSimulator
{

    [CustomEditor(typeof(XboxInput))]
    public class XboxInputEditor : Editor
    {

        #region ImportantVariables
        private XboxInput destinationInput;
        #endregion


        #region BuiltinMethods

        private void OnEnable()
        {
            destinationInput = (XboxInput)target; // rzutowanie jako obiekt do Inspekcji
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
