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
    [TabGroup("Signals")] public GameObject prefabSignal;
    [TabGroup("Signals")] public GameObject contentSignalList;
    [TabGroup("Signals")] public List<GameObject> signals = new List<GameObject>();

    [TabGroup("Sitios")] public List<GameObject> sitios = new List<GameObject>();
    [TabGroup("Sitios")] public ControlDatosAux controlDatosAux;
    
    public float updateRate = 5;
    private float countdown;
    
    //public GameObject contentSitiosList;
    //public UnityEvent<DataSitio> SitioSeleccionado;
    
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

    /*public void SetSelectedSitio(DataSitio _DataSitio)
    {
        SitioSeleccionado.Invoke(_DataSitio);
        
        ClearSignals();
        
        if (_DataSitio != null)
        {
            if (textoNombre != null)
                textoNombre.text = _DataSitio.nombre + " (" + _DataSitio.abreviacion + ")";

            if (textoFecha != null)
                textoFecha.text = _DataSitio.fecha;
            
            if (textoVoltaje != null)
                textoVoltaje.text = "Voltaje: " + _DataSitio.voltaje.ToString();

            if (contentSignalList != null && prefabSignal != null)
            {
                AddSignalPanel("Bomba", null);
                
                foreach (var signal in _DataSitio.bomba)
                {
                    AddSignalPanel("Bomba", signal);
                }

                AddSignalPanel("Presion", null);

                foreach (var signal in _DataSitio.presion)
                {
                    AddSignalPanel("Presion", signal);
                }

                AddSignalPanel("Gasto", null);

                foreach (var signal in _DataSitio.gasto)
                {
                    AddSignalPanel("Gasto", signal);
                }

                AddSignalPanel("Totalizado", null);

                foreach (var signal in _DataSitio.totalizado)
                {
                    AddSignalPanel("Totalizado", signal);
                }
            }
        }
    }*/

    public void AddSignalPanel(string _type, SignalBase _signal)
    {
        GameObject instance = Instantiate(prefabSignal, Vector3.zero, Quaternion.identity,
            contentSignalList.transform);
        instance.transform.localEulerAngles = Vector3.zero;
        
        RectTransform m_RectTransform = instance.GetComponent<RectTransform>();
        m_RectTransform.anchoredPosition = new Vector2(0,0);
        
        ControlSignalUI controlSignal = instance.GetComponent<ControlSignalUI>();
        controlSignal.SetSignal(_type, _signal);

        signals.Add(instance);
    }
    
    public void ClearSignals()
    {
        foreach (var signal in signals)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(signal);
            }
            else
            {
                Destroy(signal);
            }
        }
        
        signals.Clear();
    }
    
    public virtual void SetSitioSelectUI_Prefab(SitioGPS _sitio) { }
    
    public virtual void SetSitioSelectUI_GO(SitioGPS _sitio) { }
    
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
        signals.Clear();
    }

    public virtual void SetSitiosEnd()
    {
        
    }
}
