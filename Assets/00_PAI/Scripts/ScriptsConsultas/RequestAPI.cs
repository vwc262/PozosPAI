using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class RequestAPI : MonoBehaviour
{
    #region Singleton
    private static RequestAPI _instance;
    public static RequestAPI Instance { get { return _instance; } }
    #endregion

    public int delay = 10;
    
    private float _ServiceVersion;

    private string url, urlCompleta;
    
    Coroutine corrutinaPoleo, corrutinaInfraestructura, corrutinaSignals;
    
    [TabGroup("Comunication")] public ConnectionData MyConectionData;

    [TabGroup("Comunication")] public bool errorInfraestructuraHTML;
    [TabGroup("Comunication")] public bool errorUpdateHTML;
    [TabGroup("Comunication")] public bool errorUpdateTotalizados;
    [TabGroup("Comunication")] public bool errorUpdateHistoricos;
    [TabGroup("Comunication")] public bool respInfraestructura;
    [TabGroup("Comunication")] public bool respUpdateSites;

    [TabGroup("Comunication")] public int InfraestructuraCout = 0;
    [TabGroup("Comunication")] public int UpdateCout = 0;
    [TabGroup("Comunication")] public int TotalizadoCout = 0;
    [TabGroup("Comunication")] public int HistoricoCout = 0;
    [TabGroup("Comunication")] public bool versionInitialized = false;

    [TabGroup("Historicos")] private int idSitio;
    [TabGroup("Historicos")] private DateTime totalizadosTime1 = DateTime.Now.Subtract(TimeSpan.FromDays(2));
    [TabGroup("Historicos")] private DateTime totalizadosTime2 = DateTime.Now;
    [TabGroup("Historicos")] public RequestBoy.TipoPromedio tipoPromedio;

    public Action TotalizadosCallback;
    
    public Action HistoricoCallback;
    
    [TabGroup("Events")] public UnityEvent infraestructuraActualizada;
    [TabGroup("Events")] public UnityEvent InitializeVersionEvent;
    [TabGroup("Events")] public UnityEvent UpdateVersionEvent;
    
    public bool usePoleo = true;
    public bool useDataFile;
    public bool enableHistoricos = true;
    public bool simulaCallBackHistoricos;
    
    public enum Proyectos
    {
        Default,                        //00 - Default: 'Default',
        GustavoAMadero,                 //01 - GustavoAMadero: 'Gustavo A Madero',
        Padierna,                       //02 - Padierna: 'Padierna',
        PozosSistemaLerma,              //03 - PozosSistemaLerma: 'Pozos Sistema Lerma',
        Iztapalapa,                     //04 - Iztapalapa: 'Iztapalapa',
        Chalmita,                       //05 - Chalmita: 'Chalmita',
        Yaqui,                          //06 - Yaqui: 'Yaqui',
        SistemaCutzamala,               //07 - SistemaCutzamala: 'SistemaCutzamala',
        PruebasCampo,                   //08 - PruebasCampo: 'Puebas Campo',
        SantaCatarina,                  //09 - SantaCatarina: 'Santa Catarina',
        Chiconautla,                    //10 - Chiconautla: 'Chiconautla',
        Sorpasso,                       //11 - Sorpasso: 'Sorpasso',
        EscudoNacional,                 //12 - EscudoNacional: 'Escudo Nacional',
        ClimatologicasHidrometricas,    //13 - ClimatologicasHidrometricas: 'Estaciones Climatologicas e Hidrometricas',
        Teoloyucan,                     //14 - Teoloyucan: 'Teoloyucan',
        Pruebas,                        //15 - Pruebas: 'Pruebas',
        LineaMorada,                    //16 - LineaMorada: 'Línea Morada',
        PozosAIFA,                      //17 - PozosAIFA: 'Pozos AIFA',
        PozosZumpango,                  //18 - PozosZumpango: 'Pozos Zumpango',
        PaseoDelRio,                    //19 - PaseoDelRio: 'Paseo del Rio',
        Aduana,                         //20 - Aduana: 'Aduana',
        PozosPAI,                       //21 - PozosPAI: 'Pozos PAI',
        PozosCoyoacan,                  //22 - PozosCoyoacan: 'Pozos Coyoacan',
        PozosAzcapotzalco,              //23 - PozosAzcapotzalco: 'Pozos Azcapotzalco',
        Encharcamientos,                //24 - Encharcamientos: 'Encharcamientos',
        Lumbreras,                      //25 - Lumbreras: 'Lumbreras',
        Ramales                         //26 - Ramales: 'Ramales'
    }

    [TabGroup("Data")] public Proyectos sistema = Proyectos.PozosAIFA;

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
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        MyConectionData.ReadConnectionData();
    }

    // private void Start()
    // {
    //     IniciarPoleo()
    // }
    
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

        // switch (sistema)
        // {
        //     case Proyectos.Lerma:
        //         url = GetLocalIPAddress(Metodos.Lerma_Infraestructura);
        //         corrutinaInfraestructura = StartCoroutine(DoRequest(Metodos.Lerma_Infraestructura));
        //         break;
        //     case Proyectos.Chiconautla:
        //         url = GetLocalIPAddress(Metodos.Chiconautla_Infraestructura);
        //         corrutinaInfraestructura = StartCoroutine(DoRequest(Metodos.Chiconautla_Infraestructura));
        //         break;
        // }
      
        yield return new WaitForSeconds(delay);
        LanzarPoleo();
    }

    private IEnumerator Polear()
    {
        if (corrutinaSignals != null) StopCoroutine(corrutinaSignals);
        
        url = GetAddressByMethod(Metodos.UpdateData);
        corrutinaSignals = StartCoroutine(DoRequest(Metodos.UpdateData));
        
        // switch (sistema)
        // {
        //     case Proyectos.Lerma:
        //         url = GetLocalIPAddress(Metodos.Lerma_UpdateData);
        //         corrutinaSignals = StartCoroutine(DoRequest(Metodos.Lerma_UpdateData));
        //         break;
        //     case Proyectos.Chiconautla:
        //         url = GetLocalIPAddress(Metodos.Chiconautla_UpdateData);
        //         corrutinaSignals = StartCoroutine(DoRequest(Metodos.Chiconautla_UpdateData));
        //         break;
        // }
        
        yield return new WaitForSeconds(delay);
        LanzarPoleo();
    }
    
    public void GetTotalizadosByDates(DateTime time1, DateTime time2, Action Callback)
    {
        totalizadosTime1 = time1;
        totalizadosTime2 = time2;
        GetTotalizados();

        TotalizadosCallback = Callback;
    }

    private void GetTotalizados()
    {
        url = GetAddressByMethod(Metodos.UpdateTotalizados);
        StartCoroutine(DoRequest(Metodos.UpdateTotalizados));
    }
    
    public void GetHistricosByDates(int _idSitio, DateTime _time1, DateTime _time2, RequestBoy.TipoPromedio _tipoPromedio, Action Callback)
    {
        idSitio = _idSitio;
        totalizadosTime1 = _time1;
        totalizadosTime2 = _time2;
        tipoPromedio = _tipoPromedio;
        HistoricoCallback = Callback;
        
        GetHistoricos();
    }
    
    public void GetHistricosByDates(int _idSitio, int _sistema, DateTime _time1, DateTime _time2, RequestBoy.TipoPromedio _tipoPromedio, Action Callback)
    {
        idSitio = _idSitio;
        totalizadosTime1 = _time1;
        totalizadosTime2 = _time2;
        tipoPromedio = _tipoPromedio;
        HistoricoCallback = Callback;
        
        GetHistoricosBySistema(_sistema);
    }
    
    private void GetHistoricos()
    {
        url = GetAddressByMethod(Metodos.UpdateHistoricos);
        StartCoroutine(DoRequest(Metodos.UpdateHistoricos));
    }
    
    private void GetHistoricosBySistema(int _sistema)
    {
        url = GetAddressByMethod(Metodos.UpdateHistoricos, _sistema);
        //Debug.Log(url);
        StartCoroutine(DoRequestHistoricosBySistema(Metodos.UpdateHistoricos));
    }
    
    public string GetAddressByMethod(string method)
    {
        switch (method)
        {
            case Metodos.Infraestructura:
                if (sistema == Proyectos.PozosPAI)
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
                if (sistema == Proyectos.PozosPAI)
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
            case Metodos.UpdateTotalizados:
                WWWForm formData = new WWWForm();
                formData.AddField("fechaInicial", totalizadosTime1.ToString());
                formData.AddField("fechaFinal", totalizadosTime2.ToString());
                unityWebRequest = UnityWebRequest.Post(address, formData);
                
                yield return unityWebRequest.SendWebRequest();
                CallBack(unityWebRequest, method);
                break;
            case Metodos.UpdateHistoricos:
            {
                if (enableHistoricos)
                {
                    SiteDescription sitio =
                        RequestAPI._instance.dataRequestAPI.infraestructura.Sites.Find(item => item.Id == idSitio);
                    List<SignalsDescriptionContainerC> signalsDescriptionContainer = sitio.SignalsDescriptionContainer;

                    dataRequestAPI.historicosBySitio.Gasto.Clear();
                    dataRequestAPI.historicosBySitio.Presion.Clear();
                    dataRequestAPI.historicosBySitio.Totalizado.Clear();
                    dataRequestAPI.historicosBySitio.Bomba.Clear();
                    dataRequestAPI.historicosBySitio.Nivel.Clear();

                    SignalsDescriptionContainerC signalDescriptionC_G =
                        signalsDescriptionContainer.Find(
                            item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.GASTO);
                    SignalsDescriptionContainerC signalDescriptionC_P =
                        signalsDescriptionContainer.Find(item =>
                            item.TipoSignal == (int)SignalBase.TipoSignalEnum.PRESION);
                    SignalsDescriptionContainerC signalDescriptionC_T =
                        signalsDescriptionContainer.Find(item =>
                            item.TipoSignal == (int)SignalBase.TipoSignalEnum.TOTALIZADO);
                    SignalsDescriptionContainerC signalDescriptionC_B =
                        signalsDescriptionContainer.Find(
                            item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.BOMBA);
                    SignalsDescriptionContainerC signalDescriptionC_N =
                        signalsDescriptionContainer.Find(
                            item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.NIVEL);

                    if (signalDescriptionC_G != null)
                    {
                        RequestBoy DataH_G = new RequestBoy();
                        DataH_G.IdSignal = signalDescriptionC_G.SignalsDescription.First().IdSignal;
                        DataH_G.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                        DataH_G.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                        unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                            JsonUtility.ToJson(DataH_G), "application/json");

                        yield return unityWebRequest.SendWebRequest();
                        SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.GASTO);
                    }

                    if (signalDescriptionC_P != null)
                    {
                        RequestBoy DataH_P = new RequestBoy();
                        DataH_P.IdSignal = signalDescriptionC_P.SignalsDescription.First().IdSignal;
                        DataH_P.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                        DataH_P.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                        unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                            JsonUtility.ToJson(DataH_P), "application/json");

                        yield return unityWebRequest.SendWebRequest();
                        SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.PRESION);
                    }

                    if (signalDescriptionC_T != null)
                    {
                        RequestBoy DataH_T = new RequestBoy();
                        DataH_T.IdSignal = signalDescriptionC_T.SignalsDescription.First().IdSignal;
                        DataH_T.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                        DataH_T.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                        unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                            JsonUtility.ToJson(DataH_T), "application/json");

                        yield return unityWebRequest.SendWebRequest();
                        SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.TOTALIZADO);
                    }

                    if (signalDescriptionC_B != null)
                    {
                        RequestBoy DataH_B = new RequestBoy();
                        DataH_B.IdSignal = signalDescriptionC_B.SignalsDescription.First().IdSignal;
                        DataH_B.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                        DataH_B.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                        unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                            JsonUtility.ToJson(DataH_B), "application/json");

                        yield return unityWebRequest.SendWebRequest();
                        SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.BOMBA);
                    }

                    if (signalDescriptionC_N != null)
                    {
                        RequestBoy DataH_N = new RequestBoy();
                        DataH_N.IdSignal = signalDescriptionC_N.SignalsDescription.First().IdSignal;
                        DataH_N.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                        DataH_N.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                        unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                            JsonUtility.ToJson(DataH_N), "application/json");

                        yield return unityWebRequest.SendWebRequest();
                        SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.NIVEL);
                    }

                    //Llamada a funcion de callback
                    CallBackHistoricos();
                }
                else if (simulaCallBackHistoricos)
                {
                    yield return new WaitForSeconds(2);
                    CallBackHistoricos();
                }
            }
                break;
            default:
                break;
        }
    }
    
    private IEnumerator DoRequestHistoricosBySistema(string method)
    {
        string address = $"{url}";
        
        UnityWebRequest unityWebRequest = null;
        
        if (enableHistoricos)
        {
            SiteDescription sitio =
                RequestAPI._instance.dataRequestAPI.infraestructura.Sites.Find(item => item.Id % 100 == idSitio);
            List<SignalsDescriptionContainerC> signalsDescriptionContainer = sitio.SignalsDescriptionContainer;

            dataRequestAPI.historicosBySitio.Gasto.Clear();
            dataRequestAPI.historicosBySitio.Presion.Clear();
            dataRequestAPI.historicosBySitio.Totalizado.Clear();
            dataRequestAPI.historicosBySitio.Bomba.Clear();
            dataRequestAPI.historicosBySitio.Nivel.Clear();

            SignalsDescriptionContainerC signalDescriptionC_G =
                signalsDescriptionContainer.Find(
                    item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.GASTO);
            SignalsDescriptionContainerC signalDescriptionC_P =
                signalsDescriptionContainer.Find(item =>
                    item.TipoSignal == (int)SignalBase.TipoSignalEnum.PRESION);
            SignalsDescriptionContainerC signalDescriptionC_T =
                signalsDescriptionContainer.Find(item =>
                    item.TipoSignal == (int)SignalBase.TipoSignalEnum.TOTALIZADO);
            SignalsDescriptionContainerC signalDescriptionC_B =
                signalsDescriptionContainer.Find(
                    item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.BOMBA);
            SignalsDescriptionContainerC signalDescriptionC_N =
                signalsDescriptionContainer.Find(
                    item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.NIVEL);

            if (signalDescriptionC_G != null)
            {
                RequestBoy DataH_G = new RequestBoy();
                DataH_G.IdSignal = signalDescriptionC_G.SignalsDescription.First().IdSignal;
                DataH_G.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                DataH_G.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                    JsonUtility.ToJson(DataH_G), "application/json");

                yield return unityWebRequest.SendWebRequest();
                SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.GASTO);
            }

            if (signalDescriptionC_P != null)
            {
                RequestBoy DataH_P = new RequestBoy();
                DataH_P.IdSignal = signalDescriptionC_P.SignalsDescription.First().IdSignal;
                DataH_P.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                DataH_P.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                    JsonUtility.ToJson(DataH_P), "application/json");

                yield return unityWebRequest.SendWebRequest();
                SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.PRESION);
            }

            if (signalDescriptionC_T != null)
            {
                RequestBoy DataH_T = new RequestBoy();
                DataH_T.IdSignal = signalDescriptionC_T.SignalsDescription.First().IdSignal;
                DataH_T.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                DataH_T.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                    JsonUtility.ToJson(DataH_T), "application/json");

                yield return unityWebRequest.SendWebRequest();
                SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.TOTALIZADO);
            }

            if (signalDescriptionC_B != null)
            {
                RequestBoy DataH_B = new RequestBoy();
                DataH_B.IdSignal = signalDescriptionC_B.SignalsDescription.First().IdSignal;
                DataH_B.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                DataH_B.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                    JsonUtility.ToJson(DataH_B), "application/json");

                yield return unityWebRequest.SendWebRequest();
                SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.BOMBA);
            }

            if (signalDescriptionC_N != null)
            {
                RequestBoy DataH_N = new RequestBoy();
                DataH_N.IdSignal = signalDescriptionC_N.SignalsDescription.First().IdSignal;
                DataH_N.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                DataH_N.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                    JsonUtility.ToJson(DataH_N), "application/json");

                yield return unityWebRequest.SendWebRequest();
                SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.NIVEL);
            }

            //Llamada a funcion de callback
            CallBackHistoricos();
        }
        else if (simulaCallBackHistoricos)
        {
            yield return new WaitForSeconds(2);
            CallBackHistoricos();
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
                
                case Metodos.UpdateHistoricos:
                    errorUpdateHistoricos = true;
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

    private void SetDataHistoricos(UnityWebRequest unityWebRequest, string method, SignalBase.TipoSignalEnum TipoSignal)
    {
        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError ||
            unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(unityWebRequest.error + "\n" + unityWebRequest.url);

            switch (method)
            {
                default:
                    break;

                case Metodos.UpdateHistoricos:
                    errorUpdateHistoricos = true;
                    break;
            }
        }
        else
        {
            if (unityWebRequest.isDone)
            {
                Reportes reportes;
                    
                switch (method)
                {
                    case Metodos.UpdateHistoricos:
                        switch (TipoSignal)
                        {
                            case SignalBase.TipoSignalEnum.GASTO:
                                //Debug.Log("Data: " + unityWebRequest.downloadHandler.text);
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                dataRequestAPI.historicosBySitio.Gasto = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.PRESION:
                                //Debug.Log("Data: " + unityWebRequest.downloadHandler.text);
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                dataRequestAPI.historicosBySitio.Presion = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.TOTALIZADO:
                                //Debug.Log("Data: " + unityWebRequest.downloadHandler.text);
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                dataRequestAPI.historicosBySitio.Totalizado = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.BOMBA:
                                //Debug.Log("Data: " + unityWebRequest.downloadHandler.text);
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                dataRequestAPI.historicosBySitio.Bomba = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.NIVEL:
                                //Debug.Log("Data: " + unityWebRequest.downloadHandler.text);
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                dataRequestAPI.historicosBySitio.Nivel = reportes.Reporte;
                                HistoricoCout++;
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    private void CallBackHistoricos()
    {
        if (HistoricoCallback != null)
            HistoricoCallback();
        HistoricoCallback = null;
    }

    public string GetAdressServ()
    {
        if (MyConectionData.useLocalHost)
            return MyConectionData.local;
        else
            return MyConectionData.external;
    }
    
    public string GetFechaFormatConsulta(DateTime _date)
    {
        return _date.Year + "-" + _date.Month.ToString("00")+ "-" + _date.Day.ToString("00") + " " +
               _date.Hour.ToString("00") + ":" + _date.Minute.ToString("00") + ":" + _date.Second.ToString("00");
    }
}
