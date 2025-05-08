using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class ControlBombas : Singleton<ControlBombas>
{
    public CommandVWC commandVWC;
    public CommandResponse CommandResponse;

    //public string usuario = "VWC";
    public BombaAction action;
    
    //public string address = $"http://w1.doomdns.com:11002/API/VWC/PozosSistemaLermaUnreal/SendCommand";
    // public string address = $"/api24/VWC/Unreal/Chiconautla2024/InsertComando";
    public bool errorControlBombaHTML;
    
    public ControlSitio sitio;

    public bool simulaSendCommand;
    
    public enum BombaAction
    {
        Arrancar,
        Parar
    }
    
    private void Start()
    {
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);
    }

    public void UpdateInfoSitio(ControlSitio sitio)
    {
        this.sitio = sitio;
    }
    
    [Button]
    public void SetCommandValues(string _usuario, int _idSitio, int numeroBomba, BombaAction _action)
    {
        commandVWC.Usuario = _usuario;
        commandVWC.IdEstacion = _idSitio;
        commandVWC.Codigo = GetCodigo(commandVWC.IdEstacion, numeroBomba, _action);
        commandVWC.RegModbus = 2020;
    }

    public int GetCodigo(int idEstacion, int numeroBomba, BombaAction _action)
    {
        int codigo = 0;
        
        switch (_action)
        {
            case BombaAction.Arrancar:
                codigo = idEstacion << 8 | numeroBomba << 4 | 1;
                //codigo = numeroBomba << 4 | 1;
                break;
            case BombaAction.Parar:
                codigo = idEstacion << 8 | numeroBomba << 4 | 2;
                //codigo = numeroBomba << 4 | 2;
                break;
        }

        return codigo;
    }

    [Button]
    public virtual void SendCommand()
    {
        if (ControlLogin._singletonExists)
        {
            SetCommandValues(ControlLogin.singleton.login.Credencials.usuario, sitio.dataSitio.idSitio, sitio.indexBomba, action);
            StartCoroutine(DoRequest());
        }
    }
    
    // [Button]
    // public void SendCommand(string _usuario, BombaAction _action)
    // {
    //     usuario = _usuario;
    //     action = _action;
    //     SendCommand();
    // }

    public void SendCommand(BombaAction _action)
    {
        action = _action;
        SendCommand();
    }

    public void SendCommand(int _idSitio, int _indexBomba, BombaAction _action)
    {
        if (ControlLogin._singletonExists)
        {
            SetCommandValues(ControlLogin.singleton.login.Credencials.usuario, _idSitio, _indexBomba, _action);
            StartCoroutine(DoRequest());
        }
    }
    
    public virtual IEnumerator DoRequest()
    {
        if (!simulaSendCommand)
        {
            if (RequestAPI.Instance != null)
            {
                UnityWebRequest unityWebRequest = null;

                unityWebRequest = UnityWebRequest.Post(RequestAPI.Instance.GetAddressByMethod(Metodos.SendCommand),
                    JsonUtility.ToJson(commandVWC), "application/json");

                yield return unityWebRequest.SendWebRequest();

                CallBack(unityWebRequest);
            }
        }
        else
        {
            CommandResponse.ResponseBln = true;
            CommandResponse.ResponseText = "Simulated command send";
            Debug.Log(CommandResponse);
        }
    }
    
    public void CallBack(UnityWebRequest unityWebRequest)
    {
        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(unityWebRequest.error);

            errorControlBombaHTML = true;
        }
        else
        {
            if (unityWebRequest.isDone)
            {
                CommandResponse = JsonUtility.FromJson<CommandResponse>(unityWebRequest.downloadHandler.text);
            }
        }
    }
    
    public void ActivateLoginUI()
    {
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.ActivateLoginPanel();
    }
}