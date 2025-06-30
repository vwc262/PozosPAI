using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBoyGraph : Singleton<ControlBoyGraph>
{
    public List<BoyGraph> ListGraph;

    private void Start()
    {
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);
    }

    public void UpdateInfoSitio(ControlSitio sitio)
    {
        if (sitio != null)
        {
            if (ListGraph.Count > 0)
            {
                foreach (var graph in ListGraph)
                {
                    graph.InitPanelsActuallizacion();
                }

                if (RequestAPI.singleton.sistema == EstructurasAPI.Proyectos.PozosPAI)
                {
                    BoyGraph.idSitio = sitio.dataSitio.idSitio % 100;
                    BoyGraph.EstructuraSitio = (int)sitio.dataSitio.Estructura;

                    RequestAPI_Historicos.singleton.GetHistricosByDates(
                        BoyGraph.idSitio, (int)sitio.dataSitio.Estructura,
                        BoyGraph.minDate,
                        BoyGraph.maxDate,
                        BoyGraph.tipoPromedio,
                        HistoricosCallBack);
                }
                else
                {
                    BoyGraph.idSitio = sitio.dataSitio.idSitio;
                    BoyGraph.EstructuraSitio = (int)sitio.dataSitio.Estructura;

                    RequestAPI_Historicos.singleton.GetHistricosByDates(
                        BoyGraph.idSitio,
                        BoyGraph.minDate,
                        BoyGraph.maxDate,
                        BoyGraph.tipoPromedio,
                        HistoricosCallBack);
                }
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
