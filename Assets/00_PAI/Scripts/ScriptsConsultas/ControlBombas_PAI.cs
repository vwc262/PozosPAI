using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

public class ControlBombas_PAI : ControlBombas
{
    public PlayMakerFSM controlBombasFSM;
    
    private void Start()
    {
        if (ControlUpdateUI._singletonExists)
            ControlUpdateUI.singleton.SitioSeleccionadoSitioGPS.AddListener(UpdateInfoSitio);
    }
    
    [Button]
    public override void SendCommand()
    {
        if (ControlLogin._singletonExists)
        {
            SetCommandValues(ControlLogin.singleton.login.Credencials.usuario, sitioSeleccionado.MyDataSitio.idSitio % 100, sitioSeleccionado.indexBomba, action);
            StartCoroutine(DoRequest());
        }
    }
    
    public override IEnumerator DoRequest()
    {
        if (!simulaSendCommand)
        {
            if (RequestAPI.Instance != null)
            {
                UnityWebRequest unityWebRequest = null;
                Debug.Log(RequestAPI.Instance.GetAddressByMethod(Metodos.SendCommand, sitioSeleccionado.MyDataSitio.Estructura));
                unityWebRequest = UnityWebRequest.Post(RequestAPI.Instance.GetAddressByMethod(Metodos.SendCommand, sitioSeleccionado.MyDataSitio.Estructura),
                    JsonUtility.ToJson(commandVWC), "application/json");

                yield return unityWebRequest.SendWebRequest();

                CallBack(unityWebRequest);
            }
        }
        else
        {
            CommandResponse.ResponseBln = true;
            CommandResponse.ResponseText = "Simulated command send";
            Debug.Log(RequestAPI.Instance.GetAddressByMethod(Metodos.SendCommand, sitioSeleccionado.MyDataSitio.Estructura));
            Debug.Log(CommandResponse);
        }
    }

    public void SendEventFSM(string eventName)
    {
        if (controlBombasFSM != null)
            controlBombasFSM.SendEvent(eventName);
    }
}
