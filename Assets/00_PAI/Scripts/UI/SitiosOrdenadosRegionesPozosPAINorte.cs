using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SitiosOrdenadosRegionesPozosPAINorte
    : SitiosOrdenados
{ 
    [TabGroup("Regiones")] public List<ControlRegionUILabel> RegionesLabelUILabel;
    [TabGroup("Regiones")] public List<ControlRegionUIList> RegionesLabelUIList;
    [TabGroup("UI")] public Sprite noFoldRegion;
    
    public GameObject contentSitiosList;

    public override void Init()
    {
    }

    public void SetListenersRegiones()
    {
        for (int i = 0; i < RegionesLabelUILabel.Count; i++)
        {
            var i1 = i;
            RegionesLabelUILabel[i].onValueIsOnChange.AddListener((bool val) =>
            {
                //SelectRegion(i1, val);
                
                if (ControlDatos._singletonExists)
                    SetEnableZonaByID(ControlDatos.singleton.GetIDRegionByIndex(i1), val);
            });
        }
    }
    
    public override void clearListas()
    {
        // foreach (var _region in RegionsLabelUIList)
        // {
        //     _region.sitiosRegion.Clear();
        // }

        ResetUIRegiones();
        
        dictionaryListSitios.Clear();
    }

    public override void InitListasUI()
    {
        CreateUIRegiones();
        //CreateUIAcciones();
    }

    public override void updateListSitios()
    {
        for (int i = 0; i < RegionesLabelUIList.Count; i++)
        {
            if (RegionesLabelUIList[i].rootRegion != null) 
                RegionesLabelUIList[i].sitiosRegion = RegionesLabelUIList[i].rootRegion.GetComponentsInChildren<ControlSelectSitio>().ToList();
            
            dictionaryListSitios.Add(i,RegionesLabelUIList[i].sitiosRegion);
        }
	    
        //SetSelectedColor();
	    
        OrdenGastoPresionTotalizado(0);
    }
    
    public override void ToggleRegion(int index)
    {
        int contRegionesActivas = 0;
        
        for (int i = 0; i < RegionesLabelUIList.Count; i++)
        {
            if (i == index)
                RegionesLabelUIList[i].gameObject.SetActive(!RegionesLabelUIList[i].gameObject.activeSelf);
            
            if (RegionesLabelUIList[i].gameObject.activeSelf)
                contRegionesActivas++;
        }
        
        for (int i = 0; i < RegionesLabelUIList.Count; i++)
        {
            RegionesLabelUILabel[i].SetIsOn(RegionesLabelUIList[i].gameObject.activeSelf);
            //RegionesLabelUILabel[i].foldButtonRegion.sprite = foldInRegion;
            if(RegionesLabelUIList[i].gameObject.activeSelf)
            {
                //RegionesLabelUILabel[i].foldButtonRegion.sprite = foldOutRegion;
                var rect1 = RegionesLabelUIList[i].gameObject.GetComponent<RectTransform>().rect;
                RegionesLabelUIList[i].gameObject.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(rect1.width, heights[contRegionesActivas]);
			    
                var content = RegionesLabelUIList[i].gameObject.GetComponent<ScrollRect>().content;
                var rect2 = content.GetComponent<RectTransform>().rect;
                content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -rect2.height/2);
            }
        }
    }

    public void ResetTotales()
    {
        foreach (var regional in RegionesLabelUILabel)
        {
            regional.coutActRegional = 0;
            regional.coutNoActRegional = 0;
        }
    }
    
    public void SetTextTotales()
    {
        foreach (var regional in RegionesLabelUILabel)
        {
            if (regional.TextActRegional != null) regional.TextActRegional.text = regional.coutActRegional.ToString();
            if (regional.TextNoActRegional != null) regional.TextNoActRegional.text = regional.coutNoActRegional.ToString();
        }
    }

    public void ResetUIRegiones()
    {
        foreach (var labelRegion in  RegionesLabelUILabel)
        {
            Destroy(labelRegion.gameObject);
        }
        
        RegionesLabelUILabel.Clear();
        
        foreach (var ListRegion in  RegionesLabelUIList)
        {
            Destroy(ListRegion.gameObject);
        }
        
        RegionesLabelUIList.Clear();
    }

    public void CreateUIRegiones()
    {
        if (ControlDatos._singletonExists)
        {
            for (int i = 0; i < totalRegiones; i++)
            {
                ControlRegionUILabel instanceLabel = Instantiate(ControlDatos.singleton.prefabUIRegionaLabel, contentSitiosList.transform).
                    GetComponent<ControlRegionUILabel>();
                
                instanceLabel.sitiosOrdenados = this;
                instanceLabel.region = (i + 1);
                instanceLabel.regionID = ControlDatos.singleton.GetIDRegionByIndex(i);
                string nameRegion = ControlDatos.singleton.GetNameRegionByID(ControlDatos.singleton.GetIDRegionByIndex(i));
                instanceLabel.SetNameRegional(nameRegion);
                instanceLabel.foldButtonRegion.color = new Color(selectedColors[i].r,selectedColors[i].g,selectedColors[i].b);
                instanceLabel.gameObject.name = "Label " + nameRegion;
                RegionesLabelUILabel.Add(instanceLabel);
                
                ControlRegionUIList instanceList = Instantiate(ControlDatos.singleton.prefabUIRegionaList, contentSitiosList.transform).
                    GetComponent<ControlRegionUIList>();
                
                instanceList.gameObject.name = "List " + instanceLabel.region.ToString();
                RegionesLabelUIList.Add(instanceList);
            }

            SetListenersRegiones();
        }

        if (ControlAccesoPozosPAI.singleton.colapseList)
        {
            for (int i = 0; i < RegionesLabelUILabel.Count; i++)
            {
                //Debug.Log("Region: " + RegionesLabelUILabel[i].region);

                switch ((RequestAPI.Proyectos)ControlDatos_PAI.singleton.listIdRegionales[
                            RegionesLabelUILabel[i].region - 1])
                {
                    case RequestAPI.Proyectos.Teoloyucan:
                        RegionesLabelUIList[i].gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                            ControlAccesoPozosPAI.Proyectos.Teoloyucan));
                        break;

                    case RequestAPI.Proyectos.PozosZumpango:
                        RegionesLabelUIList[i].gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                            ControlAccesoPozosPAI.Proyectos.PozosZumpango));
                        break;

                    //case RequestAPI.Proyectos.PozosReyesFerrocarril:
                    //    RegionesLabelUIList[i].gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                    //        ControlAccesoPozosPAI.Proyectos.PozosReyesFerrocarril));
                    //    break;

                    case RequestAPI.Proyectos.PozosAIFA:
                        RegionesLabelUIList[i].gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                            ControlAccesoPozosPAI.Proyectos.PozosAIFA));
                        break;
                }
            }
        }

        ToggleRegion(-1);

        if (totalRegiones == 1)
        {
            RegionesLabelUILabel[0].foldButtonRegion.sprite = noFoldRegion;
            RegionesLabelUILabel[0].RegionButtonCollapse.SetActive(false);
        }
    }

    // public void CreateUIAcciones()
    // {
    //     if (ControlAccionesRegiones._singletonExists)
    //     {
    //         ControlAccionesRegiones.singleton.SetEditorsAccionesRegiones(totalRegiones);
    //     }
    // }

    public void SetEnableZonaByID(int ID_zona, bool enable)
    {
        if (ControlRegionZones._singletonExists)
            ControlRegionZones.singleton.SetActiveZoneByID(ID_zona, enable);
    }
}
