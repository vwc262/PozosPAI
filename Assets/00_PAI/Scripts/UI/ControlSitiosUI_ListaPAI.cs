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
        
        ((SitiosOrdenados_PAI)sitiosOrdenados).ResetTotales();
        
        foreach (var controlSitio in sitios)
        {
            //ControlUISitio controlSitio = sitio.GetComponent<ControlUISitio>();
        
            if (controlSitio != null)
            {
                if (controlSitio.gameObject.activeSelf)
                {
                    if (controlSitio.sitio.dataInTime)
                    {
                        ((SitiosOrdenados_PAI)sitiosOrdenados).RegionesLabelUILabel[ControlDatos.singleton.GetIndexRegionByID(controlSitio.sitio.dataSitio.Estructura)]
                            .coutActRegional++;
                        coutActTotal++;
                    }
                    else
                    {
                        ((SitiosOrdenados_PAI)sitiosOrdenados).RegionesLabelUILabel[ControlDatos.singleton.GetIndexRegionByID(controlSitio.sitio.dataSitio.Estructura)]
                            .coutNoActRegional++;
                        coutNoActTotal++;
                    }
                }
            }
        }
        
        if (TextActTotal != null) TextActTotal.text = coutActTotal.ToString();
        if (TextNoActTotal != null) TextNoActTotal.text = coutNoActTotal.ToString();

        ((SitiosOrdenados_PAI)sitiosOrdenados).SetTextTotales();
    }
    
    public override void SetSitioSelectUI_GO(ControlSitio sitio)
    {
        GameObject instancePrefab = ControlPrefabs.singleton.GetPrefabUIListSitio(sitio.dataSitio.tipoSitio);
        
        if (instancePrefab != null)
        {
            GameObject instance = Instantiate(instancePrefab,
                ((SitiosOrdenados_PAI)sitiosOrdenados)
                .RegionesLabelUIList[ControlDatos.singleton.GetIndexRegionByID(sitio.dataSitio.Estructura)].rootRegion
                .transform);

            RectTransform m_RectTransform = instance.GetComponent<RectTransform>();
            m_RectTransform.anchoredPosition = new Vector2(0, 0);

            ControlUISitio controlUI_Sitio = instance.GetComponent<ControlUISitio>();
            controlUI_Sitio.SetSitio(sitio);

            if (sitio.controlUIsitio != null)
                Destroy(sitio.controlUIsitio.gameObject);
            sitio.controlUIsitio = controlUI_Sitio;

            instance.name = $"PanelSitio_{sitio.dataSitio.nombre}_{sitio.dataSitio.Estructura}";
            sitios.Add(controlUI_Sitio);
        }
    }

    [Button]
    public void DeseleccionarAll()
    {
        if (sitiosOrdenados != null)
        {
            foreach (var listSitios in ((SitiosOrdenados_PAI)sitiosOrdenados).RegionesLabelUIList)
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
