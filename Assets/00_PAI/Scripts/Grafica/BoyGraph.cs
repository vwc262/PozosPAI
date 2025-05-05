using System;
using System.Collections.Generic;
using System.Linq;
using HutongGames.PlayMaker;
using Sirenix.OdinInspector;
using UI.Dates;
using UnityEngine;
using UnityEngine.Serialization;

public class BoyGraph : MonoBehaviour
{
    // Hola Boy
    public enum TipoDatosGrafico
    {
        NONE,
        GASTO,
        PRESION,
        TOTALIZADO,
        BOMBA,
        NIVEL
    }

    public WMG_Axis_Graph axisGraph;
    public UI.Dates.DatePicker datePicker;

    public List<Reporte> SerieDataGasto;
    public List<Reporte> SerieDataPresion;
    public List<Reporte> SerieDataTotalizado;
    public List<Reporte> SerieDataBomba;
    public List<Reporte> SerieDataNivel;
    public List<Vector2> subSerieDataGasto;
    public List<Vector2> subSerieDataPresion;
    public List<Vector2> subSerieDataTotalizado;
    public List<Vector2> subSerieDataBomba;
    public List<Vector2> subSerieDataNivel;
    public List<string> LabelsEjeX;
    public string serieName = "Totalizado";
    public int maxDataValueGasto;
    public int maxDataValuePresion;
    public int maxDataValueTotalizado;
    public int maxDataValueBomba;
    public int maxDataValueNivel;
    
    public SerializableDate date01;
    public SerializableDate date02;
    
    public static int idSitio;
    public static int EstructuraSitio;
    public static RequestBoy.TipoPromedio tipoPromedio;
    public static DateTime minDate;
    public static DateTime maxDate;

    public WMG_Series serieTotalizado;
    public WMG_Series serieGasto;
    public WMG_Series seriePresion;
    public WMG_Series serieBomba;
    public WMG_Series serieNivel;

    public TipoDatosGrafico graficoIzquierdo;
    public TipoDatosGrafico graficoDerecho;

    public GameObject UI_ActiveGasto;
    public GameObject UI_ActivePresion;
    public GameObject UI_ActiveTotalizado;
    public GameObject UI_ActiveBomba;
    public GameObject UI_ActiveNivel;
    
    public GameObject UI_Calendar;
    public GameObject UI_Graph;

    public TMPro.TMP_Text UI_Calendar_FechaMin;
    public TMPro.TMP_Text UI_Calendar_FechaMax;
    
    public TMPro.TMP_Text UI_FechaMin;
    public TMPro.TMP_Text UI_FechaMax;
    
    public GameObject panelActualizando;
    public GameObject panelNoDatos;

    public GameObject ButtonGraficGasto;
    public GameObject ButtonGraficPresion;
    public GameObject ButtonGraficTotalizado;
    public GameObject ButtonGraficBomba;
    public GameObject ButtonGraficNivel;

    public int numLabelsEjeX = 25;

    private void Start()
    {
        setFechas(DateTime.Today, DateTime.Today);

        tipoPromedio = RequestBoy.TipoPromedio.DIAS;

        graficoIzquierdo = TipoDatosGrafico.GASTO;
        graficoDerecho = TipoDatosGrafico.PRESION;
        
        if (ControlBoyGraph._singletonExists)
            ControlBoyGraph.singleton.ListGraph.Add(this);
    }

    public void setFechas(SerializableDate _date01, SerializableDate _date02)
    {
        date01 = _date01;
        date02 = _date02;

        if (date01.Date < date02.Date)
        {
            minDate = date01.Date;
            maxDate = date02.Date.AddDays(1);
        }
        else
        {
            minDate = date02.Date;
            maxDate = date01.Date.AddDays(1);
        }

        if (UI_FechaMin != null)
            UI_FechaMin.text = GetDateString(minDate);
        
        if (UI_FechaMax != null)
            UI_FechaMax.text = GetDateString(maxDate.AddSeconds(-1));
    }

    public String GetDateString(DateTime _date)
    {
        return _date.Day + "/" + _date.Month + "/" + _date.Year;
    }

