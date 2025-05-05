using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBoyGraph : Singleton<ControlBoyGraph>
{
    public List<BoyGraph> ListGraph;

    private void Start()
    {
        if (ControlUpdateUI._singletonExists)
            ControlUpdateUI.singleton.SitioSeleccionado.AddListener(UpdateInfoSitio);
    }

    public void UpdateInfoSitio(DataSitio _DataSitio)
    {
        if (ListGraph.Count > 0)
        {
            foreach (var graph in ListGraph)
            {
                graph.InitPanelsActuallizacion();
            }

            if (RequestAPI.Instance.sistema == RequestAPI.Proyectos.PozosPAI)
            {
                BoyGraph.idSitio = _DataSitio.idSitio % 100;
                BoyGraph.EstructuraSitio = (int)_DataSitio.Estructura;
            
                RequestAPI.Instance.GetHistricosByDates(
                    BoyGraph.idSitio,(int)_DataSitio.Estructura,
                    BoyGraph.minDate,
                    BoyGraph.maxDate,
                    BoyGraph.tipoPromedio,
                    HistoricosCallBack);
            }
            else
            {
                BoyGraph.idSitio = _DataSitio.idSitio;
                BoyGraph.EstructuraSitio = (int)_DataSitio.Estructura;
            
                RequestAPI.Instance.GetHistricosByDates(
                    BoyGraph.idSitio,
                    BoyGraph.minDate,
                    BoyGraph.maxDate,
                    BoyGraph.tipoPromedio,
                    HistoricosCallBack);
            }
        }
    }

    public void HistoricosCallBack()
    {
        foreach (var graph in ListGraph)
        {
            graph.HistoricosCallBack();
        }
    }
}
