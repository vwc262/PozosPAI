using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

//#if UNITY_EDITOR
public class ControlDatos : Singleton<ControlDatos>
{
    //[TabGroup("Sitios")] public RequestAPI requestAPI;
    [TabGroup("Sitios")] public float updateDataTime = 10;
    [TabGroup("Sitios")] public bool UpdateLoop = true;

    [SerializeField][TabGroup("Sitios")]
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "smallDescription")]
    public List<ControlSitio> listSitios = new List<ControlSitio>();
    //[TabGroup("Sitios")] public List<int> listIdRegiones = new List<int>();

    //[TabGroup("Sitios")] public int regiones;

    //[TabGroup("Marcadores")]public List<GameObject> listMarcadoresSitios = new List<GameObject>();

    [TabGroup("Marcadores")]public float longitud0;
    [TabGroup("Marcadores")]public float latitud0;
    
    [TabGroup("Marcadores")]public float spanLongitud;
    [TabGroup("Marcadores")]public float spanLatitud;
    
    [TabGroup("Marcadores")]public float maxAltitude;
    [TabGroup("Marcadores")]public float alturaMarcador;

    [TabGroup("Marcadores")]public Vector3 position;

    [TabGroup("Marcadores")]public LayerMask groundedLayerMaskayer;
    //[TabGroup("Marcadores")]public ControlSitioUI controlSitioUI;
    
    [TabGroup("Overlap")]public bool useOverlapingDesp = true;
    [TabGroup("Overlap")]public float overlapMoveDistance = 100;
    [TabGroup("Overlap")]public float overlapingDistance = 1000;
    [TabGroup("Overlap")]public int overlapingSteps = 1000;
    [TabGroup("Overlap")]public bool finishOverlap = false;
    
    [TabGroup("Totalizados")] public UDateTime totalizadosTime1;
    [TabGroup("Totalizados")] public UDateTime totalizadosTime2;
    [TabGroup("Totalizados")] public List<TotalizadoPorSitio> totalizadosPorFecha;
    
    [TabGroup("Sitios")] public UnityEvent DatosInicializados;
    [TabGroup("Sitios")] public Coroutine UpdateDataCoroutine;
    [TabGroup("Sitios")] public Coroutine ActualizarInfraestructuraCoroutine;
    
    [TabGroup("Mapa")] public float maxLatitud;
    [TabGroup("Mapa")] public float minLatitud;
    [TabGroup("Mapa")] public float maxLongitud;
    [TabGroup("Mapa")] public float minLongitud;
    
    [TabGroup("Regiones")] public List<Region> regiones;
    
    public virtual void Start()
    {
    }

    public void IniciarUpdateData()
    {
        //Coroutine update data
        if (UpdateDataCoroutine != null) StopCoroutine(UpdateDataCoroutine);
        UpdateDataCoroutine = StartCoroutine(UpdateData());
    }

    [Button]
    public void ActualizarInfraestructura()
    {
        // if (ControlSitiosUI_Lista._singletonExists)
        //     ControlSitiosUI_Lista.singleton.sitiosOrdenados.clearListasRegiones();
        //
        // DeleteSitiosGPS();

        InitDataPozos();
    }

    public void InfraestructuraActualizada()
    {
        DatosInicializados.Invoke();
        
        Canvas.ForceUpdateCanvases();
    }

    public void CreateMaracadoresSitioMap()
    {
        CreateMarcadoresSitios_GO();
        
        GetOriginalPos();

        if (useOverlapingDesp)
            RecalculateOverlaping();
    }

    public void ClearListUIRegiones()
    {
        if (ControlSitiosUI_Lista._singletonExists)
            ControlSitiosUI_Lista.singleton.sitiosOrdenados.InitListasUIRegiones();
    }

    public void CreateListUISitios()
    {
        ReCreateSitiosUI_GO();
        
        if (ControlSitiosUI_Lista._singletonExists)
            ControlSitiosUI_Lista.singleton.sitiosOrdenados.updateListSitios();
    }

    public void RecreateUIListSitios()
    {
        StartCoroutine(RecreateUISitiosCoroutine());
    }

    public IEnumerator RecreateUISitiosCoroutine()
    {
        ClearListUIRegiones();
        
        yield return new WaitForSeconds(0.1f);

        CreateListUISitios();
    }

    [Button]
    [TabGroup("Sitios")]public virtual void InitDataPozos()
    {
        listSitios.Clear();

        var cont = 0;

        for (int i = 0; i < ControlRequest.singleton.listRequestAPI.Count; i++)
        {
            foreach (SiteDescription sitio in ControlRequest.singleton.listRequestAPI[i].dataRequestAPI.infraestructura.Sites.
                         OrderByDescending(x=>x.Latitud))
            {
                ControlSitio newSitio = new ControlSitio();
                newSitio.dataSitio = GetDataSitioFromSiteDescription(sitio, i);
                newSitio.dataSitio.idSitioUnity = cont++;
                newSitio.indexRequestAPI = i;

                listSitios.Add(newSitio);
            }
        }
        
        // listIdRegiones.Clear();
        //
        // foreach (var sitio in listSitios.DistinctBy(item => item.dataSitio.Estructura))
        // {
        //     listIdRegiones.Add(sitio.dataSitio.Estructura);
        // }
        //
        // regiones = listIdRegiones.Count();
    }

    public int GetIndexRegionByID(int idRegion)
    {
        return regiones.FindIndex(Item => Item.idRegion == idRegion);
    }

    public int GetIDRegionByIndex(int index)
    {
        if (index < regiones.Count)
            return regiones[index].idRegion;

        return 0;
    }
    
    [TabGroup("Marcadores")]
    public void DeleteSitiosGPS()
    {
        int i = 0;
        foreach (var sitio in listSitios)
        {
            print($"Deleting {i++} {sitio}");
            
            if (Application.isEditor)
            {
                DestroyImmediate(sitio.controlMarcadorMap.gameObject);
            }
            else
            {
                Destroy(sitio.controlMarcadorMap.gameObject);
            }
        }
    }
    
    public virtual void CreateMarcadoresSitios_GO()
    {
        var cont = 1;

        foreach (var sitio in listSitios)
        {
            position = this.transform.position + Gps2UnityConverter.GPS2Unity(sitio.dataSitio.latitud, sitio.dataSitio.longitud);

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(position, transform.TransformDirection(Vector3.down), out hit, maxAltitude,
                    groundedLayerMaskayer))
            {
                position.y = hit.point.y + alturaMarcador;
            }
            
            GameObject instancePrefab = ControlPrefabs.singleton.GetPrefabMarcadorSitio(sitio.dataSitio.tipoSitio);

            if (instancePrefab != null)
            {
                GameObject instance;

                if (ControlMap._singletonExists && ControlMap.singleton.contenedorMarcadores != null)
                    instance = Instantiate(instancePrefab, ControlMap.singleton.contenedorMarcadores.transform);
                else
                    instance = Instantiate(instancePrefab, this.transform);

                instance.transform.position = position;
                instance.name = $"Sitio_{sitio.dataSitio.nombre}_{sitio.dataSitio.Estructura}";

                ControlMarcadorSitio myControlMarcadorSitio = instance.GetComponent<ControlMarcadorSitio>();
                if (myControlMarcadorSitio != null)
                {
                    myControlMarcadorSitio.SetDataSitio(sitio);
                }

                if (sitio.controlMarcadorMap != null)
                    Destroy(sitio.controlMarcadorMap.gameObject);

                sitio.controlMarcadorMap = myControlMarcadorSitio;
            }
        }
    }
    
    [Button][GUIColor(1,0.5f,0.5f)]
    [TabGroup("Marcadores")]
    public void ReCreateSitiosUI_GO()
    {
        if (ControlSitiosUI_Lista._singletonExists)
        {
            foreach (var sitio in listSitios)
            {
                ControlSitiosUI_Lista.singleton.SetSitioSelectUI_GO(sitio);
            }

            ControlSitiosUI_Lista.singleton.SetSitiosUIEnd();
        }
    }
    
    [TabGroup("Marcadores")]
    [Button]
    public void DeleteSitiosSelectUI()
    {
        if (ControlSitiosUI_Lista._singletonExists)
            ControlSitiosUI_Lista.singleton.DeleteSitios();
    }
    
    IEnumerator UpdateData()
    {
        while (UpdateLoop)
        {
            UpdateDataPozos();
            yield return new WaitForSeconds(updateDataTime);
        }
    }
    
    public virtual DataSitio GetDataSitioFromSiteDescription(SiteDescription sitio, int indexRequest)
    {
        DataSitio newDataSitio = new DataSitio();
        
        newDataSitio.idSitio = sitio.Id;
        newDataSitio.nombre = sitio.Nombre;
        newDataSitio.abreviacion = sitio.Abreviacion;
        newDataSitio.fecha = sitio.Tiempo;
        newDataSitio.voltaje = sitio.Voltaje;
        newDataSitio.Estructura = sitio.Grupo;
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
    
    [Button][GUIColor(0.25f,0.25f,1)]
    [TabGroup("Sitios")]
    public virtual void UpdateDataPozos()
    {
        var cont = 0;
        foreach (ControlSitio controlSitio in listSitios)
        {
            //DataSitio newDataSitio = new DataSitio();
            SiteDescription sitio = ControlRequest.singleton.listRequestAPI[controlSitio.indexRequestAPI].
                dataRequestAPI.infraestructura.Sites.Find(
                    item => item.Id == controlSitio.dataSitio.idSitio);

            if (sitio != null)
            {
                controlSitio.dataSitio.SetDataSitio(GetDataSitioFromSiteDescription(sitio, controlSitio.indexRequestAPI));
                controlSitio.dataSitio.idSitioUnity = cont;
                controlSitio.GetStatusConexionSitio();
            }

            cont++;
        }
    }
    
    private void ReadTotalizados(int indexRequest)
    {
        totalizadosPorFecha = ControlRequest.singleton.listRequestAPI[indexRequest].dataRequestAPI.totalizadosPorFecha.ListaTotalizadoPorSitio;

        totalizadosPorFecha = totalizadosPorFecha.OrderByDescending(
            x => x.Diferencia).ToList();
    }

    [TabGroup("Overlap")]
    public Vector3[] originalPos;

    [TabGroup("Overlap")]
    [Button]
    [GUIColor(1, 0, 1)]
    private void GetOriginalPos()
    {
        originalPos = new Vector3[listSitios.Count];
        for (int i = 0; i < listSitios.Count; i++)
        {
            var billboard = listSitios[i].controlMarcadorMap.rootOverlaping;
            originalPos[i] = billboard.transform.localPosition;
            listSitios[i].controlMarcadorMap.billboardObj.positionGPSOriginal = originalPos[i];
        }
    }
    
    [TabGroup("Overlap")]
    [Button]
    [GUIColor(1, 0, 0.5f)]
    private void RevertToOriginalPos()
    {
        for (int i = 0; i < listSitios.Count; i++)
        {
            var sitio1 = listSitios[i].controlMarcadorMap.rootOverlaping;
            sitio1.transform.localPosition = originalPos[i];
        }
    }
    
    [TabGroup("Overlap")]
    [Button]
    [GUIColor(1,1,0)]
    private void RecalculateOverlaping()
    {
        finishOverlap = false;
        var contSteps = 0;
        while(!finishOverlap)
        {
            finishOverlap = true;
            for (int i = 0; i < listSitios.Count; i++)
            {
                var sitio1 = listSitios[i].controlMarcadorMap.rootOverlaping;

                for (int j = 0; j < listSitios.Count; j++)
                {
                    var sitio2 = listSitios[j].controlMarcadorMap.rootOverlaping;
                    if (sitio1 != sitio2)
                        if (Vector3.Distance(sitio1.transform.position.with(y:0), sitio2.transform.position.with(y:0)) < overlapingDistance)
                        {
                            var dir = (sitio1.transform.position - sitio2.transform.position).normalized;
                            dir.y = 0;
                            sitio1.transform.Translate(dir * overlapMoveDistance, Space.World);
                            listSitios[i].controlMarcadorMap.billboardObj.positionFinalMarcador = sitio1.transform.localPosition;
                            finishOverlap = false;
                        }
                }

                contSteps++;
                if (contSteps > overlapingSteps) finishOverlap = true;
            }
        }
    }
    
    public string GetNameRegionByIndex(int index, int indexRequest)
    {
        return GetNameRegionByID(GetIDRegionByIndex(index), indexRequest);
    }
    
    public string GetNameRegionByID(int idRegion, int indexRequest)
    {
        Region regionAux = regiones.Find(item => item.idRegion == idRegion);
        if (regionAux != null)    
            return regionAux.nombre;

        return "Region " + (idRegion);
    }

    [Button]
    public void RecalcualtePositionMarcadores()
    {
        Gps2UnityConverter.longitud0 = longitud0;
        Gps2UnityConverter.latitud0 = latitud0;
        Gps2UnityConverter.spanLongitud = spanLongitud;
        Gps2UnityConverter.spanLatitud = spanLatitud;

        foreach (var instance in listSitios)
        {
            position = instance.controlMarcadorMap.transform.position + 
                       Gps2UnityConverter.GPS2Unity(instance.dataSitio.latitud, instance.dataSitio.longitud);
            
            position.y = instance.controlMarcadorMap.transform.position.y;
            instance.controlMarcadorMap.transform.position = position;
        }
    }

    public virtual void SetGlobalDataSitios()
    {
        if (listSitios.Count > 0)
        {
            maxLongitud = listSitios.Max(item => item.dataSitio.longitud);
            minLongitud = listSitios.Min(item => item.dataSitio.longitud);

            maxLatitud = listSitios.Max(item => item.dataSitio.latitud);
            minLatitud = listSitios.Min(item => item.dataSitio.latitud);
        }
    }

    public virtual int GetContRegiones()
    {
        return regiones.Count;
    }
}

[Serializable]
public class Region
{
    public string nombre;
    public int idRegion;
}