    //Select sitio
    public void UpdateInfoSitio(DataSitio _DataSitio)
    {
        if (RequestAPI.Instance.sistema == RequestAPI.Proyectos.PozosPAI)
        {
            idSitio = _DataSitio.idSitio % 100;
            EstructuraSitio = (int)_DataSitio.Estructura;
            
            RequestAPI.Instance.GetHistricosByDates(
                idSitio,(int)_DataSitio.Estructura,
                minDate,
                maxDate,
                tipoPromedio,
                HistoricosCallBack);
        }
        else
        {
            idSitio = _DataSitio.idSitio;
            EstructuraSitio = (int)_DataSitio.Estructura;
            
            RequestAPI.Instance.GetHistricosByDates(
                idSitio,
                minDate,
                maxDate,
                tipoPromedio,
                HistoricosCallBack);
        }
    }
    
	[Button]
    public void UpdateSerieData()
    {
        setSubSerieData();

        updateSeries();
    }
    
    public void SetSerieDataIzquierda(WMG_Series _serie, List<Vector2> _datos, int _maxDataValue, int _numTicks, bool enableLabels = true)
    {
        axisGraph.yAxis.AxisMaxValue = _maxDataValue;
        axisGraph.yAxis.AxisNumTicks = _numTicks;
        axisGraph.yAxis.hideLabels = !enableLabels;
        
        _serie.gameObject.SetActive(true);
        _serie.useSecondYaxis = false;
        _serie.pointValues.SetList(_datos);
        _serie.extraXSpace = 35 - ((_datos.Count - 10f) * 1.8f);
        if (_serie.extraXSpace < 10) _serie.extraXSpace = 10;
        
        if (gameObject.activeInHierarchy)
            axisGraph.Refresh();
    }
    
    public void SetSerieDataDerecha(WMG_Series _serie, List<Vector2> _datos, int _maxDataValue, int _numTicks, bool enableLabels = true)
    {
        axisGraph.yAxis2.AxisMaxValue = _maxDataValue;
        axisGraph.yAxis2.AxisNumTicks = _numTicks;
        axisGraph.yAxis2.hideLabels = !enableLabels;

        _serie.gameObject.SetActive(true);
        _serie.useSecondYaxis = true;
        _serie.pointValues.SetList(_datos);
        _serie.extraXSpace = 35 - ((_datos.Count - 10f) * 1.8f);
        if (_serie.extraXSpace < 10) _serie.extraXSpace = 10;
        
        if (gameObject.activeInHierarchy)
            axisGraph.Refresh();
    }

