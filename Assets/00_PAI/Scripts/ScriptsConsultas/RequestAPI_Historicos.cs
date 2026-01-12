using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;

public class RequestAPI_Historicos : Singleton<RequestAPI_Historicos>
{
    public Action HistoricoCallback;
    private string url;
    
    [TabGroup("Comunication")] public bool errorUpdateHistoricos;
    [TabGroup("Comunication")] public int HistoricoCout = 0;
    
    [TabGroup("Historicos")] private int idSitio;
    [TabGroup("Historicos")] private DateTime totalizadosTime1 = DateTime.Now.Subtract(TimeSpan.FromDays(2));
    [TabGroup("Historicos")] private DateTime totalizadosTime2 = DateTime.Now;
    [TabGroup("Historicos")] public RequestBoy.TipoPromedio tipoPromedio;
    
    public bool enableHistoricos = true;
    public bool simulaCallBackHistoricos;
    
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
        url = ControlRequest.singleton.listRequestAPI[0].GetAddressByMethod(Metodos.UpdateHistoricos);
        StartCoroutine(DoRequest(Metodos.UpdateHistoricos));
    }
    
    private void GetHistoricosBySistema(int _sistema)
    {
        url = ControlRequest.singleton.listRequestAPI[0].GetAddressByMethod(Metodos.UpdateHistoricos, _sistema);
        StartCoroutine(DoRequestHistoricosBySistema(Metodos.UpdateHistoricos, _sistema));
    }
    
    private IEnumerator DoRequest(string method)
    {
        string address = $"{url}";
        
        UnityWebRequest unityWebRequest = null;
        
        switch (method)
        {
            case Metodos.UpdateHistoricos:
            {
                if (enableHistoricos)
                {
                    SiteDescription sitio = ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.infraestructura.Sites.Find(
                        item => item.Id == idSitio);
                    
                    List<SignalsDescriptionContainerC> signalsDescriptionContainer = sitio.SignalsDescriptionContainer;

                    ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Gasto.Clear();
                    ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Presion.Clear();
                    ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Totalizado.Clear();
                    ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Bomba.Clear();
                    ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Nivel.Clear();
                    ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Automatismo.Clear();
                    ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.FalloAutomatismo.Clear();

                    SignalsDescriptionContainerC signalDescriptionC_G =
                        signalsDescriptionContainer.Find(item => 
                            item.TipoSignal == (int)SignalBase.TipoSignalEnum.GASTO);
                    SignalsDescriptionContainerC signalDescriptionC_P =
                        signalsDescriptionContainer.Find(item =>
                            item.TipoSignal == (int)SignalBase.TipoSignalEnum.PRESION);
                    SignalsDescriptionContainerC signalDescriptionC_T =
                        signalsDescriptionContainer.Find(item =>
                            item.TipoSignal == (int)SignalBase.TipoSignalEnum.TOTALIZADO);
                    SignalsDescriptionContainerC signalDescriptionC_B =
                        signalsDescriptionContainer.Find(item => 
                            item.TipoSignal == (int)SignalBase.TipoSignalEnum.BOMBA);
                    SignalsDescriptionContainerC signalDescriptionC_N =
                        signalsDescriptionContainer.Find(item => 
                            item.TipoSignal == (int)SignalBase.TipoSignalEnum.NIVEL);
                    SignalsDescriptionContainerC signalDescriptionC_A =
                        signalsDescriptionContainer.Find(item => 
                            item.TipoSignal == (int)SignalBase.TipoSignalEnum.AUTOMATISMO);
                    SignalsDescriptionContainerC signalDescriptionC_FA =
                        signalsDescriptionContainer.Find(item => 
                            item.TipoSignal == (int)SignalBase.TipoSignalEnum.ARRANQUEFALLIDO);

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
                    
                    if (signalDescriptionC_A != null)
                    {
                        RequestBoy DataH = new RequestBoy();
                        DataH.IdSignal = signalDescriptionC_A.SignalsDescription.First().IdSignal;
                        DataH.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                        DataH.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                        unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                            JsonUtility.ToJson(DataH), "application/json");

                        yield return unityWebRequest.SendWebRequest();
                        SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.AUTOMATISMO);
                    }
                    
                    if (signalDescriptionC_FA != null)
                    {
                        RequestBoy DataH = new RequestBoy();
                        DataH.IdSignal = signalDescriptionC_FA.SignalsDescription.First().IdSignal;
                        DataH.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                        DataH.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                        unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                            JsonUtility.ToJson(DataH), "application/json");

                        yield return unityWebRequest.SendWebRequest();
                        SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.ARRANQUEFALLIDO);
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
    
    private IEnumerator DoRequestHistoricosBySistema(string method, int sistema)
    {
        string address = $"{url}";
        
        UnityWebRequest unityWebRequest = null;
        
        if (enableHistoricos)
        {
            SiteDescription sitio =
                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.infraestructura.Sites.Find(item => item.Id == idSitio + (sistema * 100));
            
            if (sitio != null)
            {
                List<SignalsDescriptionContainerC> signalsDescriptionContainer = sitio.SignalsDescriptionContainer;

                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Gasto.Clear();
                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Presion.Clear();
                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Totalizado.Clear();
                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Bomba.Clear();
                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Nivel.Clear();
                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Automatismo.Clear();
                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.FalloAutomatismo.Clear();

                SignalsDescriptionContainerC signalDescriptionC_G =
                    signalsDescriptionContainer.Find(item =>
                        item.TipoSignal == (int)SignalBase.TipoSignalEnum.GASTO);
                SignalsDescriptionContainerC signalDescriptionC_P =
                    signalsDescriptionContainer.Find(item =>
                        item.TipoSignal == (int)SignalBase.TipoSignalEnum.PRESION);
                SignalsDescriptionContainerC signalDescriptionC_T =
                    signalsDescriptionContainer.Find(item =>
                        item.TipoSignal == (int)SignalBase.TipoSignalEnum.TOTALIZADO);
                SignalsDescriptionContainerC signalDescriptionC_B =
                    signalsDescriptionContainer.Find(item =>
                        item.TipoSignal == (int)SignalBase.TipoSignalEnum.BOMBA);
                SignalsDescriptionContainerC signalDescriptionC_N =
                    signalsDescriptionContainer.Find(item =>
                        item.TipoSignal == (int)SignalBase.TipoSignalEnum.NIVEL);
                SignalsDescriptionContainerC signalDescriptionC_A =
                    signalsDescriptionContainer.Find(item =>
                        item.TipoSignal == (int)SignalBase.TipoSignalEnum.AUTOMATISMO);
                SignalsDescriptionContainerC signalDescriptionC_FA =
                    signalsDescriptionContainer.Find(item =>
                        item.TipoSignal == (int)SignalBase.TipoSignalEnum.ARRANQUEFALLIDO);

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

                if (signalDescriptionC_A != null)
                {
                    RequestBoy DataH = new RequestBoy();
                    DataH.IdSignal = signalDescriptionC_A.SignalsDescription.First().IdSignal;
                    DataH.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                    DataH.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                    unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                        JsonUtility.ToJson(DataH), "application/json");

                    yield return unityWebRequest.SendWebRequest();
                    SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.AUTOMATISMO);
                }

                if (signalDescriptionC_FA != null)
                {
                    RequestBoy DataH = new RequestBoy();
                    DataH.IdSignal = signalDescriptionC_FA.SignalsDescription.First().IdSignal;
                    DataH.FechaInicial = GetFechaFormatConsulta(totalizadosTime1);
                    DataH.FechaFinal = GetFechaFormatConsulta(totalizadosTime2);

                    unityWebRequest = UnityWebRequest.Post(address + "&tipoPromedio=" + (int)tipoPromedio,
                        JsonUtility.ToJson(DataH), "application/json");

                    yield return unityWebRequest.SendWebRequest();
                    SetDataHistoricos(unityWebRequest, method, SignalBase.TipoSignalEnum.ARRANQUEFALLIDO);
                }
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
                    case Metodos.UpdateHistoricos:

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
    
    public string GetFechaFormatConsulta(DateTime _date)
    {
        return _date.Year + "-" + _date.Month.ToString("00")+ "-" + _date.Day.ToString("00") + " " +
               _date.Hour.ToString("00") + ":" + _date.Minute.ToString("00") + ":" + _date.Second.ToString("00");
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
                                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Gasto = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.PRESION:
                                //Debug.Log("Data: " + unityWebRequest.downloadHandler.text);
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Presion = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.TOTALIZADO:
                                //Debug.Log("Data: " + unityWebRequest.downloadHandler.text);
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Totalizado = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.BOMBA:
                                //Debug.Log("Data: " + unityWebRequest.downloadHandler.text);
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Bomba = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.NIVEL:
                                //Debug.Log("Data: " + unityWebRequest.downloadHandler.text);
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Nivel = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.AUTOMATISMO:
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.Automatismo = reportes.Reporte;
                                HistoricoCout++;
                                break;
                            
                            case SignalBase.TipoSignalEnum.ARRANQUEFALLIDO:
                                errorUpdateHistoricos = false;
                                reportes = JsonUtility.FromJson<Reportes>(unityWebRequest.downloadHandler.text);
                                ControlRequest.singleton.listRequestAPI[0].dataRequestAPI.historicosBySitio.FalloAutomatismo = reportes.Reporte;
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
}
