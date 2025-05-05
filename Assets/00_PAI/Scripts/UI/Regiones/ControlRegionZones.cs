using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Serialization;

public class ControlRegionZones : Singleton<ControlRegionZones>
{
    public List<Zone> zones;

    public float alphaValueON;
    public float alphaValueOFF;
    
    public float alphaBorderValueON;
    public float alphaBorderValueOFF;
    
    public float emissionIntensityON;
    public float emissionIntensityOFF;

    public float alphaInSelect = 190;
    public float alphaInNoSelect = 80;

    public void SetActiveZoneByID(int _idZone, bool _isActive)
    {
        Zone zoneAux = zones.Find(item => item.zonaID == _idZone);

        if (zoneAux != null)
        {
            if (zoneAux.useRenderer)
            {
                Renderer renderer = zoneAux.zonaGameObject.GetComponent<Renderer>();

                foreach (var mat in renderer.materials)
                {
                    //Debug.Log(mat.name);
                    if (mat.name == "border (Instance)")
                    {
                        Color colorAux = mat.color;
                        colorAux.a = _isActive ? alphaBorderValueON : alphaBorderValueOFF;
                        mat.color = colorAux;

                        mat.SetFloat("_EmissiveIntensity", _isActive ? emissionIntensityON : emissionIntensityOFF);
                        HDMaterial.ValidateMaterial(mat);
                    }
                    else
                    {
                        Color colorAux = mat.color;
                        colorAux.a = _isActive ? alphaValueON : alphaValueOFF;
                        mat.color = colorAux;
                    }
                }
            }

            if (zoneAux.useFSM)
            {
                if (zoneAux.zonaFSM != null)
                {
                    zoneAux.zonaFSM.FsmVariables.FindFsmFloat("inVallue").Value = _isActive ? alphaInSelect : alphaInNoSelect;
                }
            }
        }
    }
}

[Serializable]
public class Zone
{
    public bool useRenderer;
    public bool useFSM;
    public int zonaID;
    public GameObject zonaGameObject;
    public PlayMakerFSM zonaFSM;
}