    public void updateSeries()
    {
        serieGasto.gameObject.SetActive(false);
        seriePresion.gameObject.SetActive(false);
        serieTotalizado.gameObject.SetActive(false);
        serieBomba.gameObject.SetActive(false);
        serieNivel.gameObject.SetActive(false);
        
        if (UI_ActiveGasto != null) UI_ActiveGasto.SetActive(false);
        if (UI_ActivePresion != null) UI_ActivePresion.SetActive(false);
        if (UI_ActiveTotalizado != null) UI_ActiveTotalizado.SetActive(false);
        if (UI_ActiveBomba != null) UI_ActiveBomba.SetActive(false);
        if (UI_ActiveNivel != null) UI_ActiveNivel.SetActive(false);

        axisGraph.yAxis.hideLabels = true;
        axisGraph.yAxis2.hideLabels = true;

        switch (graficoIzquierdo)
        {
            case TipoDatosGrafico.GASTO:
                if (UI_ActiveGasto != null) UI_ActiveGasto.SetActive(true);
                SetSerieDataIzquierda(serieGasto, subSerieDataGasto, maxDataValueGasto, 8);
                SetDataLabelsEjeX(SerieDataGasto);
                break;
            case TipoDatosGrafico.PRESION:
                if (UI_ActivePresion != null) UI_ActivePresion.SetActive(true);
                SetSerieDataIzquierda(seriePresion, subSerieDataPresion, maxDataValuePresion, 8);
                SetDataLabelsEjeX(SerieDataPresion);
                break;
            case TipoDatosGrafico.TOTALIZADO:
                if (UI_ActiveTotalizado != null) UI_ActiveTotalizado.SetActive(true);
                SetSerieDataIzquierda(serieTotalizado, subSerieDataTotalizado, maxDataValueTotalizado, 8);
                SetDataLabelsEjeX(SerieDataTotalizado);
                break;
            case TipoDatosGrafico.BOMBA:
                if (UI_ActiveBomba != null) UI_ActiveBomba.SetActive(true);
                SetSerieDataIzquierda(serieBomba, subSerieDataBomba, maxDataValueBomba, 4, false);
                SetDataLabelsEjeX(SerieDataBomba);
                break;
            case TipoDatosGrafico.NIVEL:
                if (UI_ActiveNivel != null) UI_ActiveNivel.SetActive(true);
                SetSerieDataIzquierda(serieNivel, subSerieDataNivel, maxDataValueNivel, 10);
                SetDataLabelsEjeX(SerieDataNivel);
                break;
        }
        
        switch (graficoDerecho)
        {
            case TipoDatosGrafico.GASTO:
                if (UI_ActiveGasto != null) UI_ActiveGasto.SetActive(true);
                SetSerieDataDerecha(serieGasto, subSerieDataGasto, maxDataValueGasto, 8);
                break;
            case TipoDatosGrafico.PRESION:
                if (UI_ActivePresion != null) UI_ActivePresion.SetActive(true);
                SetSerieDataDerecha(seriePresion, subSerieDataPresion, maxDataValuePresion, 8);
                break;
            case TipoDatosGrafico.TOTALIZADO:
                if (UI_ActiveTotalizado != null) UI_ActiveTotalizado.SetActive(true);
                SetSerieDataDerecha(serieTotalizado, subSerieDataTotalizado, maxDataValueTotalizado, 8);
                break;
            case TipoDatosGrafico.BOMBA:
                if (UI_ActiveBomba != null) UI_ActiveBomba.SetActive(true);
                SetSerieDataDerecha(serieBomba, subSerieDataBomba, maxDataValueBomba, 4, false);
                break;
            case TipoDatosGrafico.NIVEL:
                if (UI_ActiveNivel != null) UI_ActiveNivel.SetActive(true);
                SetSerieDataDerecha(serieNivel, subSerieDataNivel, maxDataValueNivel, 10);
                break;
        }
        
        if (LabelsEjeX.Count <= numLabelsEjeX)
            axisGraph.groups.SetList(LabelsEjeX);
        else
        {
            float desp = LabelsEjeX.Count / (float)numLabelsEjeX;
            List<string> auxList = new List<string>();

            for (int i = 0; i < numLabelsEjeX; i++)
            {
                auxList.Add(LabelsEjeX[(int)(i*desp)]);
            }
            
            axisGraph.groups.SetList(auxList);
        }

        if (tipoPromedio == RequestBoy.TipoPromedio.HORAS)
            axisGraph.xAxis.AxisLabelSize = 30;
        else
            axisGraph.xAxis.AxisLabelSize = 40;
    }

    private void SetDataLabelsEjeX(List<Reporte> serieDatos)
    {
        LabelsEjeX.Clear();
        
        for (int i = 0; i < serieDatos.Count; i++)
        {
            if (tipoPromedio == RequestBoy.TipoPromedio.HORAS)
                LabelsEjeX.Add(serieDatos[i].Tiempo.Substring(5,5) + "   " + serieDatos[i].Tiempo.Substring(11,2) + "h");
            else
                LabelsEjeX.Add(serieDatos[i].Tiempo.Substring(5,5));
        }
    }
    
    // [Button]
    // public void SetSerieData()
    // {
    //     axisGraph.yAxis.AxisMaxValue = maxDataValue;
    //     axisGraph.yAxis2.AxisMaxValue = maxDataValue;
    //     axisGraph.groups.SetList(LabelsEjeX);
    //     
    //     serieGasto = axisGraph.lineSeries[0].GetComponent<WMG_Series>();
    //     serieGasto.pointValues.SetList(subSerieDataGasto);
    //     serieGasto.extraXSpace = 35 - ((subSerieDataGasto.Count - 10f) * 1.8f);
    //     if (serieGasto.extraXSpace < 10) serieGasto.extraXSpace = 10;
    //     
    //     seriePresion = axisGraph.lineSeries[1].GetComponent<WMG_Series>();
    //     seriePresion.pointValues.SetList(subSerieDataPresion);
    //     seriePresion.extraXSpace = 35 - ((subSerieDataPresion.Count - 10f) * 1.8f);
    //     if (seriePresion.extraXSpace < 10) seriePresion.extraXSpace = 10;
    //
    //     serieTotalizado = axisGraph.lineSeries[2].GetComponent<WMG_Series>();
    //     serieTotalizado.pointValues.SetList(subSerieDataTotalizado);
    //     serieTotalizado.extraXSpace = 35 - ((subSerieDataTotalizado.Count - 10f) * 1.8f);
    //     if (serieTotalizado.extraXSpace < 10) serieTotalizado.extraXSpace = 10;
    //     
    //     axisGraph.Refresh();
    // }

