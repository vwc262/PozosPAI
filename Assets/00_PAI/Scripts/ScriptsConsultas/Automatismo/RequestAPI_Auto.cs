using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class RequestAPI_Auto : Singleton<RequestAPI_Auto>
{
    public int delay = 10;
    
    private float _ServiceVersion;
    
    private string url, urlCompleta;

    private Coroutine corrutinaPoleo;
    private Coroutine corrutinaDatosSegmentos;
    private Coroutine corrutinaDatosSegmentosConf;
    
    [TabGroup("Comunication")] public ConnectionData MyConectionData;
    
    [TabGroup("Comunication")] public bool errorAutomatismoSegementosHTML;
    [TabGroup("Comunication")] public bool errorConfAutomatismoSegementosHTML;
    [TabGroup("Comunication")] public bool errorAutomatismoEstacionesHTML;
    [TabGroup("Comunication")] public bool errorConfAutomatismoEstacionesHTML;
    [TabGroup("Comunication")] public bool respAutomatismoSegementosHTML;
    [TabGroup("Comunication")] public bool respConfAutomatismoSegementosHTML;
    [TabGroup("Comunication")] public bool respAutomatismoEstacionesHTML;
    [TabGroup("Comunication")] public bool respConfAutomatismoEstacionesHTML;
    [TabGroup("Comunication")] public bool errorSendCommandSegmentoHTML;
    [TabGroup("Comunication")] public int AutomatismoSegementosCout = 0;
    [TabGroup("Comunication")] public int ConfAutomatismoSegementosCout = 0;
    [TabGroup("Comunication")] public int AutomatismoEstacionesCout = 0;
    [TabGroup("Comunication")] public int ConfAutomatismoEstacionesCout = 0;
    
    [TabGroup("Datos")] public SegmentosAutomatismo segmentosAutomatismo;
    [TabGroup("Datos")] public SegmentosAutomatismo ConfSegmentosAutomatismo;
    [TabGroup("Datos")] public List<EstacionesSistema> estacionesSistemaAutomatismo;
    [TabGroup("Datos")] public List<EstacionesSistema> ConfEstacionesSistemaAutomatismo;

    public UnityEvent datosAutomatismoActualizados;
    
    protected override void Awake()
    {
        base.Awake();
        
        MyConectionData.ReadConnectionData();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LanzarPoleo();
    }
    
    public void LanzarPoleo()
    {
        if (corrutinaPoleo != null) StopCoroutine(this.corrutinaPoleo);
        
        corrutinaPoleo = StartCoroutine(Polear());
    }

    public void ForzarActualizacionDatos(int sistema)
    {
        if (corrutinaPoleo != null) StopCoroutine(this.corrutinaPoleo);
        if (corrutinaDatosSegmentos != null) StopCoroutine(corrutinaDatosSegmentos);
        foreach (var estacion in estacionesSistemaAutomatismo)
        {
            if (estacion.corrutinaEstaciones != null) StopCoroutine(estacion.corrutinaEstaciones);
        }

        corrutinaPoleo = StartCoroutine(ActualizarDatosCoroutine(sistema));
    }
    
    private IEnumerator Polear()
    {
        if (corrutinaDatosSegmentos != null) StopCoroutine(corrutinaDatosSegmentos);
        
        //Polear datos en UTR
        url = GetAddressByMethod(Metodos.AutomatismoSegmentos);
        corrutinaDatosSegmentos = StartCoroutine(DoRequest(Metodos.AutomatismoSegmentos, url));
        
        if (corrutinaDatosSegmentosConf != null) StopCoroutine(corrutinaDatosSegmentosConf);
        
        //Polear datos en BD
        url = GetAddressByMethod(Metodos.ConfiguracionAutomatismoSegmentos);
        corrutinaDatosSegmentosConf = StartCoroutine(DoRequest(Metodos.ConfiguracionAutomatismoSegmentos, url));

        foreach (var estacion in estacionesSistemaAutomatismo)
        {
            if (estacion.corrutinaEstaciones != null) StopCoroutine(estacion.corrutinaEstaciones);
            
            url = GetAddressByMethod(Metodos.AutomatismoEstaciones, (int)estacion.sistema);
            estacion.corrutinaEstaciones = StartCoroutine(DoRequest(Metodos.AutomatismoEstaciones, (int)estacion.sistema, url));
        }
        
        foreach (var estacion in ConfEstacionesSistemaAutomatismo)
        {
            if (estacion.corrutinaEstacionesConf != null) StopCoroutine(estacion.corrutinaEstacionesConf);
            
            url = GetAddressByMethod(Metodos.ConfiguracionAutomatismoEstaciones, (int)estacion.sistema);
            estacion.corrutinaEstacionesConf = StartCoroutine(DoRequest(Metodos.ConfiguracionAutomatismoEstaciones, (int)estacion.sistema, url));
        }
        
        yield return new WaitForSeconds(delay);
        LanzarPoleo();
    }
    
    private IEnumerator ActualizarDatosCoroutine(int sistema)
    {
        yield return new WaitForSeconds(0.5f);
        
        if (corrutinaDatosSegmentos != null) StopCoroutine(corrutinaDatosSegmentos);
        
        //Polear datos en UTR
        url = GetAddressByMethod(Metodos.AutomatismoSegmentos);
        corrutinaDatosSegmentos = StartCoroutine(DoRequest(Metodos.AutomatismoSegmentos, url));

        if (corrutinaDatosSegmentosConf != null) StopCoroutine(corrutinaDatosSegmentosConf);
        
        //Polear datos en BD
        url = GetAddressByMethod(Metodos.ConfiguracionAutomatismoSegmentos);
        corrutinaDatosSegmentosConf = StartCoroutine(DoRequest(Metodos.ConfiguracionAutomatismoSegmentos, url));
   
        EstacionesSistema estacion = estacionesSistemaAutomatismo.Find(
            x => x.sistema == (RequestAPI.Proyectos)sistema);
        
        if (estacion.corrutinaEstaciones != null) StopCoroutine(estacion.corrutinaEstaciones);
            
        url = GetAddressByMethod(Metodos.AutomatismoEstaciones, (int)estacion.sistema);
        estacion.corrutinaEstaciones = StartCoroutine(DoRequestActualizacion(Metodos.AutomatismoEstaciones, (int)estacion.sistema, url));
        
        EstacionesSistema estacionConf = ConfEstacionesSistemaAutomatismo.Find(
            x => x.sistema == (RequestAPI.Proyectos)sistema);
        
        if (estacionConf.corrutinaEstaciones != null) StopCoroutine(estacionConf.corrutinaEstaciones);
            
        url = GetAddressByMethod(Metodos.AutomatismoEstaciones, (int)estacionConf.sistema);
        estacionConf.corrutinaEstaciones = StartCoroutine(DoRequestActualizacion(Metodos.AutomatismoEstaciones, (int)estacionConf.sistema, url));
        
        yield return new WaitForSeconds(delay);
        LanzarPoleo();
    }
    
    public string GetAddressByMethod(string method)
    {
        switch (method)
        {
            case Metodos.AutomatismoSegmentos:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/API24/VWC/APP2024/GetSegmentosAutomatismoPozosPAI";
                else
                    return $"{MyConectionData.external}/API24/VWC/APP2024/GetSegmentosAutomatismoPozosPAI";
                break;
            
            case Metodos.ConfiguracionAutomatismoSegmentos:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/API24/VWC/APP2024/GetConfiguracionSegmentosAutomatismoPozosPAI";
                else
                    return $"{MyConectionData.external}/API24/VWC/APP2024/GetConfiguracionSegmentosAutomatismoPozosPAI";
                break;
        }

        return "";
    }
    
    public string GetAddressByMethod(string method, int sistema)
    {
        switch (method)
        {
            case Metodos.AutomatismoEstaciones:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/API24/VWC/APP2024/GetEstacionesAutomatismo?idProyecto={sistema}";
                else
                    return $"{MyConectionData.external}/API24/VWC/APP2024/GetEstacionesAutomatismo?idProyecto={sistema}";
                break;
            
            case Metodos.ConfiguracionAutomatismoEstaciones:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/API24/VWC/APP2024/GetConfiguracionEstacionesAutomatismo?idProyecto={sistema}";
                else
                    return $"{MyConectionData.external}/API24/VWC/APP2024/GetConfiguracionEstacionesAutomatismo?idProyecto={sistema}";
                break;
            
            case Metodos.SendCommandSegmento:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/api24/VWC/app2024/" +
                           $"InsertConfiguracionSegmentoAutomatismo?idProyecto={(int)sistema}";
                else
                    return $"{MyConectionData.external}/api24/VWC/app2024/" +
                           $"InsertConfiguracionSegmentoAutomatismo?idProyecto={(int)sistema}";
                break;
            
            case Metodos.SendCommandEstaciones:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/api24/VWC/app2024/" +
                           $"InsertConfiguracionPozosAutomatismo?idProyecto={(int)sistema}";
                else
                    return $"{MyConectionData.external}/api24/VWC/app2024/" +
                           $"InsertConfiguracionPozosAutomatismo?idProyecto={(int)sistema}";
                break;
            case Metodos.SendCommandReconocimientoAuto:
                if (MyConectionData.useLocalHost)
                    return $"{MyConectionData.local}/api24/VWC/app2024/" +
                           $"InsertConfiguracionReconocimientoAutomatismo?idProyecto={(int)sistema}";
                else
                    return $"{MyConectionData.external}/api24/VWC/app2024/" +
                           $"InsertConfiguracionReconocimientoAutomatismo?idProyecto={(int)sistema}";
                break;
        }

        return "";
    }
    
    private IEnumerator DoRequest(string method, string address)
    {
        //print(address);
        UnityWebRequest unityWebRequest = null;
        
        switch (method)
        {
            case Metodos.AutomatismoSegmentos:
                unityWebRequest = UnityWebRequest.Get(address);
                yield return unityWebRequest.SendWebRequest();
                CallBack(unityWebRequest, method);
                break;
            case Metodos.ConfiguracionAutomatismoSegmentos:
                unityWebRequest = UnityWebRequest.Get(address);
                yield return unityWebRequest.SendWebRequest();
                CallBack(unityWebRequest, method);
                break;
            default:
                break;
        }
    }
    
    private IEnumerator DoRequest(string method, int sistema, string address)
    {
        UnityWebRequest unityWebRequest = null;
        
        switch (method)
        {
            case Metodos.AutomatismoEstaciones:
                unityWebRequest = UnityWebRequest.Get(address);
                yield return unityWebRequest.SendWebRequest();
                CallBack(unityWebRequest, method, sistema);
                break;
            case Metodos.ConfiguracionAutomatismoEstaciones:
                unityWebRequest = UnityWebRequest.Get(address);
                yield return unityWebRequest.SendWebRequest();
                CallBack(unityWebRequest, method, sistema);
                break;
            default:
                break;
        }
    }
    
    private IEnumerator DoRequestActualizacion(string method, int sistema, string address)
    {
        UnityWebRequest unityWebRequest = null;
        
        switch (method)
        {
            case Metodos.AutomatismoEstaciones:
                unityWebRequest = UnityWebRequest.Get(address);
                yield return unityWebRequest.SendWebRequest();
                CallBackActualizacion(unityWebRequest, method, sistema);
                break;
                
            case Metodos.ConfiguracionAutomatismoEstaciones:
                unityWebRequest = UnityWebRequest.Get(address);
                yield return unityWebRequest.SendWebRequest();
                CallBackActualizacion(unityWebRequest, method, sistema);
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
                case Metodos.AutomatismoSegmentos:
                    errorAutomatismoSegementosHTML = true;
                    break;
                case Metodos.ConfiguracionAutomatismoSegmentos:
                    errorConfAutomatismoSegementosHTML = true;
                    break;
            }
        }
        else
        {
            if (unityWebRequest.isDone)
            {
                switch (method)
                {
                    case Metodos.AutomatismoSegmentos:
                        errorAutomatismoSegementosHTML = false;
                        respAutomatismoSegementosHTML = true;
                        segmentosAutomatismo = JsonUtility.FromJson<SegmentosAutomatismo>(unityWebRequest.downloadHandler.text);
                        AutomatismoSegementosCout++;
                        break;
                    
                    case Metodos.ConfiguracionAutomatismoSegmentos:
                        errorConfAutomatismoSegementosHTML = false;
                        respConfAutomatismoSegementosHTML = true;
                        ConfSegmentosAutomatismo = JsonUtility.FromJson<SegmentosAutomatismo>(unityWebRequest.downloadHandler.text);
                        ConfAutomatismoSegementosCout++;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    private void CallBack(UnityWebRequest unityWebRequest, string method, int sistema)
    {
        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(unityWebRequest.error + "\n" + unityWebRequest.url);
            
            switch (method)
            {
                case Metodos.AutomatismoEstaciones:
                    errorAutomatismoEstacionesHTML = true;
                    break;
                case Metodos.ConfiguracionAutomatismoEstaciones:
                    errorConfAutomatismoEstacionesHTML = true;
                    break;
            }
        }
        else
        {
            if (unityWebRequest.isDone)
            {
                switch (method)
                {
                    case Metodos.AutomatismoEstaciones:
                        errorAutomatismoEstacionesHTML = false;
                        respAutomatismoEstacionesHTML = true;
                        estacionesSistemaAutomatismo.Find(
                                item => item.sistema == (RequestAPI.Proyectos)sistema).
                                estacionesAutomatismo = JsonUtility.FromJson<EstacionesAutomatismo>(unityWebRequest.downloadHandler.text);
                        AutomatismoEstacionesCout++;
                        break;
                    case Metodos.ConfiguracionAutomatismoEstaciones:
                        errorConfAutomatismoEstacionesHTML = false;
                        respConfAutomatismoEstacionesHTML = true;
                        ConfEstacionesSistemaAutomatismo.Find(
                                item => item.sistema == (RequestAPI.Proyectos)sistema).
                            estacionesAutomatismo = JsonUtility.FromJson<EstacionesAutomatismo>(unityWebRequest.downloadHandler.text);
                        ConfAutomatismoEstacionesCout++;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    private void CallBackActualizacion(UnityWebRequest unityWebRequest, string method, int sistema)
    {
        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(unityWebRequest.error + "\n" + unityWebRequest.url);
            
            switch (method)
            {
                case Metodos.AutomatismoEstaciones:
                    errorAutomatismoEstacionesHTML = true;
                    break;
                case Metodos.ConfiguracionAutomatismoEstaciones:
                    errorConfAutomatismoEstacionesHTML = true;
                    break;
            }
        }
        else
        {
            if (unityWebRequest.isDone)
            {
                switch (method)
                {
                    case Metodos.AutomatismoEstaciones:
                        errorAutomatismoEstacionesHTML = false;
                        respAutomatismoEstacionesHTML = true;
                        estacionesSistemaAutomatismo.Find(
                                item => item.sistema == (RequestAPI.Proyectos)sistema).
                            estacionesAutomatismo = JsonUtility.FromJson<EstacionesAutomatismo>(unityWebRequest.downloadHandler.text);
                        datosAutomatismoActualizados.Invoke();
                        AutomatismoEstacionesCout++;
                        break;
                    case Metodos.ConfiguracionAutomatismoEstaciones:
                        errorConfAutomatismoEstacionesHTML = false;
                        respConfAutomatismoEstacionesHTML = true;
                        ConfEstacionesSistemaAutomatismo.Find(
                                item => item.sistema == (RequestAPI.Proyectos)sistema).
                            estacionesAutomatismo = JsonUtility.FromJson<EstacionesAutomatismo>(unityWebRequest.downloadHandler.text);
                        datosAutomatismoActualizados.Invoke();
                        ConfAutomatismoEstacionesCout++;
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    public void SendCommandSegmento(int sistema, SegmentoAutomatismo segmento)
    {
        if (ControlLogin._singletonExists)
        {
            SetCommandValues(ControlLogin.singleton.login.Credencials.usuario, segmento);
            StartCoroutine(DoRequestSendCommand(Metodos.SendCommandSegmento, sistema));
        }
    }
    
    public IEnumerator DoRequestSendCommand(string metodo, int sistema)
    {
        UnityWebRequest unityWebRequest = null;
        Debug.Log(GetAddressByMethod(metodo, sistema));
        //print(JsonUtility.ToJson(commandAutomatismo));
        
        unityWebRequest = UnityWebRequest.Post(GetAddressByMethod(metodo, sistema),
            JsonUtility.ToJson(commandAutomatismo), "application/json");

        yield return unityWebRequest.SendWebRequest();

        CallBackSendCommand(unityWebRequest);
    }

    [TabGroup("Command")] public CommandAutomatismo commandAutomatismo;
    [TabGroup("Command")] public CommandResponse commandResponse;
    
    [Button]
    public void SetCommandValues(string _usuario, SegmentoAutomatismo segmento)
    {
        commandAutomatismo = new CommandAutomatismo();
        commandAutomatismo.Usuario = _usuario;
        commandAutomatismo.Id = segmento.ID;
        commandAutomatismo.segmentoAutomatismo = segmento;
    }
    
    public void SetCommandValues(string _usuario, int id_Segmento, EstacionesAutomatismo estaciones)
    {
        commandAutomatismo = new CommandAutomatismo();
        commandAutomatismo.Usuario = _usuario;
        commandAutomatismo.Id = id_Segmento;
        commandAutomatismo.estacionesAutomatismo = estaciones;
    }
    
    public void CallBackSendCommand(UnityWebRequest unityWebRequest)
    {
        if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(unityWebRequest.error);
            errorSendCommandSegmentoHTML = true;
        }
        else
        {
            if (unityWebRequest.isDone)
            {
                errorSendCommandSegmentoHTML = false;
                commandResponse = JsonUtility.FromJson<CommandResponse>(unityWebRequest.downloadHandler.text);
            }
        }
    }
    
    public void SendCommandEstaciones(int sistema, int id_Segmento, EstacionesAutomatismo estaciones)
    {
        if (ControlLogin._singletonExists)
        {
            SetCommandValues(ControlLogin.singleton.login.Credencials.usuario, id_Segmento, estaciones);
            StartCoroutine(DoRequestSendCommand(Metodos.SendCommandEstaciones, sistema));
        }
    }
    
    public void SendCommandReconocimientoAutomatismo(int sistema, int id_Segmento, EstacionesAutomatismo estaciones)
    {
        if (ControlLogin._singletonExists)
        {
            SetCommandValues(ControlLogin.singleton.login.Credencials.usuario, id_Segmento, estaciones);
            StartCoroutine(DoRequestSendCommand(Metodos.SendCommandReconocimientoAuto, sistema));
        }
    }
}
