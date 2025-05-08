using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ControlSitioUI : Singleton<ControlSitioUI>
{
    // [TabGroup("Signals")] public GameObject prefabSignal;
    // [TabGroup("Signals")] public GameObject contentSignalList;
    // [TabGroup("Signals")] public List<GameObject> signals = new List<GameObject>();

    [TabGroup("Sitios")] public List<GameObject> sitios = new List<GameObject>();
    //[TabGroup("Sitios")] public ControlDatos controlDatos;
    
    public float updateRate = 5;
    private float countdown;

    [ShowInInspector]
    public static bool moveScrollBarOnSelect = true;
    
    private void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            UpdateData();
            countdown = updateRate;
        }            
    }
    
    public virtual void UpdateData() { }
    
    // public void ClearSignals()
    // {
    //     foreach (var signal in signals)
    //     {
    //         if (Application.isEditor)
    //         {
    //             DestroyImmediate(signal);
    //         }
    //         else
    //         {
    //             Destroy(signal);
    //         }
    //     }
    //     
    //     signals.Clear();
    // }
    
    public virtual void SetSitioSelectUI_Prefab(ControlSitio sitio) { }
    
    public virtual void SetSitioSelectUI_GO(ControlSitio sitio) { }
    
    public void DeleteSitios()
    {
        foreach (var sitio in sitios)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(sitio);
            }
            else
            {
                Destroy(sitio);
            }
        }
        sitios.Clear();
        //signals.Clear();
    }

    public virtual void SetSitiosEnd()
    {
        
    }
}