    public void setSubSerieData()
    { 
        subSerieDataGasto.Clear();
        subSerieDataPresion.Clear();
        subSerieDataTotalizado.Clear();
        subSerieDataBomba.Clear();
        subSerieDataNivel.Clear();
        maxDataValueGasto = 10;
        maxDataValuePresion = 1;
        maxDataValueTotalizado = 10;
        maxDataValueBomba = 3;
        maxDataValueNivel = 1;

        // for (int i = 0; i < SerieDataGasto.Count; i++)
        // {
        //      subSerieDataGasto.Add(new Vector2(i, 
        //          (SerieDataGasto[i].Valor >= 0? SerieDataGasto[i].Valor : 0)));
        //     
        //      if (maxDataValueGasto < subSerieDataGasto.Last().y)
        //          maxDataValueGasto = (int)(subSerieDataGasto.Last().y + 1);
        // }
        
        // for (int i = 0; i < SerieDataPresion.Count; i++)
        // {
        //     subSerieDataPresion.Add(new Vector2(i, 
        //         (SerieDataPresion[i].Valor >= 0? SerieDataPresion[i].Valor : 0)));
        //
        //     if (maxDataValuePresion < subSerieDataPresion.Last().y)
        //         maxDataValuePresion = (int)(subSerieDataPresion.Last().y + 1);
        // }
        
        // for (int i = 0; i < SerieDataTotalizado.Count; i++)
        // {
        //     subSerieDataTotalizado.Add(new Vector2(i, 
        //         (SerieDataTotalizado[i].Valor >= 0? SerieDataTotalizado[i].Valor : 0)));
        //
        //     if (maxDataValueTotalizado < subSerieDataTotalizado.Last().y)
        //         maxDataValueTotalizado = (int)(subSerieDataTotalizado.Last().y + 1);
        // }

        SetSubSerieData(SerieDataGasto, subSerieDataGasto, ref maxDataValueGasto);
        SetSubSerieData(SerieDataPresion, subSerieDataPresion, ref maxDataValuePresion);
        SetSubSerieData(SerieDataTotalizado, subSerieDataTotalizado, ref maxDataValueTotalizado);
        SetSubSerieData(SerieDataNivel, subSerieDataNivel, ref maxDataValueNivel);
        
        SetSubSerieDataBomba(SerieDataBomba, subSerieDataBomba, ref maxDataValueBomba);
        
        if (ButtonGraficGasto != null) ButtonGraficGasto.SetActive(subSerieDataGasto.Count()>0);
        if (ButtonGraficPresion != null) ButtonGraficPresion.SetActive(subSerieDataPresion.Count()>0);
        if (ButtonGraficTotalizado != null) ButtonGraficTotalizado.SetActive(subSerieDataTotalizado.Count()>0);
        if (ButtonGraficBomba != null) ButtonGraficBomba.SetActive(subSerieDataBomba.Count()>0);
        if (ButtonGraficNivel != null) ButtonGraficNivel.SetActive(subSerieDataNivel.Count()>0);

        if (subSerieDataGasto.Count() == 0) DeactivateGrafica(TipoDatosGrafico.GASTO);
        if (subSerieDataPresion.Count() == 0) DeactivateGrafica(TipoDatosGrafico.PRESION);
        if (subSerieDataTotalizado.Count() == 0) DeactivateGrafica(TipoDatosGrafico.TOTALIZADO);
        if (subSerieDataBomba.Count() == 0) DeactivateGrafica(TipoDatosGrafico.BOMBA);
        if (subSerieDataNivel.Count() == 0) DeactivateGrafica(TipoDatosGrafico.NIVEL);

    }

