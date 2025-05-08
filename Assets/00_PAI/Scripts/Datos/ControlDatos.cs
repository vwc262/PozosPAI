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
    [TabGroup("Sitios")] public List<int> listIdRegionales = new List<int>();

    [TabGroup("Sitios")] public int regionales;

    //[TabGroup("Marcadores")]public List<GameObject> listMarcadoresSitios = new List<GameObject>();

    [TabGroup("Marcadores")]public float longitud0;
    [TabGroup("Marcadores")]public float latitud0;
    
    [TabGroup("Marcadores")]public float spanLongitud;
    [TabGroup("Marcadores")]public float spanLatitud;
    
    [TabGroup("Marcadores")]public float maxAltitude;
    [TabGroup("Marcadores")]public float alturaMarcador;

    [TabGroup("Marcadores")]public Vector3 position;

    [TabGroup("Marcadores")]public LayerMask groundedLayerMaskayer;
    [TabGroup("Marcadores")]public ControlSitioUI controlSitioUI;
    
    [TabGroup("Prefabs")]public GameObject prefabMarcadorSitioLerma1;
    [TabGroup("Prefabs")]public GameObject prefabMarcadorSitioLerma2;
    [TabGroup("Prefabs")]public GameObject prefabMarcadorRepetidor;
    [TabGroup("Prefabs")]public GameObject prefabMarcadorSitioEnConstruccion;
    [TabGroup("Prefabs")]public GameObject prefabPanelSitioLerma1;
    [TabGroup("Prefabs")]public GameObject prefabPanelSitioLerma2;
    [TabGroup("Prefabs")]public GameObject prefabPanelRepetidor;
    [TabGroup("Prefabs")]public GameObject prefabPanelSitioEnConstruccion;
    [TabGroup("Prefabs")]public GameObject prefabUIRegionaLabel;
    [TabGroup("Prefabs")]public GameObject prefabUIRegionaList;
    
    [TabGroup("Overlap")]public bool useOverlapingDesp = true;
    [TabGroup("Overlap")]public float overlapMoveDistance = 100;
    [TabGroup("Overlap")]public float overlapingDistance = 1000;
    [TabGroup("Overlap")]public int overlapingSteps = 1000;
    [TabGroup("Overlap")]public bool finishOverlap = false;
    
    [TabGroup("Totalizados")] public UDateTime totalizadosTime1;
    [TabGroup("Totalizados")] public UDateTime totalizadosTime2;
    [TabGroup("Totalizados")] public List<TotalizadoPorSitio> totalizadosPorFecha;
    
    //public DisableTerrainMeshCollider colliderMapa;
    public SitiosOrdenados sitiosOrdenados;
    //public bool ActInfraestructuraCoroutine;

    public UnityEvent DatosInicializados;

    public Coroutine UpdateDataCoroutine;
    //public Coroutine UpdateDataGPSCoroutine;
    public Coroutine ActualizarInfraestructuraCoroutine;

    public virtual void Start()
    {
        // if (requestAPI != null)
        // {
        //     requestAPI.infraestructuraActualizada.AddListener(ActualizarInfraestructura);
        // }
    }

    public void IniciarUpdateData()
    {
        //Coroutine update data
        if (UpdateDataCoroutine != null) StopCoroutine(UpdateDataCoroutine);
        UpdateDataCoroutine = StartCoroutine(UpdateData());
        
        // if (UpdateDataGPSCoroutine != null) StopCoroutine(UpdateDataGPSCoroutine);
        // UpdateDataGPSCoroutine = StartCoroutine(UpdateDataSitiosGPS());
    }

    [Button]
    public void ActualizarInfraestructura()
    {
        if (sitiosOrdenados != null)
            sitiosOrdenados.clearListas();

        DeleteSitiosGPS();

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

    public void ClearListUISitios()
    {
        if (sitiosOrdenados != null)
            sitiosOrdenados.InitListasUI();
    }

    public void CreateListUISitios()
    {
        ReCreateSitiosUI_GO();
        
        if (sitiosOrdenados != null)
            sitiosOrdenados.updateListSitios();
    }

    public void RecreateUISitios()
    {
        StartCoroutine(RecreateUISitiosCoroutine());
    }

    public IEnumerator RecreateUISitiosCoroutine()
    {
        ClearListUISitios();
        
        yield return new WaitForSeconds(0.1f);

        CreateListUISitios();
    }

    [Button]
    [TabGroup("Sitios")]public void InitDataPozos()
    {
        listSitios.Clear();

        var cont = 0;
        
        foreach (SiteDescription sitio in RequestAPI.Instance.dataRequestAPI.infraestructura.Sites)
        {
            ControlSitio newSitio = new ControlSitio();
            newSitio.dataSitio = GetDataSitioFromSiteDescription(sitio);
            newSitio.dataSitio.idSitioUnity = cont++;

            listSitios.Add(newSitio);
        }

        listIdRegionales.Clear();
        
        foreach (var sitio in listSitios.DistinctBy(item => item.dataSitio.Estructura))
        {
            listIdRegionales.Add(sitio.dataSitio.Estructura);
        }

        regionales = listIdRegionales.Count();
    }

    public int GetIndexRegionByID(int idRegion)
    {
        return listIdRegionales.FindIndex(Item => Item == idRegion);
    }

    public int GetIDRegionByIndex(int index)
    {
        if (index < listIdRegionales.Count)
            return listIdRegionales[index];

        return 0;
    }

    [Button][GUIColor(1,0.25f,0.25f)]
    [TabGroup("Marcadores")]
    public void ReCreateAll()
    {
        ReCreateSitiosGPS();
        ReCreateSitiosUI_prefab();
    }
    
    [Button][GUIColor(1,0.5f,0.5f)]
    [TabGroup("Marcadores")]
    public void ReCreateSitiosGPS()
    {
        DeleteSitiosGPS();
        CreateSitiosGPS_prefab();
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
    
    [TabGroup("Marcadores")]
    public void CreateSitiosGPS_prefab()
    {
    #if UNITY_EDITOR
        var cont = 1;
        
        Gps2UnityConverter.longitud0 = longitud0;
        Gps2UnityConverter.latitud0 = latitud0;
        
        foreach (var sitio in listSitios)
        {
            // position = this.transform.position + 
            //            (Vector3.right * (sitio.longitud - longitud0)) * spanLongitud +
            //            (Vector3.forward * (sitio.latitud - latitud0)) * spanLatitud +
            //            (Vector3.up * maxAltitude);
            
            position = this.transform.position + Gps2UnityConverter.GPS2Unity(sitio.dataSitio.latitud, sitio.dataSitio.longitud);
            
            // Bit shift the index of the layer (3) to get a bit mask
            //int layerMask = -1;
            
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(position, transform.TransformDirection(Vector3.down), out hit, maxAltitude, groundedLayerMaskayer))
            {
                position.y = hit.point.y + alturaMarcador;
            }


            GameObject instancePrefab = null;
            switch (sitio.dataSitio.tipoSitioPozo)
            {
                case TipoSitioPozo.PozoLerma1:
                    instancePrefab = prefabMarcadorSitioLerma1;
                    break;
                case TipoSitioPozo.PozoLerma2:
                    instancePrefab = prefabMarcadorSitioLerma2;
                    break;
                case TipoSitioPozo.Repetidor:
                    instancePrefab = prefabMarcadorRepetidor;
                    break;
                case TipoSitioPozo.EnConstruccion:
                    instancePrefab = prefabMarcadorSitioEnConstruccion;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(instancePrefab);
            Object prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(Object));

            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, this.transform);
            instance.transform.position = position;
            
            instance.name = $"Sitio_{sitio.dataSitio.nombre}_{sitio.dataSitio.Estructura}";
            
            ControlMarcadorSitio myControlMarcadorSitio = instance.GetComponent<ControlMarcadorSitio>();
            if (myControlMarcadorSitio != null)
            {
                myControlMarcadorSitio.SetDataSitio(sitio.dataSitio);
                myControlMarcadorSitio.controlSitioUI = controlSitioUI;
            }

            //CreateGUISitios(mySitio);
            
            sitio.controlMarcadorMap = myControlMarcadorSitio;
            
            PrefabUtility.RecordPrefabInstancePropertyModifications(instance);
        }
    #endif
    }
    
    public void CreateMarcadoresSitios_GO()
    {
        var cont = 1;
        
        Gps2UnityConverter.longitud0 = longitud0;
        Gps2UnityConverter.latitud0 = latitud0;
        Gps2UnityConverter.spanLongitud = spanLongitud;
        Gps2UnityConverter.spanLatitud = spanLatitud;

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
            
            GameObject instancePrefab = null;
            switch (sitio.dataSitio.tipoSitioPozo)
            {
                case TipoSitioPozo.PozoLerma1:
                    instancePrefab = prefabMarcadorSitioLerma1;
                    break;
                case TipoSitioPozo.PozoLerma2:
                    instancePrefab = prefabMarcadorSitioLerma2;
                    break;
                case TipoSitioPozo.Repetidor:
                    instancePrefab = prefabMarcadorRepetidor;
                    break;
                case TipoSitioPozo.EnConstruccion:
                    instancePrefab = prefabMarcadorSitioEnConstruccion;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

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
                myControlMarcadorSitio.SetDataSitio(sitio.dataSitio);
                myControlMarcadorSitio.controlSitioUI = controlSitioUI;
            }

            sitio.controlMarcadorMap = myControlMarcadorSitio;
        }
    }
    
    [Button][GUIColor(1,0.5f,0.5f)]
    [TabGroup("Marcadores")]
    public void ReCreateSitiosUI_prefab()
    {
        DeleteSitiosSelectUI();

        foreach (var sitio in listSitios)
        {
            if (sitio.controlMarcadorMap != null) 
                CreateGUISitios(sitio);
        }
    }
    
    [Button][GUIColor(1,0.5f,0.5f)]
    [TabGroup("Marcadores")]
    public void ReCreateSitiosUI_GO()
    {
        DeleteSitiosSelectUI();

        foreach (var sitio in listSitios)
        {
            if (sitio.controlMarcadorMap != null)
            {
                controlSitioUI.SetSitioSelectUI_GO(sitio);
            }
        }
        
        controlSitioUI.SetSitiosEnd();
    }
    
    [TabGroup("Marcadores")]
    [Button]
    public void DeleteSitiosSelectUI()
    {
        controlSitioUI.DeleteSitios();
    }
    
    [TabGroup("Marcadores")]
    public void CreateGUISitios(ControlSitio sitio)
    {
        controlSitioUI.SetSitioSelectUI_Prefab(sitio);
    }
    
    [TabGroup("Marcadores")]
    [Button]
    [GUIColor(0.5f,1,1)]
    private void UnpackPrefabs()
    {
#if UNITY_EDITOR
        if (prefabMarcadorSitioLerma1 != null)
        {
            PrefabUtility.UnpackAllInstancesOfPrefab(prefabMarcadorSitioLerma1, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }
        if (prefabMarcadorSitioLerma2 != null)
        {
            PrefabUtility.UnpackAllInstancesOfPrefab(prefabMarcadorSitioLerma2, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }
        if (prefabMarcadorRepetidor != null)
        {
            PrefabUtility.UnpackAllInstancesOfPrefab(prefabMarcadorRepetidor, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }
        if (prefabMarcadorSitioEnConstruccion != null)
        {
            PrefabUtility.UnpackAllInstancesOfPrefab(prefabMarcadorSitioEnConstruccion, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }
        if (prefabPanelSitioLerma1 != null)
        {
            PrefabUtility.UnpackAllInstancesOfPrefab(prefabPanelSitioLerma1, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }
        if (prefabPanelSitioLerma2 != null)
        {
            PrefabUtility.UnpackAllInstancesOfPrefab(prefabPanelSitioLerma2, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }
        if (prefabPanelRepetidor != null)
        {
            PrefabUtility.UnpackAllInstancesOfPrefab(prefabPanelRepetidor, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }
        if (prefabPanelSitioEnConstruccion != null)
        {
            PrefabUtility.UnpackAllInstancesOfPrefab(prefabPanelSitioEnConstruccion, PrefabUnpackMode.OutermostRoot, InteractionMode.UserAction);
        }
        #endif
    }
    
    [TabGroup("Marcadores")]
    [Button]
    [GUIColor(0.1f,0.1f,1)]
    private void CreateAllOverlap()
    {
        ReCreateAll();
        GetOriginalPos();
        RecalculateOverlaping();
    }
    
    [TabGroup("Marcadores")]
    [Button]
    [GUIColor(1,1,1)]
    private void CreateAllOverlapUnpack()
    {
        CreateAllOverlap();
        UnpackPrefabs();
    }
    
    IEnumerator UpdateData()
    {
        while (UpdateLoop)
        {
            UpdateDataPozos();
            yield return new WaitForSeconds(updateDataTime);
        }
    }
    
    // IEnumerator UpdateDataSitiosGPS()
    // {
    //     while (UpdateLoop)
    //     {
    //         UpdateDataSitios_Marcadores();
    //         yield return new WaitForSeconds(updateDataTime);
    //     }
    // }
    
    public DataSitio GetDataSitioFromSiteDescription(SiteDescription sitio)
    {
        DataSitio newDataSitio = new DataSitio();
        
        newDataSitio.idSitio = sitio.Id;
        newDataSitio.nombre = sitio.Nombre;
        newDataSitio.abreviacion = sitio.Abreviacion;
        newDataSitio.fecha = sitio.Tiempo;
        newDataSitio.voltaje = sitio.Voltaje;
        newDataSitio.Estructura = sitio.Grupo;
        newDataSitio.tipoSitioPozo = (TipoSitioPozo)sitio.TipoEstacion;
        
        newDataSitio.longitud = sitio.Longitud;
        newDataSitio.latitud = sitio.Latitud;

        SiteBase sitebase = RequestAPI.Instance.dataRequestAPI.updateUnitySites.Sites.Find(
            item => item.Id == sitio.Id);

        if (sitebase != null)
        {
            newDataSitio.fecha = sitebase.Tiempo;
            newDataSitio.enlace = sitebase.Enlace;
            newDataSitio.fallaAC = sitebase.FallaAC;
            
            SignalsContainer signalNivel = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.NIVEL);

            if (signalNivel != null)
                newDataSitio.nivel.AddRange(signalNivel.Signals);

            SignalsContainer signalBomba = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.BOMBA);//signals["Bomba"]);

            if (signalBomba != null)
                newDataSitio.bomba.AddRange(signalBomba.Signals);
                    
            SignalsContainer signalPresion = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.PRESION);//signals["Pressure"]);

            if (signalPresion != null)
                newDataSitio.presion.AddRange(signalPresion.Signals);
            
            foreach (var signal in newDataSitio.presion)
            {
                if (!signal.DentroRango)
                    signal.Valor = 0;
            }
                    
            SignalsContainer signalGasto = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.GASTO);//signals["FlowRate"]);

            if (signalGasto != null)
                newDataSitio.gasto.AddRange(signalGasto.Signals);
            
            foreach (var signal in newDataSitio.gasto)
            {
                if (!signal.DentroRango)
                    signal.Valor = 0;
            }
                    
            SignalsContainer signalTotalizado = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.TOTALIZADO);//signals["FlowRateWithTotal"]);

            if (signalTotalizado != null)
                newDataSitio.totalizado.AddRange(signalTotalizado.Signals);
            
            foreach (var signal in newDataSitio.totalizado)
            {
                if (!signal.DentroRango)
                    signal.Valor = 0;
            }
            
            SignalsContainer signalBateria = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.VOLTAJE);//signals["Bateria"]);

            if (signalBateria != null)
                newDataSitio.Baterias.AddRange(signalBateria.Signals);
            
            SignalsContainer signalPerillaBomba = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.PERILLA_BOMBA);//signals["PerillaBomba"]);

            if (signalPerillaBomba != null)
                newDataSitio.PerillaBomba.AddRange(signalPerillaBomba.Signals);
            
            SignalsContainer signalPerillaGeneral = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.PERILLA_GENERAL);//signals["PerillaGeneral"]);

            if (signalPerillaGeneral != null)
                newDataSitio.PerillaGeneral.AddRange(signalPerillaGeneral.Signals);
            
            SignalsContainer signalVoltaje = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.VOLTAJE_RANGO);

            if (signalVoltaje != null)
                newDataSitio.Voltajes_Motor.AddRange(signalVoltaje.Signals);
            
            SignalsContainer signalCorriente = sitebase.SignalsContainer.Find(
                item => item.TipoSignal == (int)SignalBase.TipoSignalEnum.CORRIENTE_RANGO);

            if (signalCorriente != null)
                newDataSitio.Corrientes_Motor.AddRange(signalCorriente.Signals);
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
    }
    
    // [Button][GUIColor(0.25f,0.25f,1)]
    // [TabGroup("Sitios")]
    // public virtual void UpdateDataSitios_Marcadores()
    // {
    //     foreach (var sitio in listSitios)
    //     {
    //         //SitioGPS sitioGPS = sitio.GetComponent<SitioGPS>();
    //
    //         if (sitio.controlMarcadorMap != null)
    //         {
    //             ControlSitio dataSitio = listSitios.Find(item => item.dataSitio.idSitio == sitio.dataSitio.idSitio);
    //
    //             if (dataSitio != null)
    //             {
    //                 sitio.dataSitio.SetDataSitio(dataSitio.dataSitio);
    //             }
    //         }
    //     }
    // }

    [TabGroup("Totalizados")]
    [Button]
    public void GetTotalizadosByDates()
    {
        RequestAPI.Instance.GetTotalizadosByDates(
            totalizadosTime1.dateTime, 
            totalizadosTime2.dateTime,
            ReadTotalizados);
    }
    
    private void ReadTotalizados()
    {
        totalizadosPorFecha = RequestAPI.Instance.dataRequestAPI.totalizadosPorFecha.ListaTotalizadoPorSitio;

        totalizadosPorFecha = totalizadosPorFecha.OrderByDescending(
            x => x.Diferencia).ToList();
    }

    [TabGroup("Sitios")]
    [Button]
    public void SetSitioInterpolationValue(float val)
    {
        for (int i = 0; i < listSitios.Count; i++)
        {
            VWCBillboardSitio UIBilboard = listSitios[i].controlMarcadorMap.GetComponentInChildren<VWCBillboardSitio>();
            UIBilboard.RecalculateTilt(val);
            UIBilboard.RecalculateZoom(val);
        }
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
            var billboard = listSitios[i].controlMarcadorMap.GetComponentInChildren<VWCBillboardSitio>();
            originalPos[i] = billboard.transform.localPosition;
            billboard.positionGPSOriginal = originalPos[i];
#if UNITY_EDITOR
            PrefabUtility.RecordPrefabInstancePropertyModifications(billboard);
#endif
        }
    }
    
    [TabGroup("Overlap")]
    [Button]
    [GUIColor(1, 0, 0.5f)]
    private void RevertToOriginalPos()
    {
        for (int i = 0; i < listSitios.Count; i++)
        {
            var sitio1 = listSitios[i].controlMarcadorMap.GetComponentInChildren<VWCBillboardSitio>();
            sitio1.transform.localPosition = originalPos[i];
            sitio1.circleID.color = Color.cyan;
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
                var sitio1 = listSitios[i].controlMarcadorMap.GetComponentInChildren<VWCBillboardSitio>();

                sitio1.circleID.color = Color.cyan;

                for (int j = 0; j < listSitios.Count; j++)
                {
                    var sitio2 = listSitios[j].controlMarcadorMap.GetComponentInChildren<VWCBillboardSitio>();
                    if (sitio1 != sitio2)
                        if (Vector3.Distance(sitio1.transform.position.with(y:0), sitio2.transform.position.with(y:0)) < overlapingDistance)
                        {
                            //print($"Overlaping {sitio1.MyDataSitio.nombre}, {sitio2.MyDataSitio.nombre}");
                            sitio1.circleID.color = Color.red;
                            var dir = (sitio1.transform.position - sitio2.transform.position).normalized;
                            dir.y = 0;
                            sitio1.transform.Translate(dir * overlapMoveDistance, Space.World);
                            sitio1.positionFinalMarcador = sitio1.transform.localPosition;
                            sitio1.RecalculateLineRenderer();
                            finishOverlap = false;
                        }
                }

                contSteps++;
                if (contSteps > overlapingSteps) finishOverlap = true;
            }
        }
    }
    
    public string GetNameRegionByIndex(int index)
    {
        return GetNameRegionByID(GetIDRegionByIndex(index));
    }
    
    public string GetNameRegionByID(int idRegion)
    {
        if (RequestAPI.Instance != null)
        {
            Region regionAux = (RequestAPI.Instance.dataRequestAPI.regiones.Find(item => item.idRegion == idRegion));
            if (regionAux != null)    
                return regionAux.nombre;
        }

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
}
