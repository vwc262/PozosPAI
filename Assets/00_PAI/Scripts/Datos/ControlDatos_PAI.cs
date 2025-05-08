using System.Windows.Forms.VisualStyles;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class ControlDatos_PAI : ControlDatos
{
    public override void Start()
    {
        base.Start();
        
        if (RequestAPI_Auto._singletonExists)
            RequestAPI_Auto.singleton.datosAutomatismoActualizados.AddListener(ForzarUpdateDatos);
    }

    public void ForzarUpdateDatos()
    {
        print("FORZAR UPDATE");
        UpdateDataPozos();
        //UpdateDataSitios_Marcadores();
    }

    [Button][GUIColor(0.25f,0.25f,1)]
    [TabGroup("Sitios")]
    public override void UpdateDataPozos()
    {
        var cont = 0;
        
        foreach (ControlSitio controlSitio in listSitios)
        {
            SiteDescription sitio = RequestAPI.Instance.dataRequestAPI.infraestructura.Sites.Find(
                item => item.Id == controlSitio.dataSitio.idSitio);

            if (sitio != null)
            {
                controlSitio.dataSitio.SetDataSitio(GetDataSitioFromSiteDescription(sitio));
                controlSitio.dataSitio.idSitioUnity = cont;
                controlSitio.GetStatusConexionSitio();
            }

            cont++;
        }

        UpdateDataAutomatismo();
    }

    public void UpdateDataAutomatismo()
    {
        if (RequestAPI_Auto._singletonExists)
        {
            foreach (var sistema in RequestAPI_Auto.singleton.estacionesSistemaAutomatismo)
            {
                foreach (var estacion in sistema.estacionesAutomatismo.EstacionAutomatismos)
                {
                    ControlSitio sitio = listSitios.Find(item =>
                        item.dataSitio.idSitio == (estacion.IdEstacion + (100 * (int)sistema.sistema)));

                    SegmentoAutomatismo segmento =
                        RequestAPI_Auto.singleton.segmentosAutomatismo.Segmentos.Find(item =>
                            item.ID == estacion.IdSegmento);

                    if (sitio.dataSitio.automationData == null)
                        sitio.dataSitio.automationData = new Automation();

                    sitio.dataSitio.automationData.isActiveAutomation = estacion.Automatismo == 1 ? true : false;
                    sitio.dataSitio.automationData.index = estacion.Secuencia;
                    sitio.dataSitio.automationData.AutomationError = estacion.BanderaArranqueFallido == 1 ? true : false;
                    sitio.dataSitio.automationData.nominalVoltage = estacion.VNominal;

                    if (segmento != null)
                    {
                        sitio.dataSitio.automationData.idSubestacion = segmento.ID;
                        sitio.dataSitio.automationData.toleranceVoltage = segmento.Tolerancia;
                        sitio.dataSitio.automationData.starupTime = segmento.T1;
                        sitio.dataSitio.automationData.windowTime = segmento.T2;
                    }

                    if (ControlAutomation._singletonExists)
                        ControlAutomation.singleton.enableControlAutomatismo();
                }
            }

            foreach (var sistema in RequestAPI_Auto.singleton.ConfEstacionesSistemaAutomatismo)
            {
                foreach (var estacion in sistema.estacionesAutomatismo.EstacionAutomatismos)
                {
                    ControlSitio sitio = listSitios.Find(item =>
                        item.dataSitio.idSitio == (estacion.IdEstacion + (100 * (int)sistema.sistema)));
                    
                    SegmentoAutomatismo segmento =
                        RequestAPI_Auto.singleton.ConfSegmentosAutomatismo.Segmentos.Find(item =>
                            item.ID == estacion.IdSegmento);
                    
                    if (sitio.dataSitio.automationData == null)
                        sitio.dataSitio.automationData = new Automation();
                    
                    sitio.dataSitio.automationData.ConfIsActiveAutomation = estacion.Automatismo == 1 ? true : false;
                    sitio.dataSitio.automationData.ConfIndex = estacion.Secuencia;
                    sitio.dataSitio.automationData.ConfNominalVoltage = estacion.VNominal;

                    if (segmento != null)
                    {
                        sitio.dataSitio.automationData.ConfToleranceVoltage = segmento.Tolerancia;
                        sitio.dataSitio.automationData.ConfStarupTime = segmento.T1;
                        sitio.dataSitio.automationData.ConfWindowTime = segmento.T2;
                    }
                }
            }
        }
    }
    
    // public override void UpdateDataSitios_Marcadores()
    // {
    //     foreach (var sitio in listSitios)
    //     {
    //         //SitioGPS sitioGPS = marcador.GetComponent<SitioGPS>();
    //
    //         if (sitio.controlMarcadorMap != null)
    //         {
    //             ControlSitio controlSitio = listSitios.Find(item => item.dataSitio.idSitio == sitio.dataSitio.idSitio);
    //
    //             if (controlSitio != null)
    //             {
    //                 sitio.dataSitio.SetDataSitio(controlSitio.dataSitio);
    //                 
    //                 sitio.dataSitio.automationData.SetDataAutomation(controlSitio.dataSitio.automationData);
    //             }
    //         }
    //     }
    // }
}
