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

    private string url, urlCompleta;
    
    Coroutine corrutinaPoleo, corrutinaInfraestructura, corrutinaSignals;
    
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
    
    [TabGroup("Events")] public UnityEvent infraestructuraActualizada;
    [TabGroup("Events")] public UnityEvent InitializeVersionEvent;
    [TabGroup("Events")] public UnityEvent UpdateVersionEvent;
    
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
        
        infraestructuraActualizada.Invoke();
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

        url = GetAddressByMethod(Metodos.Infraestructura);
        corrutinaInfraestructura = StartCoroutine(DoRequest(Metodos.Infraestructura));
      
        yield return new WaitForSeconds(delay);
        LanzarPoleo();
    }

    private IEnumerator Polear()
    {
        if (corrutinaSignals != null) StopCoroutine(corrutinaSignals);
        
        url = GetAddressByMethod(Metodos.UpdateData);
        corrutinaSignals = StartCoroutine(DoRequest(Metodos.UpdateData));
        
        yield return new WaitForSeconds(delay);
        LanzarPoleo();
    }
    
    // public void GetTotalizadosByDates(DateTime time1, DateTime time2, Action Callback)
    // {
    //     totalizadosTime1 = time1;
    //     totalizadosTime2 = time2;
    //     GetTotalizados();
    //
    //     TotalizadosCallback = Callback;
    // }

    // private void GetTotalizados()
    // {
    //     url = GetAddressByMethod(Metodos.UpdateTotalizados);
    //     StartCoroutine(DoRequest(Metodos.UpdateTotalizados));
    // }
    
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

    private IEnumerator DoRequest(string method)
    {
        string address = $"{url}";
        
        UnityWebRequest unityWebRequest = null;
        
        switch (method)
        {
            case Metodos.Infraestructura:
            case Metodos.UpdateData:
                unityWebRequest = UnityWebRequest.Get(address);
                
                yield return unityWebRequest.SendWebRequest();
                CallBack(unityWebRequest, method);
                break;
            // case Metodos.UpdateTotalizados:
            //     WWWForm formData = new WWWForm();
            //     formData.AddField("fechaInicial", totalizadosTime1.ToString());
            //     formData.AddField("fechaFinal", totalizadosTime2.ToString());
            //     unityWebRequest = UnityWebRequest.Post(address, formData);
            //     
            //     yield return unityWebRequest.SendWebRequest();
            //     CallBack(unityWebRequest, method);
            //     break;
            // case Metodos.UpdateHistoricos:
            // {
            //     if (enableHistoricos)
            //     {
            //         SiteDescription sitio =
            //             RequestAPI.singleton.dataRequestAPI.infraestructura.Sites.Find(item => item.Id == idSitio);
            //         List<SignalsDescriptionContainerC> signalsDescriptionContainer = sitio.SignalsDescriptionContainer;
            //
            //         dataRequestAPI.historicosBySitio.Gasto.Clear();
            //         dataRequestAPI.historicosBySitio.Presion.Clear();
            //         dataRequestAPI.historicosBySitio.Totalizado.Clear();
            //         dataRequestAPI.historicosBySitio.Bomba.Clear();
            //         dataRequestAPI.historicosBySitio.Nivel.Clear();
            //
            //         SignalsDescriptionContainerC signalDescriptionC_G =
            //             signalsDescriptionContainer.Find(
            //                 item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.GASTO);
            //         SignalsDescriptionContainerC signalDescriptionC_P =
            //             signalsDescriptionContainer.Find(item =>
            //                 item.TipoSignal == (int)SignalBase.TipoSignalEnum.PRESION);
            //         SignalsDescriptionContainerC signalDescriptionC_T =
            //             signalsDescriptionContainer.Find(item =>
            //                 item.TipoSignal == (int)SignalBase.TipoSignalEnum.TOTALIZADO);
            //         SignalsDescriptionContainerC signalDescriptionC_B =
            //             signalsDescriptionContainer.Find(
            //                 item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.BOMBA);
            //         SignalsDescriptionContainerC signalDescriptionC_N =
            //             signalsDescriptionContainer.Find(
            //                 item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.NIVEL);
            //
            //         if (signalDescriptionC_G != null)
            //         {
            //             RequestBoy DataH_G = new RequestBoy();
            //             DataH_G.IdSignal = signalDescriptionC_G.SignalsDescription.First().IdSignal;
            //             DataH_G.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
            //             DataH_G.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);
            //
            //             unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
            //                 JsonUtility.ToJson(DataH_G), "application/json");
            //
            //             yield return unityWebRequest.SendWebRequest();
            //             SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.GASTO);
            //         }
            //
            //         if (signalDescriptionC_P != null)
            //         {
            //             RequestBoy DataH_P = new RequestBoy();
            //             DataH_P.IdSignal = signalDescriptionC_P.SignalsDescription.First().IdSignal;
            //             DataH_P.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
            //             DataH_P.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);
            //
            //             unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
            //                 JsonUtility.ToJson(DataH_P), "application/json");
            //
            //             yield return unityWebRequest.SendWebRequest();
            //             SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.PRESION);
            //         }
            //
            //         if (signalDescriptionC_T != null)
            //         {
            //             RequestBoy DataH_T = new RequestBoy();
            //             DataH_T.IdSignal = signalDescriptionC_T.SignalsDescription.First().IdSignal;
            //             DataH_T.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
            //             DataH_T.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);
            //
            //             unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
            //                 JsonUtility.ToJson(DataH_T), "application/json");
            //
            //             yield return unityWebRequest.SendWebRequest();
            //             SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.TOTALIZADO);
            //         }
            //
            //         if (signalDescriptionC_B != null)
            //         {
            //             RequestBoy DataH_B = new RequestBoy();
            //             DataH_B.IdSignal = signalDescriptionC_B.SignalsDescription.First().IdSignal;
            //             DataH_B.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
            //             DataH_B.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);
            //
            //             unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
            //                 JsonUtility.ToJson(DataH_B), "application/json");
            //
            //             yield return unityWebRequest.SendWebRequest();
            //             SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.BOMBA);
            //         }
            //
            //         if (signalDescriptionC_N != null)
            //         {
            //             RequestBoy DataH_N = new RequestBoy();
            //             DataH_N.IdSignal = signalDescriptionC_N.SignalsDescription.First().IdSignal;
            //             DataH_N.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
            //             DataH_N.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);
            //
            //             unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
            //                 JsonUtility.ToJson(DataH_N), "application/json");
            //
            //             yield return unityWebRequest.SendWebRequest();
            //             SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.NIVEL);
            //         }
            //
            //         //Llamada a funcion de callback
            //         CallBackHistoricos();
            //     }
            //     else if (simulaCallBackHistoricos)
            //     {
            //         yield return new WaitForSeconds(2);
            //         CallBackHistoricos();
            //     }
            // }
            // break;
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
                
                // case Metodos.UpdateHistoricos:
                //     errorUpdateHistoricos = true;
                //     break;
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
                        infraestructuraActualizada.Invoke();
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
                            InitializeVersionEvent.Invoke();
                        }
                        else if(_ServiceVersion != ServiceVersion)
                        {
                            UpdateVersionEvent.Invoke();
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
    
    public string GetAdressServ()
    {
        if (MyConectionData.useLocalHost)
            return MyConectionData.local;
        else
            return MyConectionData.external;
    }
}