    public void SetSubSerieData(List<Reporte> serieData, List<Vector2> subSerieData, ref int maxData)
    {
        for (int i = 0; i < serieData.Count; i++)
        {
            subSerieData.Add(new Vector2(i, (serieData[i].Valor >= 0? serieData[i].Valor : 0)));

            if (maxData < subSerieData.Last().y)
                maxData = (int)(subSerieData.Last().y + 1);
        }
    }
    
    public void SetSubSerieDataBomba(List<Reporte> serieData, List<Vector2> subSerieData, ref int maxData)
    {
        for (int i = 0; i < serieData.Count; i++)
        {
            subSerieData.Add(new Vector2(i, (serieData[i].Valor >= 0? Mathf.RoundToInt(serieData[i].Valor) : 0)));

            if (maxData < subSerieData.Last().y)
                maxData = (int)(subSerieData.Last().y + 1);
        }
    }
    
    public void ActualizarDataFechas()
    {
        if (datePicker.SelectedDates.Count == 1)
        {
            setFechas(datePicker.SelectedDates[0], datePicker.SelectedDates[0]);
            
            if (RequestAPI.Instance.sistema == RequestAPI.Proyectos.PozosPAI)
            {
                RequestAPI.Instance.GetHistricosByDates(
                    idSitio,EstructuraSitio,
                    minDate,
                    maxDate,
                    tipoPromedio,
                    HistoricosCallBack);
            }
            else
            {
                RequestAPI.Instance.GetHistricosByDates(
                    idSitio,
                    minDate,
                    maxDate,
                    tipoPromedio,
                    HistoricosCallBack);
            }
        }
        else if (datePicker.SelectedDates.Count == 2)
        {
            setFechas(datePicker.SelectedDates[0], datePicker.SelectedDates[1]);
            
            if (RequestAPI.Instance.sistema == RequestAPI.Proyectos.PozosPAI)
            {
                RequestAPI.Instance.GetHistricosByDates(
                    idSitio,EstructuraSitio,
                    minDate,
                    maxDate,
                    tipoPromedio,
                    HistoricosCallBack);
            }
            else
            {
                RequestAPI.Instance.GetHistricosByDates(
                    idSitio,
                    minDate,
                    maxDate,
                    tipoPromedio,
                    HistoricosCallBack);
            }
        }

        ChangeVisibleCalendar();
    }

    // public void ReadTotalizados()
    // {
    //     if (RequestAPI.Instance != null)
    //     {
    //         SerieData.Clear();
    //
    //         for (int i = 0; i < RequestAPI.Instance.totalizadosPorFecha.ListaTotalizadoPorSitio.Count; i++)
    //         {
    //             SerieData.Add(RequestAPI.Instance.totalizadosPorFecha.ListaTotalizadoPorSitio[i]);
    //         }
    //         
    //         SerieData = SerieData.OrderByDescending(x => x.Diferencia).ToList();
    //
    //         UpdateSerieData();
    //     }
    // }
    
    public void InitPanelsActuallizacion()
    {
        axisGraph.gameObject.SetActive(false);
        SetActivePanelActualizando(true);
        SetActivePanelNoDatos(false);
    }

    public void SetActivePanelActualizando(bool _active)
    {
        panelActualizando.SetActive(_active);
    }
    
    public void SetActivePanelNoDatos(bool _active)
    {
        panelNoDatos.SetActive(_active);
    }
    
    public void HistoricosCallBack()
    {
        if (RequestAPI.Instance != null)
        {
            SerieDataGasto = RequestAPI.Instance.dataRequestAPI.historicosBySitio.Gasto;
            SerieDataPresion = RequestAPI.Instance.dataRequestAPI.historicosBySitio.Presion;
            SerieDataTotalizado = RequestAPI.Instance.dataRequestAPI.historicosBySitio.Totalizado;
            SerieDataBomba = RequestAPI.Instance.dataRequestAPI.historicosBySitio.Bomba;
            SerieDataNivel = RequestAPI.Instance.dataRequestAPI.historicosBySitio.Nivel;
            
            UpdateSerieData();
            
            if (SerieDataGasto.Count > 0 ||
                SerieDataPresion.Count > 0 ||
                SerieDataTotalizado.Count > 0 ||
                SerieDataBomba.Count > 0  ||
                SerieDataNivel.Count > 0 )
            {
                SetActivePanelActualizando(false);
                SetActivePanelNoDatos(false);
                axisGraph.gameObject.SetActive(true);
            }
            else
            {
                SetActivePanelNoDatos(true);
                SetActivePanelActualizando(false);
                axisGraph.gameObject.SetActive(false);
            }
        }
    }

