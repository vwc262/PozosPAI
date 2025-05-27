using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using Lean.Touch;
using Raskulls.ScriptableSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlMarcadorSitio : MonoBehaviour
{
    public ControlSitio sitio;
    public GameObject rootOverlaping;
    public VWCBillboardSitio billboardObj;
    
    [TabGroup("Debug")] public bool debugSphere;
    [TabGroup("Debug")] public GameObject colliderDebug;
    [TabGroup("Debug")] public GameObject sphereDebug;
    
    [TabGroup("Seleccion")] public SE_SelectSitio eventSelectSitio;
    [TabGroup("Seleccion")] public static LeanSelectByFinger leanSelectByFinger;
    [TabGroup("Seleccion")] private LeanSelectable selectable;
    [TabGroup("Seleccion")] public float timeToDobleClick;
    [TabGroup("Seleccion")] public Vector3 SelectedSitioOffset;
    
    [TabGroup("Interfaz")] public TMPro.TMP_Text textoIdSitioUnity;
    [TabGroup("Interfaz")] public TMPro.TMP_Text textoAlias;
    [TabGroup("Interfaz")] public List<GameObject> MarcaSeleccionado;
    [TabGroup("Interfaz")] public List<GameObject> MarcaNoSeleccionado;
    
    [TabGroup("Update")] public float updateRate = 60;
    [TabGroup("Update")] private Coroutine UpateMarcadorCoroutine;
    [TabGroup("Update")] public float diferencia;
    [TabGroup("Update")] public float umbralGreen;
    [TabGroup("Update")] public float umbralYellow;
    [TabGroup("Update")] public Color statusColor;
    [TabGroup("Update")] public Color statusColor1;
    [TabGroup("Update")] public Color statusColor2;
    [TabGroup("Update")] public Color statusColor3;
    
    public void Start()
    {
        UpateMarcadorCoroutine = StartCoroutine(StatusUI());
        
        DeseleccionarSitio();
        
        if (billboardObj != null)
            billboardObj.controlSitio = this;
    }

    public virtual IEnumerator StatusUI()
    {
        yield return new WaitForSeconds(updateRate);
    }
    
    public void AddToSelectanbles()
    {
        if (leanSelectByFinger == null)
        {
            leanSelectByFinger = FindObjectOfType<LeanSelectByFinger>();
        }
        
        if (selectable == null)
            selectable = gameObject.GetComponent<LeanSelectable>();

        if (leanSelectByFinger != null)
        {
            List<LeanSelectable> MySelectables = new List<LeanSelectable>();
            MySelectables.AddRange(leanSelectByFinger.Selectables.ToArray());
            
            foreach (var objselectable in MySelectables)
            {
                leanSelectByFinger.Deselect(objselectable);
            }
                
            if (selectable != null)
                leanSelectByFinger.Selectables.Add(selectable);
        }
    }

    public virtual void SetSelectedSitio()
    {
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.SetSelectedSitio(sitio);
        
        if (timeToDobleClick > 0)
        {
            Debug.Log("DobleClick");
            if (ControlParticular._singletonExists)
                ControlParticular.singleton.InitCoroutineActivateParticular();
        }
        else
        {
            timeToDobleClick = 0.5f;
            StartCoroutine(CountDownDobleClick());
        }
    }
    
    public virtual void SeleccionarSitio()
    {
        foreach (var go in MarcaSeleccionado)
        {
            go.SetActive(true);
        }
        
        foreach (var go in MarcaNoSeleccionado)
        {
            go.SetActive(false);
        }

        if (billboardObj != null)
            billboardObj.SetSelectedSitio();
    }
    
    public virtual void DeseleccionarSitio()
    {
        foreach (var go in MarcaSeleccionado)
        {
            go.SetActive(false);
        }
        
        foreach (var go in MarcaNoSeleccionado)
        {
            go.SetActive(true);
        }

        if (billboardObj != null)
            billboardObj.SetDeselectedSitio();
    }

    public virtual void SetDataSitio(ControlSitio _Sitio) { }

    public void CreateSphere()
    {
        if(!debugSphere)
            return;
        
        var go = Instantiate(sphereDebug,colliderDebug.transform.position, Quaternion.identity);
        go.SetActive(true);
    }
    
    public IEnumerator CountDownDobleClick()
    {
        while (timeToDobleClick > 0)
        {
            timeToDobleClick -= Time.deltaTime;
            yield return null;
        }
    }

    public Vector3 GetMarcadorPosition()
    {
        if (billboardObj != null)
            return billboardObj.transform.position;

        return transform.position;
    }
}
