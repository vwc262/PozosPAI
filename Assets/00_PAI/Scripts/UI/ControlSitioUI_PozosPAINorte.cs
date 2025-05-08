using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ControlSitioUI_PAI  : ControlSitioUI
{
    public Text TextNoActTotal;
    public Text TextActTotal;
    
    public int coutNoActTotal;
    public int coutActTotal;

    [TabGroup("Sitios")] public SitiosOrdenadosRegionesPozosPAINorte sitiosOrdenados;
    
    public override void UpdateData()
    {
        coutNoActTotal = 0;
        coutActTotal = 0;
        
        sitiosOrdenados.ResetTotales();
        
        foreach (var sitio in sitios)
        {
            ControlUISitio controlSitio = sitio.GetComponent<ControlUISitio>();
        
            if (controlSitio != null)
            {
                if (controlSitio.gameObject.activeSelf)
                {
                    if (controlSitio.sitio.dataInTime)
                    {
                        sitiosOrdenados.RegionesLabelUILabel[ControlDatos.singleton.GetIndexRegionByID(controlSitio.sitio.dataSitio.Estructura)]
                            .coutActRegional++;
                        coutActTotal++;
                    }
                    else
                    {
                        sitiosOrdenados.RegionesLabelUILabel[ControlDatos.singleton.GetIndexRegionByID(controlSitio.sitio.dataSitio.Estructura)]
                            .coutNoActRegional++;
                        coutNoActTotal++;
                    }
                }
            }
        }
        
        if (TextActTotal != null) TextActTotal.text = coutActTotal.ToString();
        if (TextNoActTotal != null) TextNoActTotal.text = coutNoActTotal.ToString();

        sitiosOrdenados.SetTextTotales();
    }
    
    // public override void SetSitioSelectUI_Prefab(ControlSitio sitio)
    // {
    //     #if UNITY_EDITOR
    //     GameObject instancePrefab = null;
    //     switch (sitio.dataSitio.tipoSitioPozo)
    //     {
    //         case TipoSitioPozo.PozoLerma1:
    //             instancePrefab = controlDatos.prefabPanelSitioLerma1;
    //             break;
    //         case TipoSitioPozo.PozoLerma2:
    //             instancePrefab = controlDatos.prefabPanelSitioLerma1;
    //             break;
    //         case TipoSitioPozo.Repetidor:
    //             instancePrefab = controlDatos.prefabPanelRepetidor;
    //             break;
    //         case TipoSitioPozo.EnConstruccion:
    //             instancePrefab = controlDatos.prefabPanelSitioEnConstruccion;
    //             break;
    //         default:
    //             throw new ArgumentOutOfRangeException();
    //     }
    //     string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(instancePrefab);
    //     Object prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(Object));
    //
    //     GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, 
    //         sitiosOrdenados.RegionesLabelUIList[(int)sitio.dataSitio.Estructura - 1].
    //             rootRegion.transform);
    //
    //     RectTransform m_RectTransform = instance.GetComponent<RectTransform>();
    //     m_RectTransform.anchoredPosition = new Vector2(0,0);
    //     
    //     ControlUISitio controlSignal = instance.GetComponent<ControlUISitio>();
    //     controlSignal.SetSitio(sitio);
    //     instance.name = $"PanelSitio_{sitio.dataSitio.nombre}_{sitio.dataSitio.Estructura}";
    //     sitios.Add(instance);
    //
    //     PrefabUtility.RecordPrefabInstancePropertyModifications(instance);
    //     #endif
    // }
    
    public override void SetSitioSelectUI_GO(ControlSitio sitio)
    {
        GameObject instancePrefab = null;
        
        switch (sitio.dataSitio.tipoSitioPozo)
        {
            case TipoSitioPozo.PozoLerma1:
                instancePrefab = ControlDatos.singleton.prefabPanelSitioLerma1;
                break;
            case TipoSitioPozo.PozoLerma2:
                instancePrefab = ControlDatos.singleton.prefabPanelSitioLerma1;
                break;
            case TipoSitioPozo.Repetidor:
                instancePrefab = ControlDatos.singleton.prefabPanelRepetidor;
                break;
            case TipoSitioPozo.EnConstruccion:
                instancePrefab = ControlDatos.singleton.prefabPanelSitioEnConstruccion;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        GameObject instance = Instantiate(instancePrefab, 
            sitiosOrdenados.RegionesLabelUIList[ControlDatos.singleton.GetIndexRegionByID(sitio.dataSitio.Estructura)].
                rootRegion.transform);

        RectTransform m_RectTransform = instance.GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = new Vector2(0,0);
        
        ControlUISitio controlUI_Sitio = instance.GetComponent<ControlUISitio>();
        controlUI_Sitio.SetSitio(sitio);
        sitio.controlUIsitio = controlUI_Sitio;
        instance.name = $"PanelSitio_{sitio.dataSitio.nombre}_{sitio.dataSitio.Estructura}";
        sitios.Add(instance);
    }

    [Button]
    public void DeseleccionarAll()
    {
        if (sitiosOrdenados != null)
        {
            foreach (var listSitios in sitiosOrdenados.RegionesLabelUIList)
            {
                foreach (var sitio in listSitios.sitiosRegion)
                {
                    if (sitio.toggleSelectForAnalitics != null)
                    {
                        sitio.toggleSelectForAnalitics.isOn = false;
                    }
                }
            }
        }
    }
}
