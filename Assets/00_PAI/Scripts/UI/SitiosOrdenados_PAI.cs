using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SitiosOrdenados_PAI : SitiosOrdenados
{ 
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
    
    public override void clearListasRegiones()
    {
        ResetUIRegiones();
        
        dictionaryListSitios.Clear();
    }

    public override void InitListasUIRegiones()
    {
        clearListasRegiones();
        CreateUIRegiones();
    }

    public override void updateListSitios()
    {
        for (int i = 0; i < RegionesLabelUIList.Count; i++)
        {
            if (RegionesLabelUIList[i].rootRegion != null) 
                RegionesLabelUIList[i].sitiosRegion = RegionesLabelUIList[i].rootRegion.GetComponentsInChildren<ControlUISitio>().ToList();
            
            dictionaryListSitios.Add(i,RegionesLabelUIList[i].sitiosRegion);
        }
	    
        OrdenGastoPresionTotalizado(0);
    }
    
    public override void ToggleRegion(int index)
    {
        int contRegionesActivas = 0;
        
        
        if (header != null)
            HeigtHeader = header.GetComponent<RectTransform>().rect.height;
        if (contentSitiosList != null)
            HeigtContenedor = contentSitiosList.GetComponent<RectTransform>().rect.height;
        if (RegionesLabelUILabel.Count > 0)
        {
            int activeAndEnabledregions = RegionesLabelUILabel.Where(x => x.gameObject.activeSelf).Count();
            
            HeigtLabelRegiones = (RegionesLabelUILabel[0].GetComponent<RectTransform>().rect.height + HeigtAuxSpacing) *
                                 activeAndEnabledregions;
        }
        
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
            
            if(RegionesLabelUIList[i].gameObject.activeSelf)
            {
                var rect1 = RegionesLabelUIList[i].gameObject.GetComponent<RectTransform>().rect;
                RegionesLabelUIList[i].gameObject.GetComponent<RectTransform>().sizeDelta =
                    new Vector2(rect1.width, (HeigtContenedor - HeigtHeader - HeigtLabelRegiones) / contRegionesActivas);
			    
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
                // bool activation = true;
                // for (int j = 0; j < ControlAccesoPozosPAI.singleton.configuration.regionesDeshabilitadas.Count; j++)
                //     if(ControlAccesoPozosPAI.singleton.configuration.regionesDeshabilitadas.Contains(i))
                //         activation = false;
                //
                // CreateUIRegion(i,activation);
                
                
                if (ControlDatos.singleton.GetIDRegionByIndex(i) != 0)
                    CreateUIRegion(i);
                else
                {
                    CreateUIRegion(i, ControlAccesoPozosPAI.singleton.configuration.habilitarBarrientos);
                }
            }

            SetListenersRegiones();
        }

        if (ControlAccesoPozosPAI.singleton.colapseList)
        {
            for (int i = 0; i < RegionesLabelUILabel.Count; i++)
            {
                switch ((EstructurasAPI.Proyectos)ControlDatos.singleton.regiones[
                            RegionesLabelUILabel[i].region - 1].idRegion)
                {
                    case EstructurasAPI.Proyectos.Teoloyucan:
                        RegionesLabelUIList[i].gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                            ControlAccesoPozosPAI.Proyectos.Teoloyucan));
                        break;

                    case EstructurasAPI.Proyectos.PozosZumpango:
                        RegionesLabelUIList[i].gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                            ControlAccesoPozosPAI.Proyectos.PozosZumpango));
                        break;
                    
                    case EstructurasAPI.Proyectos.PozosAIFA:
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

    public void CreateUIRegion(int i, bool defaultActive = true)
    {
        ControlRegionUILabel instanceLabel = Instantiate(ControlPrefabs.singleton.prefabUIRegionaLabel, contentSitiosList.transform).
            GetComponent<ControlRegionUILabel>();
                
        instanceLabel.sitiosOrdenados = this;
        instanceLabel.region = (i + 1);
        instanceLabel.regionID = ControlDatos.singleton.GetIDRegionByIndex(i);
        string nameRegion = ControlDatos.singleton.GetNameRegionByID(ControlDatos.singleton.GetIDRegionByIndex(i),0);
        instanceLabel.SetNameRegional(nameRegion);
        if (selectedColors.Length > i)
            instanceLabel.foldButtonRegion.color = new Color(selectedColors[i].r,selectedColors[i].g,selectedColors[i].b);
        instanceLabel.gameObject.name = "Label " + nameRegion;
        RegionesLabelUILabel.Add(instanceLabel);
                
        ControlRegionUIList instanceList = Instantiate(ControlPrefabs.singleton.prefabUIRegionaList, contentSitiosList.transform).
            GetComponent<ControlRegionUIList>();
                
        instanceList.gameObject.name = "List " + instanceLabel.region.ToString();
        RegionesLabelUIList.Add(instanceList);
        
        instanceLabel.gameObject.SetActive(defaultActive);
        instanceList.gameObject.SetActive(defaultActive);
    }

    public void SetEnableZonaByID(int ID_zona, bool enable)
    {
        if (ControlRegionZones._singletonExists)
            ControlRegionZones.singleton.SetActiveZoneByID(ID_zona, enable);
    }
}
