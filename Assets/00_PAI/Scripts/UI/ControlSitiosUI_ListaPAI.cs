using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ControlSitiosUI_ListaPAI : ControlSitiosUI_Lista
{
    public Text TextNoActTotal;
    public Text TextActTotal;
    
    public int coutNoActTotal;
    public int coutActTotal;
    
    public override void UpdateData()
    {
        coutNoActTotal = 0;
        coutActTotal = 0;
        
        ((SitiosOrdenadosRegionesPozosPAINorte)sitiosOrdenados).ResetTotales();
        
        foreach (var controlSitio in sitios)
        {
            //ControlUISitio controlSitio = sitio.GetComponent<ControlUISitio>();
        
            if (controlSitio != null)
            {
                if (controlSitio.gameObject.activeSelf)
                {
                    if (controlSitio.sitio.dataInTime)
                    {
                        ((SitiosOrdenadosRegionesPozosPAINorte)sitiosOrdenados).RegionesLabelUILabel[ControlDatos.singleton.GetIndexRegionByID(controlSitio.sitio.dataSitio.Estructura)]
                            .coutActRegional++;
                        coutActTotal++;
                    }
                    else
                    {
                        ((SitiosOrdenadosRegionesPozosPAINorte)sitiosOrdenados).RegionesLabelUILabel[ControlDatos.singleton.GetIndexRegionByID(controlSitio.sitio.dataSitio.Estructura)]
                            .coutNoActRegional++;
                        coutNoActTotal++;
                    }
                }
            }
        }
        
        if (TextActTotal != null) TextActTotal.text = coutActTotal.ToString();
        if (TextNoActTotal != null) TextNoActTotal.text = coutNoActTotal.ToString();

        ((SitiosOrdenadosRegionesPozosPAINorte)sitiosOrdenados).SetTextTotales();
    }
    
    public override void SetSitioSelectUI_GO(ControlSitio sitio)
    {
        GameObject instancePrefab = null;
        
        switch (sitio.dataSitio.tipoSitioPozo)
        {
            case TipoSitioPozo.Pozo:
                instancePrefab = ControlPrefabs.singleton.prefabPanelUISitio;
                break;
            case TipoSitioPozo.Repetidor:
                instancePrefab = ControlPrefabs.singleton.prefabPanelUIRepetidor;
                break;
            case TipoSitioPozo.EnConstruccion:
                instancePrefab = ControlPrefabs.singleton.prefabPanelUISitioEnConstruccion;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        GameObject instance = Instantiate(instancePrefab, 
            ((SitiosOrdenadosRegionesPozosPAINorte)sitiosOrdenados).RegionesLabelUIList[ControlDatos.singleton.GetIndexRegionByID(sitio.dataSitio.Estructura)].
                rootRegion.transform);

        RectTransform m_RectTransform = instance.GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = new Vector2(0,0);
        
        ControlUISitio controlUI_Sitio = instance.GetComponent<ControlUISitio>();
        controlUI_Sitio.SetSitio(sitio);
        sitio.controlUIsitio = controlUI_Sitio;
        instance.name = $"PanelSitio_{sitio.dataSitio.nombre}_{sitio.dataSitio.Estructura}";
        sitios.Add(controlUI_Sitio);
    }

    [Button]
    public void DeseleccionarAll()
    {
        if (sitiosOrdenados != null)
        {
            foreach (var listSitios in ((SitiosOrdenadosRegionesPozosPAINorte)sitiosOrdenados).RegionesLabelUIList)
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
