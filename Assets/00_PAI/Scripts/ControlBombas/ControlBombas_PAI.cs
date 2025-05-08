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
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);
    }
    
    [Button]
    public override void SendCommand()
    {
        if (ControlLogin._singletonExists)
        {
            SetCommandValues(ControlLogin.singleton.login.Credencials.usuario, sitio.dataSitio.idSitio % 100, sitio.indexBomba, action);
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
                Debug.Log(RequestAPI.Instance.GetAddressByMethod(Metodos.SendCommand, sitio.dataSitio.Estructura));
                unityWebRequest = UnityWebRequest.Post(RequestAPI.Instance.GetAddressByMethod(Metodos.SendCommand, sitio.dataSitio.Estructura),
                    JsonUtility.ToJson(commandVWC), "application/json");

                yield return unityWebRequest.SendWebRequest();

                CallBack(unityWebRequest);
            }
        }
        else
        {
            CommandResponse.ResponseBln = true;
            CommandResponse.ResponseText = "Simulated command send";
            Debug.Log(RequestAPI.Instance.GetAddressByMethod(Metodos.SendCommand, sitio.dataSitio.Estructura));
            Debug.Log(CommandResponse);
        }
    }

    public void SendEventFSM(string eventName)
    {
        if (controlBombasFSM != null)
            controlBombasFSM.SendEvent(eventName);
    }
}
