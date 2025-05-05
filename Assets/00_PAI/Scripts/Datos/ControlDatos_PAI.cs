using System.Windows.Forms.VisualStyles;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class ControlDatos_PAI : ControlDatosAux
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
        UpdateDataSitios_Marcadores();
    }

    [Button][GUIColor(0.25f,0.25f,1)]
    [TabGroup("Sitios")]
    public override void UpdateDataPozos()
    {
        var cont = 0;
        
        foreach (DataSitio dataSitio in listSitios)
        {
            SiteDescription sitio = requestAPI.dataRequestAPI.infraestructura.Sites.Find(
                item => item.Id == dataSitio.idSitio);

            if (sitio != null)
            {
                dataSitio.SetDataSitio(GetDataSitioFromSiteDescription(sitio));
                dataSitio.idSitioUnity = cont;
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
                    DataSitio sitio = listSitios.Find(item =>
                        item.idSitio == (estacion.IdEstacion + (100 * (int)sistema.sistema)));

                    SegmentoAutomatismo segmento =
                        RequestAPI_Auto.singleton.segmentosAutomatismo.Segmentos.Find(item =>
                            item.ID == estacion.IdSegmento);

                    if (sitio.automationData == null)
                        sitio.automationData = new Automation();

                    sitio.automationData.isActiveAutomation = estacion.Automatismo == 1 ? true : false;
                    sitio.automationData.index = estacion.Secuencia;
                    sitio.automationData.AutomationError = estacion.BanderaArranqueFallido == 1 ? true : false;
                    sitio.automationData.nominalVoltage = estacion.VNominal;

                    if (segmento != null)
                    {
                        sitio.automationData.idSubestacion = segmento.ID;
                        sitio.automationData.toleranceVoltage = segmento.Tolerancia;
                        sitio.automationData.starupTime = segmento.T1;
                        sitio.automationData.windowTime = segmento.T2;
                    }

                    if (ControlAutomation._singletonExists)
                        ControlAutomation.singleton.enableControlAutomatismo();
                }
            }

            foreach (var sistema in RequestAPI_Auto.singleton.ConfEstacionesSistemaAutomatismo)
            {
                foreach (var estacion in sistema.estacionesAutomatismo.EstacionAutomatismos)
                {
                    DataSitio sitio = listSitios.Find(item =>
                        item.idSitio == (estacion.IdEstacion + (100 * (int)sistema.sistema)));
                    
                    SegmentoAutomatismo segmento =
                        RequestAPI_Auto.singleton.ConfSegmentosAutomatismo.Segmentos.Find(item =>
                            item.ID == estacion.IdSegmento);
                    
                    if (sitio.automationData == null)
                        sitio.automationData = new Automation();
                    
                    sitio.automationData.ConfIsActiveAutomation = estacion.Automatismo == 1 ? true : false;
                    sitio.automationData.ConfIndex = estacion.Secuencia;
                    sitio.automationData.ConfNominalVoltage = estacion.VNominal;

                    if (segmento != null)
                    {
                        sitio.automationData.ConfToleranceVoltage = segmento.Tolerancia;
                        sitio.automationData.ConfStarupTime = segmento.T1;
                        sitio.automationData.ConfWindowTime = segmento.T2;
                    }
                }
            }
        }
    }
    
    public override void UpdateDataSitios_Marcadores()
    {
        foreach (var marcador in listMarcadoresSitios)
        {
            SitioGPS sitioGPS = marcador.GetComponent<SitioGPS>();

            if (sitioGPS != null)
            {
                DataSitio dataSitio = listSitios.Find(item => item.idSitio == sitioGPS.MyDataSitio.idSitio);

                if (dataSitio != null)
                {
                    sitioGPS.MyDataSitio.SetDataSitio(dataSitio);
                    
                    sitioGPS.MyDataSitio.automationData.SetDataAutomation(dataSitio.automationData);
                }
            }
        }
    }
}
