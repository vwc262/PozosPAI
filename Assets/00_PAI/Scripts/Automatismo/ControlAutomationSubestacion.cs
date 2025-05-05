using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ControlAutomationSubestacion : MonoBehaviour
{
    public List<ControlAutomationSitio> controlAutomatismoList = new List<ControlAutomationSitio>();
    public List<ControlAutomationSitio> controlOrderAux = new List<ControlAutomationSitio>();
    public List<ControlAutomationSitio> controlOrder = new List<ControlAutomationSitio>();

    public string Nombre;
    public int startupTime;
    public int stabilitationTime;
    public int windowTime;
    public int toleranceVoltage;
    
    public TMPro.TMP_Text texto_Nombre;

    public bool useConfigurationData;
    public bool inputEnable;
    public bool isTolarenceEditable;
    
    public TMPro.TMP_InputField input_starupTime;
    public TMPro.TMP_InputField input_stabilitationTime;
    public TMPro.TMP_InputField input_windowTime;
    public TMPro.TMP_InputField input_toleranceVoltage;
    public Toggle input_useConfigurationData;
    public TMPro.TMP_Text texto_useConfigurationData;
    public Color colorConfiguracion;
    public Color colorEnSitio;
    public Button ButtonActualizarLista;
    public Button ButtonActualizarSubestacion;
    
    public Slider slider_toleranceVoltage;
    
    public int segmentoAutomatismoID;
    
    private void Start()
    {
        inputEnable = false;
        SetInputEnable(inputEnable);
    }

    private void OnDisable()
    {
        if (inputEnable)
        {
            inputEnable = false;
            SetInputEnable(inputEnable);
        }
    }

    private void OnEnable()
    {
        SetMessageData();
    }

    public void ClearLists()
    {
        foreach (var estacion in controlAutomatismoList)
        {
            Destroy(estacion.gameObject);
        }
        
        controlAutomatismoList.Clear();
    }

    public void ActivateLoginUI()
    {
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.ActivateLoginPanel();
    }

    public void SendDataSegmento()
    {
        if (RequestAPI_Auto._singletonExists)
        {
            SegmentoAutomatismo segmento = RequestAPI_Auto.singleton.segmentosAutomatismo.Segmentos.Find(
                x => x.ID == segmentoAutomatismoID);
            
            SegmentoAutomatismo segmentoAutomatismoAux = new SegmentoAutomatismo();

            segmentoAutomatismoAux.ID = segmento.ID;
            segmentoAutomatismoAux.Descripcion = segmento.Descripcion;
            
            segmentoAutomatismoAux.Tolerancia = toleranceVoltage;
            segmentoAutomatismoAux.T1 = startupTime;
            segmentoAutomatismoAux.T2 = windowTime;

            if (RequestAPI_Auto._singletonExists)
            {
                switch (segmentoAutomatismoAux.ID)
                {
                    case 1:
                        RequestAPI_Auto.singleton.SendCommandSegmento(14, segmentoAutomatismoAux);
                        break;
                    case 2:
                        RequestAPI_Auto.singleton.SendCommandSegmento(17, segmentoAutomatismoAux);
                        break;
                    case 3:
                    case 4:
                        RequestAPI_Auto.singleton.SendCommandSegmento(18, segmentoAutomatismoAux);
                        break;
                }
            }

            if (ControlAutomation._singletonExists)
                ControlAutomation.singleton.ActiveAnimation();
        }
    }
    
    public void SendDataEstaciones()
    {
        if (RequestAPI_Auto._singletonExists)
        {
            SegmentoAutomatismo segmento = RequestAPI_Auto.singleton.segmentosAutomatismo.Segmentos.Find(
                x => x.ID == segmentoAutomatismoID);

            EstacionesAutomatismo estacionesAutomatismoAux = new EstacionesAutomatismo();
            estacionesAutomatismoAux.EstacionAutomatismos = new List<EstacionAutomatismo>();

            foreach (var estacion in controlAutomatismoList)
            {
                if (estacion.isDataChanged)
                {
                    EstacionAutomatismo estacionAux = new EstacionAutomatismo();

                    switch (segmento.ID)
                    {
                        case 1:
                            estacionAux.SetDataEstacion(RequestAPI_Auto.singleton.estacionesSistemaAutomatismo.Find(
                                    x => x.sistema == (RequestAPI.Proyectos)14).estacionesAutomatismo
                                .EstacionAutomatismos
                                .Find(
                                    x => x.IdEstacion == estacion.dataSitio.idSitio % 100));
                            break;
                        case 2:
                            estacionAux.SetDataEstacion(RequestAPI_Auto.singleton.estacionesSistemaAutomatismo.Find(
                                    x => x.sistema == (RequestAPI.Proyectos)17).estacionesAutomatismo
                                .EstacionAutomatismos
                                .Find(
                                    x => x.IdEstacion == estacion.dataSitio.idSitio % 100));
                            break;
                        case 3:
                        case 4:
                            estacionAux.SetDataEstacion(RequestAPI_Auto.singleton.estacionesSistemaAutomatismo.Find(
                                    x => x.sistema == (RequestAPI.Proyectos)18).estacionesAutomatismo
                                .EstacionAutomatismos
                                .Find(
                                    x => x.IdEstacion == estacion.dataSitio.idSitio % 100));
                            break;
                    }

                    if (estacionAux.IdEstacion != 0)
                    {
                        //Modifica datos
                        estacionAux.VNominal = estacion.VNominal;
                        estacionAux.Automatismo = estacion.isActiveAutomation ? 1 : 2;
                        estacionAux.Secuencia = estacion.index;
                        estacionesAutomatismoAux.EstacionAutomatismos.Add(estacionAux);
                    }
                }
            }

            if (estacionesAutomatismoAux.EstacionAutomatismos.Count > 0)
            {
                switch (segmento.ID)
                {
                    case 1:
                        RequestAPI_Auto.singleton.SendCommandEstaciones(
                            14, segmento.ID, estacionesAutomatismoAux);
                        if (RequestAPI_Auto._singletonExists)
                            RequestAPI_Auto.singleton.ForzarActualizacionDatos(14);
                        break;
                    case 2:
                        RequestAPI_Auto.singleton.SendCommandEstaciones(
                            17, segmento.ID, estacionesAutomatismoAux);
                        if (RequestAPI_Auto._singletonExists)
                            RequestAPI_Auto.singleton.ForzarActualizacionDatos(17);
                        break;
                    case 3:
                    case 4:
                        RequestAPI_Auto.singleton.SendCommandEstaciones(
                            18, segmento.ID, estacionesAutomatismoAux);
                        if (RequestAPI_Auto._singletonExists)
                            RequestAPI_Auto.singleton.ForzarActualizacionDatos(18);
                        break;
                }
            }
            
            if (ControlAutomation._singletonExists)
                ControlAutomation.singleton.ActiveAnimation();
        }
    }

    public void SendDataReconocimientoAutomatismo(int ID_estacion)
    {
        if (RequestAPI_Auto._singletonExists)
        {
            SegmentoAutomatismo segmento = RequestAPI_Auto.singleton.segmentosAutomatismo.Segmentos.Find(
                x => x.ID == segmentoAutomatismoID);

            EstacionesAutomatismo estacionesAutomatismoAux = new EstacionesAutomatismo();
            estacionesAutomatismoAux.EstacionAutomatismos = new List<EstacionAutomatismo>();

            EstacionAutomatismo estacion = new EstacionAutomatismo();
            estacion.IdEstacion = ID_estacion;
            estacion.IdSegmento = segmentoAutomatismoID;
            estacion.BanderaArranqueFallido = 2;
            
            estacionesAutomatismoAux.EstacionAutomatismos.Add(estacion);
            
            switch (segmento.ID)
            {
                case 1:
                    RequestAPI_Auto.singleton.SendCommandReconocimientoAutomatismo(
                        14, segmento.ID, estacionesAutomatismoAux);
                    if (RequestAPI_Auto._singletonExists)
                        RequestAPI_Auto.singleton.ForzarActualizacionDatos(14);
                    break;
                case 2:
                    RequestAPI_Auto.singleton.SendCommandReconocimientoAutomatismo(
                        17, segmento.ID, estacionesAutomatismoAux);
                    if (RequestAPI_Auto._singletonExists)
                        RequestAPI_Auto.singleton.ForzarActualizacionDatos(17);
                    break;
                case 3:
                case 4:
                    RequestAPI_Auto.singleton.SendCommandReconocimientoAutomatismo(
                        18, segmento.ID, estacionesAutomatismoAux);
                    if (RequestAPI_Auto._singletonExists)
                        RequestAPI_Auto.singleton.ForzarActualizacionDatos(18);
                    break;
            }
        }
    }

    public void SetSubestacionByID(int segmentoID)
    {
        segmentoAutomatismoID = segmentoID;
        SetDataSubestacion();
    }
    
    public void SetDataSubestacion()
    {
        if (RequestAPI_Auto._singletonExists)
        {
            SegmentoAutomatismo segmento;
            
            segmento = RequestAPI_Auto.singleton.ConfSegmentosAutomatismo.Segmentos.Find(
            x => x.ID == segmentoAutomatismoID);
            
            if (segmento != null)
            {
                startupTime = segmento.T1;
                windowTime = segmento.T2;
            }

            segmento = RequestAPI_Auto.singleton.segmentosAutomatismo.Segmentos.Find(
                x => x.ID == segmentoAutomatismoID);

            if (segmento != null)
            {
                Nombre = $"Transformador: {segmento.Descripcion}";
                toleranceVoltage = segmento.Tolerancia;
            }
            
            SetDataSubestacionUI();
        }
    }

    public void SetDataSubestacionUI()
    {
        if (texto_Nombre != null) texto_Nombre.text = Nombre;
        
        if (input_starupTime != null) input_starupTime.text = startupTime.ToString();
        if (input_stabilitationTime != null) input_stabilitationTime.text = stabilitationTime.ToString();
        if (input_windowTime != null) input_windowTime.text = windowTime.ToString();
        if (input_toleranceVoltage != null) input_toleranceVoltage.text = toleranceVoltage.ToString();
        if (slider_toleranceVoltage != null) slider_toleranceVoltage.value = toleranceVoltage;
    }

    public void SetUseConfigurationData(bool value)
    {
        useConfigurationData = value;

        foreach (var automationSitio in controlAutomatismoList)
        {
            automationSitio.UpdateEditores();
            automationSitio.UpdateData();
        }
        
        SetDataSubestacion();

        SetMessageData();

        SetTextToggle();
    }
    
    public void ToggleConfigurationData()
    {
        useConfigurationData = !useConfigurationData;
        
        foreach (var automationSitio in controlAutomatismoList)
        {
            automationSitio.UpdateData();
        }

        SetDataSubestacion();

        SetMessageData();
        
        SetTextToggle();
    }
    
    public void SetTextToggle()
    {
        if (texto_useConfigurationData)
        {
            if (useConfigurationData)
                texto_useConfigurationData.text = "Usando datos de configuración";
            else
                texto_useConfigurationData.text = "Usando datos almacenados en sitio";
        }
    }

    public void SetMessageData()
    {
        if (ControlAutomation._singletonExists)
        {
            if (useConfigurationData)
            {
                ControlAutomation.singleton.SetMessage("Datos de configuración", colorConfiguracion);
            }
            else
            {
                ControlAutomation.singleton.SetMessage("Datos almacenados en sitio", colorEnSitio);
            }
        }
    }
    
    public void ToggleInput()
    {
        inputEnable = !inputEnable;
        SetInputEnable(inputEnable);
    }

    public void SetInputEnable(bool enable)
    {
        if (input_starupTime != null) input_starupTime.interactable = enable;
        if (input_stabilitationTime != null) input_stabilitationTime.interactable = enable;
        if (input_windowTime != null) input_windowTime.interactable = enable;
        if (input_toleranceVoltage != null) input_toleranceVoltage.interactable = enable && isTolarenceEditable;
        if (slider_toleranceVoltage != null) slider_toleranceVoltage.interactable = enable && isTolarenceEditable;
        if (ButtonActualizarLista != null) ButtonActualizarLista.interactable = enable;
        if (ButtonActualizarSubestacion != null) ButtonActualizarSubestacion.interactable = enable;
        
        foreach (var sitio in controlAutomatismoList)
        {
            sitio.SetEnableEditors(enable);
        }

        if (enable)
        {
            if (input_useConfigurationData != null)
            {
                input_useConfigurationData.isOn = true;
                input_useConfigurationData.interactable = false;
                input_useConfigurationData.onValueChanged.Invoke(input_useConfigurationData.isOn);
            }
        }
        else
        {
            if (input_useConfigurationData != null)
            {
                //input_useConfigurationData.isOn = enable;
                input_useConfigurationData.interactable = true;
                input_useConfigurationData.onValueChanged.Invoke(input_useConfigurationData.isOn);
            }

            SetDataSubestacion();
        }
    }

    public void MoveUP(ControlAutomationSitio sitio)
    {
        int index = sitio.index;

        if (index != 1)
        {
            controlAutomatismoList[index - 2].SetIndex(index);
            sitio.SetIndex(index - 1);
        }

        ReorderSitios();
    }
    
    public void MoveDown(ControlAutomationSitio sitio)
    {
        int index = sitio.index;

        if (index != controlAutomatismoList.Count)
        {
            controlAutomatismoList[index].SetIndex(index);
            sitio.SetIndex(index + 1);
        }

        ReorderSitios();
    }
    
    public void ReorderSitios()
    {
        controlAutomatismoList = controlAutomatismoList.OrderBy(x => x.index).ToList();

        foreach (var sitio in controlAutomatismoList)
            sitio.GetComponent<Transform>().SetSiblingIndex(controlAutomatismoList.IndexOf(sitio));
    }

    public IEnumerator SetDataSimulation()
    {
        yield return new WaitForSeconds(5f);

        foreach (var sitioA in controlAutomatismoList)
        {
            //sitioA.SetNominalVoltage(nominalVoltage);
            sitioA.SetToleranceVoltage(toleranceVoltage);
            sitioA.SetStartupTime(startupTime);
            sitioA.SetStabilitationTime(stabilitationTime);
            sitioA.SetWindowTime(windowTime);
        }
    }

    public void SetStartupTime(string time)
    {
        if (time.Contains("-"))
        {
            if (input_starupTime != null)
                input_starupTime.text = time.Replace("-", "");
        }
        else
            startupTime = ControlAutomation.ValidaInputPositiveInt(time);
    }
    
    public void SetStabilitationTime(string time)
    {
        if (time.Contains("-"))
        {
            if (input_stabilitationTime != null)
                input_stabilitationTime.text = time.Replace("-", "");
        }
        else
            stabilitationTime = ControlAutomation.ValidaInputPositiveInt(time);
    }
    
    public void SetWindowTime(string time)
    {
        if (time.Contains("-"))
        {
            if (input_windowTime != null)
                input_windowTime.text = time.Replace("-", "");
        }
        else
            windowTime = ControlAutomation.ValidaInputPositiveInt(time);
    }

    public void InitSubestacion()
    {
        RecreateIndex();
        
        inputEnable = false;
        SetInputEnable(inputEnable);
    }

    public void RecreateIndex()
    {
        controlOrder = Enumerable.Repeat((ControlAutomationSitio)null, controlAutomatismoList.Count).ToList();
        
        foreach (var estacion in controlAutomatismoList.OrderBy(x=> x.index))
        {
            if (estacion.index > 0 && controlOrder[estacion.index - 1] == null)
            {
                controlOrder[estacion.index - 1] = estacion;
            }
            else
            {
                controlOrderAux.Add(estacion);
            }
        }

        while (controlOrderAux.Count > 0)
        {
            controlOrder[controlOrder.FindIndex(x => x == null)] = controlOrderAux[0];
            
            controlOrderAux.RemoveAt(0);
        }

        int cont = 0;
        //foreach (var estacion in controlAutomatismoList.OrderBy(x=> x.index))
        foreach (var estacion in controlOrder)
        {
            cont++;
            estacion.recreateIndex(cont);
        }

        ReorderSitios();
    }
}
