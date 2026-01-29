using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class RequestAPI : Singleton<RequestAPI>
{
    public int delay = 10;
    
    private float _ServiceVersion;

    //private string urlCompleta;
    
    Coroutine corrutinaPoleo, corrutinaInfraestructura, corrutinaSignals, corrutinaAverageBarr;
    
    [TabGroup("Comunication")] public ConnectionData MyConectionData;

    [TabGroup("Comunication")] public bool errorInfraestructuraHTML;
    [TabGroup("Comunication")] public bool errorUpdateHTML;
    [TabGroup("Comunication")] public bool errorUpdateTotalizados;
    [TabGroup("Comunication")] public bool respInfraestructura;
    [TabGroup("Comunication")] public bool respUpdateSites;

    [TabGroup("Comunication")] public int InfraestructuraCout = 0;
    [TabGroup("Comunication")] public int UpdateCout = 0;
    [TabGroup("Comunication")] public int TotalizadoCout = 0;
    
    [TabGroup("Comunication")] public bool versionInitialized = false;

    public Action TotalizadosCallback;
    //
    // [TabGroup("Events")] public UnityEvent infraestructuraActualizada;
    // [TabGroup("Events")] public UnityEvent InitializeVersionEvent;
    // [TabGroup("Events")] public UnityEvent UpdateVersionEvent;
    
    public bool isInitPoleo = false;
    public bool usePoleo = true;
    public bool useDataFile;

    [TabGroup("Data")] public EstructurasAPI.Proyectos sistema = EstructurasAPI.Proyectos.PozosAIFA;

    [TabGroup("Data")] public DataRequestAPI dataRequestAPI;
    
    [TabGroup("Data")][ShowInInspector] public float ServiceVersion
    {
        get
        {
            if (dataRequestAPI.updateUnitySites == null)
                return -1;
            return dataRequestAPI.updateUnitySites.Version;
        }
    }
    
    private void Awake()
    {
        base.Awake();
        
        MyConectionData.ReadConnectionData();
    }
    
    public void IniciarPoleo()
    {
        if (usePoleo)
            IniciarPeticionDatos();
        else if (useDataFile)
            IniciarReadDataFile();
        
        isInitPoleo = true;
    }

    public void IniciarPeticionDatos()
    {
        if (corrutinaPoleo != null) StopCoroutine(this.corrutinaPoleo);
        this.corrutinaPoleo = StartCoroutine(GetInfraestructura());
    }

    public void IniciarReadDataFile()
    {
        StartCoroutine(ReadDataFile());
    }

    private IEnumerator ReadDataFile()
    {
        yield return new WaitForSeconds(1);

        dataRequestAPI.ReadJSON_DataFile();
        
        respInfraestructura = true;
    }
    
    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus && isInitPoleo)
        {
            Debug.Log("OnApplicationFocus");
            LanzarPoleo();
        }
    }

    private void LanzarPoleo()
    {
        if (corrutinaPoleo != null) StopCoroutine(this.corrutinaPoleo);
        
        if (errorInfraestructuraHTML)
        {
            this.corrutinaPoleo = StartCoroutine(GetInfraestructura());
        }
        else
        {
            this.corrutinaPoleo = StartCoroutine(Polear());
        }
    }

    private IEnumerator GetInfraestructura()
    {
        if (corrutinaInfraestructura != null) StopCoroutine(corrutinaInfraestructura);
        
        corrutinaInfraestructura = StartCoroutine(DoRequest(GetAddressByMethod(Metodos.Infraestructura), Metodos.Infraestructura));
      
        yield return new WaitForSeconds(delay);
        LanzarPoleo();
    }

    private IEnumerator Polear()
    {
        if (corrutinaSignals != null) StopCoroutine(corrutinaSignals);
        
        corrutinaSignals = StartCoroutine(DoRequest(GetAddressByMethod(Metodos.UpdateData),Metodos.UpdateData));
        
        yield return new WaitForSeconds(delay);
        LanzarPoleo();
    }
    
    public string GetAddressByMethod(string method)
    {
        switch (method)
        {
            case Metodos.Infraestructura:
                if (sistema == EstructurasAPI.Proyectos.PozosPAI)
                {
                    if (MyConectionData.useLocalHost)
                        return $"{MyConectionData.local}/API24/VWC/APP2024/GetInfraestructuraPozosPai";
                    else
                        return $"{MyConectionData.external}/API24/VWC/APP2024/GetInfraestructuraPozosPai";
                }
                else
                {
                    if (MyConectionData.useLocalHost)
                        return $"{MyConectionData.local}/api24/VWC/app2024/getInfraestructura?idProyecto={(int)sistema}";
                    else
                        return $"{MyConectionData.external}/api24/VWC/app2024/getInfraestructura?idProyecto={(int)sistema}";
                }
                
            
            case Metodos.UpdateData:
                if (sistema == EstructurasAPI.Proyectos.PozosPAI)
                {
                    if (MyConectionData.useLocalHost)
                        return $"{MyConectionData.local}/API24/VWC/APP2024/GetUpdatePozosPai";
                    else
                        return $"{MyConectionData.external}/API24/VWC/APP2024/GetUpdatePozosPai";
                }
                else
                {
                    if (MyConectionData.useLocalHost)
                        return $"{MyConectionData.local}/api24/VWC/app2024/getUpdate?idProyecto={(int)sistema}";
                    else
                        return $"{MyConectionData.external}/api24/VWC/app2024/getUpdate?idProyecto={(int)sistema}";
                }
            
            case Metodos.UpdateHistoricos:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/api24/VWC/app2024/GetReportesByPromedio?idProyecto={(int)sistema}";
                else
                    return $"{MyConectionData.external}/api24/VWC/app2024/GetReportesByPromedio?idProyecto={(int)sistema}";
            
            case Metodos.SendCommand:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/api24/VWC/app2024/insertComando?idProyecto={(int)sistema}";
                else
                    return $"{MyConectionData.external}/api24/VWC/app2024/insertComando?idProyecto={(int)sistema}";
                break;
            
            case Metodos.AverageBarrientos:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/api24/VWC/app2024/GetAveragePozosPai";
                else
                    return $"{MyConectionData.external}/api24/VWC/app2024/GetAveragePozosPai";
                break;
        }

        return "";
    }
    
    public string GetAddressByMethod(string method, int _sistema)
    {
        switch (method)
        {
            case Metodos.UpdateHistoricos:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/api24/VWC/app2024/GetReportesByPromedio?idProyecto={_sistema}";
                else
                    return $"{MyConectionData.external}/api24/VWC/app2024/GetReportesByPromedio?idProyecto={_sistema}";
            
            case Metodos.SendCommand:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/api24/VWC/app2024/insertComando?idProyecto={_sistema}";
                else
                    return $"{MyConectionData.external}/api24/VWC/app2024/insertComando?idProyecto={_sistema}";
                break;
        }

        return "";
    }

    private IEnumerator DoRequest(string _url, string method)
    {
        string address = $"{_url}";
        
        UnityWebRequest unityWebRequest = null;
        
        switch (method)
        {
            case Metodos.Infraestructura:
            case Metodos.UpdateData:
                unityWebRequest = UnityWebRequest.Get(address);
                
                yield return unityWebRequest.SendWebRequest();
                CallBack(unityWebRequest, method);
                break;
            default:
                break;
        }
    }
    
    private IEnumerator DoRequest(string _url, string method, Action<bool, ListAverages> callback)
    {
        string address = $"{_url}";
        
        UnityWebRequest unityWebRequest = null;
        
        switch (method)
        {
            case Metodos.AverageBarrientos:
                unityWebRequest = UnityWebRequest.Get(address);
                yield return unityWebRequest.SendWebRequest();
                CallBack(unityWebRequest, method, callback);
                break;
            default:
                break;
        }
    }

    private void CallBack(UnityWebRequest unityWebRequest, string method)
    {
        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(unityWebRequest.error + "\n" + unityWebRequest.url);
            
            switch (method)
            {
                case Metodos.Infraestructura:
                    errorInfraestructuraHTML = true;
                    break;

                case Metodos.UpdateData:
                    errorUpdateHTML = true;
                    break;
                
                case Metodos.UpdateTotalizados:
                    errorUpdateTotalizados = true;
                    break;
            }
        }
        else
        {
            if (unityWebRequest.isDone)
            {
                switch (method)
                {
                    case Metodos.Infraestructura:
                        errorInfraestructuraHTML = false;
                        respInfraestructura = true;
                        dataRequestAPI.infraestructura = JsonUtility.FromJson<Infraestructura>(unityWebRequest.downloadHandler.text);
                        InfraestructuraCout++;
                        break;
                    case Metodos.UpdateData:
                        errorUpdateHTML = false;
                        respUpdateSites = true;
                        dataRequestAPI.updateUnitySites = JsonUtility.FromJson<UpdateUnitySites>(unityWebRequest.downloadHandler.text);
                        UpdateCout++;
                        if (!versionInitialized)
                        {
                            _ServiceVersion = ServiceVersion;
                            versionInitialized = true;
                            
                            if (ControlRequest._singletonExists)
                                ControlRequest.singleton.InitializeVersionEvent.Invoke();
                        }
                        else if(_ServiceVersion != ServiceVersion)
                        {
                            if (ControlRequest._singletonExists)
                                ControlRequest.singleton.UpdateVersionEvent.Invoke();
                        }
                        break;
                    case Metodos.UpdateTotalizados:
                        errorUpdateTotalizados = false;
                        dataRequestAPI.totalizadosPorFecha = JsonUtility.FromJson<RespuestaTotalizadosPorFecha>(unityWebRequest.downloadHandler.text);
                        TotalizadosCallback();
                        TotalizadosCallback = null;
                        TotalizadoCout++;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    private void CallBack(UnityWebRequest unityWebRequest, string method, Action<bool, ListAverages> callback)
    {
        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(unityWebRequest.error + "\n" + unityWebRequest.url);
            callback(true, null);
        }
        else
        {
            if (unityWebRequest.isDone)
            {
                switch (method)
                {
                    case Metodos.AverageBarrientos:
                        ListAverages responce = JsonUtility.FromJson<ListAverages>(unityWebRequest.downloadHandler.text);
                        callback(false, responce);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    public string GetAdressServ()
    {
        if (MyConectionData.useLocalHost)
            return MyConectionData.local;
        else
            return MyConectionData.external;
    }
    
    public void GetAverageBarrientos(Action<bool, ListAverages> callback)
    {
        if (corrutinaAverageBarr != null) StopCoroutine(corrutinaSignals);
            
        corrutinaAverageBarr = StartCoroutine(DoRequest(GetAddressByMethod(Metodos.AverageBarrientos), Metodos.AverageBarrientos, callback));
    }
}
