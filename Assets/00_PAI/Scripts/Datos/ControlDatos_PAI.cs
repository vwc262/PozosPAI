using System.Collections.Generic;
using System.Linq;
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
    }

    [Button][GUIColor(0.25f,0.25f,1)]
    [TabGroup("Sitios")]
    public override void UpdateDataPozos()
    {
        var cont = 0;
        
        foreach (ControlSitio controlSitio in listSitios)
        {
            SiteDescription sitio = ControlRequest.singleton.listRequestAPI[controlSitio.indexRequestAPI].
                dataRequestAPI.infraestructura.Sites.Find(
                item => item.Id == controlSitio.dataSitio.idSitio);

            if (sitio != null)
            {
                controlSitio.dataSitio.nombre = sitio.Nombre;
                controlSitio.dataSitio.abreviacion = sitio.Abreviacion;
                controlSitio.dataSitio.fecha = sitio.Tiempo;
                controlSitio.dataSitio.voltaje = sitio.Voltaje;
                //controlSitio.dataSitio.Estructura = sitio.Grupo;
                controlSitio.dataSitio.tipoSitio = (TipoSitio)sitio.TipoEstacion;
                controlSitio.dataSitio.longitud = sitio.Longitud;
                controlSitio.dataSitio.latitud = sitio.Latitud;
                
                //controlSitio.dataSitio.SetDataSitio(GetDataSitioFromSiteDescription(sitio));
                SiteBase siteBaseUpdate = ControlRequest.singleton.listRequestAPI[controlSitio.indexRequestAPI].
                    dataRequestAPI.updateUnitySites.Sites.Find(
                    item => item.Id == sitio.Id);

                if (siteBaseUpdate != null)
                {
                    controlSitio.dataSitio.fecha = siteBaseUpdate.Tiempo;
                    controlSitio.dataSitio.enlace = siteBaseUpdate.Enlace;
                    controlSitio.dataSitio.fallaAC = siteBaseUpdate.FallaAC;

                    foreach (var signalUpdate in siteBaseUpdate.SignalsContainer)
                    {
                        if (controlSitio.dataSitio.GetSignalExist((SignalBase.TipoSignalEnum)signalUpdate.TipoSignal))
                        {
                            List<SignalBase> signal =
                            controlSitio.dataSitio.GetSignal((SignalBase.TipoSignalEnum)signalUpdate.TipoSignal);
                            signal.Clear();
                            signal.AddRange(signalUpdate.Signals);
                        }
                        else
                        {
                            Signal signal = new Signal();
                            signal.tipoSignal = (SignalBase.TipoSignalEnum)signalUpdate.TipoSignal;
                            signal.signals.AddRange(signalUpdate.Signals); 
                
                            controlSitio.dataSitio.listSignals.Add(signal);
                        }
                    }
                }

                controlSitio.GetStatusConexionSitio();
                
                controlSitio.dataSitio.idSitioUnity = cont;
            }

            cont++;
        }

        UpdateDataAutomatismo();
    }

    public void UpdateDataAutomatismo()
    {
        if (RequestAPI_Auto._singletonExists && ControlRequest._singletonExists)
        {
            if (ControlRequest.singleton.listRequestAPI[0].sistema == EstructurasAPI.Proyectos.PozosPAI)
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
                        sitio.dataSitio.automationData.AutomationError =
                            estacion.BanderaArranqueFallido == 1 ? true : false;
                        sitio.dataSitio.automationData.nominalVoltage = estacion.VNominal;
                        sitio.dataSitio.automationData.version = estacion.Version;

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

                        sitio.dataSitio.automationData.ConfIsActiveAutomation =
                            estacion.Automatismo == 1 ? true : false;
                        sitio.dataSitio.automationData.ConfIndex = estacion.Secuencia;
                        sitio.dataSitio.automationData.ConfNominalVoltage = estacion.VNominal;
                        sitio.dataSitio.automationData.ConfVersion = estacion.Version;

                        if (segmento != null)
                        {
                            //sitio.dataSitio.automationData.ConfToleranceVoltage = segmento.Tolerancia;
                            sitio.dataSitio.automationData.ConfStarupTime = segmento.T1;
                            sitio.dataSitio.automationData.ConfWindowTime = segmento.T2;
                        }
                    }
                }
            }
        }
    }

    public bool habilitarBarrientos;
    
    [Button]
    [TabGroup("Sitios")]public override void InitDataPozos()
    {
        listSitios.Clear();

        var cont = 0;

        for (int i = 0; i < ControlRequest.singleton.listRequestAPI.Count; i++)
        {
            foreach (SiteDescription sitio in ControlRequest.singleton.listRequestAPI[i].dataRequestAPI.infraestructura.Sites.
                         OrderByDescending(x=>x.Latitud))
            {
                if (!(sitio.Id == 1421)) //Diferente de Barrientos
                {
                    ControlSitio newSitio = new ControlSitio();
                    newSitio.dataSitio = GetDataSitioFromSiteDescription(sitio, i);
                    newSitio.dataSitio.idSitioUnity = cont++;
                    newSitio.indexRequestAPI = i;

                    listSitios.Add(newSitio);
                }
                else
                {
                    if (habilitarBarrientos)
                    {
                        ControlSitio newSitio = new ControlSitio();
                        newSitio.dataSitio = GetDataSitioFromSiteDescription(sitio, i);
                        newSitio.dataSitio.idSitioUnity = cont++;
                        newSitio.indexRequestAPI = i;

                        listSitios.Add(newSitio);
                    }
                }
            }
        }
    }
    
    public override DataSitio GetDataSitioFromSiteDescription(SiteDescription sitio, int indexRequest)
    {
        DataSitio newDataSitio = new DataSitio();
        
        newDataSitio.idSitio = sitio.Id;
        newDataSitio.nombre = sitio.Nombre;
        newDataSitio.abreviacion = sitio.Abreviacion;
        newDataSitio.fecha = sitio.Tiempo;
        newDataSitio.voltaje = sitio.Voltaje;
        
        if (!(sitio.Id == 1421)) //Diferente de Barrientos
            newDataSitio.Estructura = sitio.Grupo;
        else
            newDataSitio.Estructura = 0;
        
        newDataSitio.tipoSitio = (TipoSitio)sitio.TipoEstacion;
        
        newDataSitio.longitud = sitio.Longitud;
        newDataSitio.latitud = sitio.Latitud;

        SiteBase sitebase = ControlRequest.singleton.listRequestAPI[indexRequest].dataRequestAPI.updateUnitySites.Sites.Find(
            item => item.Id == sitio.Id);

        if (sitebase != null)
        {
            newDataSitio.fecha = sitebase.Tiempo;
            newDataSitio.enlace = sitebase.Enlace;
            newDataSitio.fallaAC = sitebase.FallaAC;

            foreach (var signalAux in sitebase.SignalsContainer)
            {
                Signal signal = new Signal();
                signal.tipoSignal = (SignalBase.TipoSignalEnum)signalAux.TipoSignal;
                signal.signals.AddRange(signalAux.Signals); 
                
                newDataSitio.listSignals.Add(signal);
            }
        }

        return newDataSitio;
    }
}
