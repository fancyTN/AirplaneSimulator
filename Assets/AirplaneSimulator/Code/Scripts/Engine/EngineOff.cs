using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EngineOff : MonoBehaviour
{

    #region MyVariables
    [Header("Wyłącznik Silnika")]
    public KeyCode engineOffKey = KeyCode.P;
    //lista zdarzen ktore stana sie gdy silnik sie wylaczy
    //do zainicjowania w inspektorze
    public UnityEvent onEngineOff = new UnityEvent();
    #endregion

    #region BuiltIn
    private void Update()
    {
        if (Input.GetKeyDown(engineOffKey))
        {
            EngineOffHandler();
        }
    }

    #endregion


    #region MyOwnImplements
    void EngineOffHandler()
    {
        if (onEngineOff != null)
        {
            //Wywołanie zdarzeń z listy po wcisnieciu klawisza
            onEngineOff.Invoke();
        }
    }
    #endregion

}