    public void ActivateGrafica(TipoDatosGrafico _grafico)
    {
        if (graficoIzquierdo == TipoDatosGrafico.NONE)
        {
            graficoIzquierdo = _grafico;
        }
        else if (graficoIzquierdo == _grafico)
        {
            graficoIzquierdo = graficoDerecho;
            graficoDerecho = TipoDatosGrafico.NONE;
        }
        else
        {
            if (graficoDerecho == TipoDatosGrafico.NONE)
            {
                graficoDerecho = _grafico;
            }
            else if (graficoDerecho == _grafico)
            {
                graficoDerecho = TipoDatosGrafico.NONE;
            }
            else
            {
                graficoIzquierdo = graficoDerecho;
                graficoDerecho = _grafico;
            }
        }
    }
    
    public void DeactivateGrafica(TipoDatosGrafico _grafico)
    {
        if (graficoIzquierdo == _grafico)
        {
            graficoIzquierdo = graficoDerecho;
            graficoDerecho = TipoDatosGrafico.NONE;
        }
        else if (graficoDerecho == _grafico)
        {
            graficoDerecho = TipoDatosGrafico.NONE;
        }
    }

    public void ChangeActiveTotalizados()
    {
        ActivateGrafica(TipoDatosGrafico.TOTALIZADO);
        updateSeries();
    }
    
    public void ChangeActiveGasto()
    {
        ActivateGrafica(TipoDatosGrafico.GASTO);
        updateSeries();
    }
    
    public void ChangeActivePresion()
    {
        ActivateGrafica(TipoDatosGrafico.PRESION);
        updateSeries();
    }
    
    public void ChangeActiveBomba()
    {
        ActivateGrafica(TipoDatosGrafico.BOMBA);
        updateSeries();
    }
    
    public void ChangeActiveNivel()
    {
        ActivateGrafica(TipoDatosGrafico.NIVEL);
        updateSeries();
    }

    public void setToggleHora(bool isOn)
    {
        if (isOn)
            tipoPromedio = RequestBoy.TipoPromedio.HORAS;
    }
    
    public void setToggleDia(bool isOn)
    {
        if (isOn)
            tipoPromedio = RequestBoy.TipoPromedio.DIAS;
    }
    
    public void setToggleSemana(bool isOn)
    {
        if (isOn)
            tipoPromedio = RequestBoy.TipoPromedio.MES;
    }

    [Button]
    public void ChangeVisibleCalendar()
    {
        if (UI_Calendar != null)
        {
            bool activeCalendar = !UI_Calendar.activeSelf;

            UI_Calendar.SetActive(activeCalendar);
            if (UI_Graph) UI_Graph.SetActive(!activeCalendar);
        }
    }
    
    public void updateDateCliked()
    {
        Debug.Log("DateClic");

        if (datePicker.SelectedDates.Count == 1)
        {
            setFechasCalendario(datePicker.SelectedDates[0], datePicker.SelectedDates[0]);
        }
        else if (datePicker.SelectedDates.Count == 2)
        {
            setFechasCalendario(datePicker.SelectedDates[0], datePicker.SelectedDates[1]);
        }
    }
    
    public void setFechasCalendario(SerializableDate _date01, SerializableDate _date02)
    {
        date01 = _date01;
        date02 = _date02;
        
        DateTime minCalDate;
        DateTime maxCalDate;

        if (date01.Date < date02.Date)
        {
            minCalDate = date01.Date;
            maxCalDate = date02.Date.AddDays(1);
        }
        else
        {
            minCalDate = date02.Date;
            maxCalDate = date01.Date.AddDays(1);
        }

        if (UI_Calendar_FechaMin != null)
            UI_Calendar_FechaMin.text = GetDateString(minCalDate);
        
        if (UI_Calendar_FechaMax != null)
            UI_Calendar_FechaMax.text = GetDateString(maxCalDate.AddSeconds(-1));
    }

    public void ChangeVisibleLabelsEjeX()
    {
        if (axisGraph != null)
            axisGraph.xAxis.hideLabels = !axisGraph.xAxis.hideLabels;
    }
}
