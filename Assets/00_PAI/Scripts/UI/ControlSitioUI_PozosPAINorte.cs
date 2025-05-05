using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ControlSitioUIPozosPaiNorte  : ControlSitioUI
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
            ControlSelectSitio controlSitio = sitio.GetComponent<ControlSelectSitio>();
        
            if (controlSitio != null)
            {
                if (controlSitio.gameObject.activeSelf)
                {
                    if (controlSitio.sitio.statusDataInTime == 1)
                    {
                        sitiosOrdenados.RegionesLabelUILabel[controlDatosAux.GetIndexRegionByID(controlSitio.sitio.MyDataSitio.Estructura)]
                            .coutActRegional++;
                        coutActTotal++;
                    }
                    else
                    {
                        sitiosOrdenados.RegionesLabelUILabel[controlDatosAux.GetIndexRegionByID(controlSitio.sitio.MyDataSitio.Estructura)]
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
    
    public override void SetSitioSelectUI_Prefab(SitioGPS _sitio)
    {
        #if UNITY_EDITOR
        GameObject instancePrefab = null;
        switch (_sitio.MyDataSitio.tipoSitioPozo)
        {
            case TipoSitioPozo.PozoLerma1:
                instancePrefab = controlDatosAux.prefabPanelSitioLerma1;
                break;
            case TipoSitioPozo.PozoLerma2:
                instancePrefab = controlDatosAux.prefabPanelSitioLerma1;
                break;
            case TipoSitioPozo.Repetidor:
                instancePrefab = controlDatosAux.prefabPanelRepetidor;
                break;
            case TipoSitioPozo.EnConstruccion:
                instancePrefab = controlDatosAux.prefabPanelSitioEnConstruccion;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        string prefabPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(instancePrefab);
        Object prefab = AssetDatabase.LoadAssetAtPath(prefabPath, typeof(Object));

        // GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, 
        //     panelGrupoSitios[(int)_sitio.MyDataSitio.Estructura].transform);
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, 
            sitiosOrdenados.RegionesLabelUIList[(int)_sitio.MyDataSitio.Estructura - 1].
                rootRegion.transform);

        RectTransform m_RectTransform = instance.GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = new Vector2(0,0);
        
        ControlSelectSitio controlSignal = instance.GetComponent<ControlSelectSitio>();
        controlSignal.SetSitio(_sitio);
        instance.name = $"PanelSitio_{_sitio.MyDataSitio.nombre}_{_sitio.MyDataSitio.Estructura}";
        sitios.Add(instance);

        PrefabUtility.RecordPrefabInstancePropertyModifications(instance);
        #endif
    }
    
    public override void SetSitioSelectUI_GO(SitioGPS _sitio)
    {
        GameObject instancePrefab = null;
        
        switch (_sitio.MyDataSitio.tipoSitioPozo)
        {
            case TipoSitioPozo.PozoLerma1:
                instancePrefab = controlDatosAux.prefabPanelSitioLerma1;
                break;
            case TipoSitioPozo.PozoLerma2:
                instancePrefab = controlDatosAux.prefabPanelSitioLerma1;
                break;
            case TipoSitioPozo.Repetidor:
                instancePrefab = controlDatosAux.prefabPanelRepetidor;
                break;
            case TipoSitioPozo.EnConstruccion:
                instancePrefab = controlDatosAux.prefabPanelSitioEnConstruccion;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        // GameObject instance = Instantiate(instancePrefab, 
        //     panelGrupoSitios[(int)_sitio.MyDataSitio.Estructura].transform);
        GameObject instance = Instantiate(instancePrefab, 
            sitiosOrdenados.RegionesLabelUIList[controlDatosAux.GetIndexRegionByID(_sitio.MyDataSitio.Estructura)].
                rootRegion.transform);

        RectTransform m_RectTransform = instance.GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = new Vector2(0,0);
        
        ControlSelectSitio controlSignal = instance.GetComponent<ControlSelectSitio>();
        controlSignal.SetSitio(_sitio);
        instance.name = $"PanelSitio_{_sitio.MyDataSitio.nombre}_{_sitio.MyDataSitio.Estructura}";
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
                        //sitio.toggleSelectForAnalitics.onValueChanged.Invoke();
                    }
                }
            }
        }
    }
}
