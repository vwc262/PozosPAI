using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using Lean.Touch;
using Raskulls.ScriptableSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlMarcadorSitio : MonoBehaviour
{
    public ControlSitio sitio;
    
    public SE_SelectSitio eventSelectSitio;
    
    public static LeanSelectByFinger leanSelectByFinger;
    private LeanSelectable selectable;

    //public ControlSitioUI controlSitioUI;

    public TMPro.TMP_Text textoNombre;
    public TMPro.TMP_Text textoFecha;
    
    public TMPro.TMP_Text textoIdSitioUnity;
    public TMPro.TMP_Text textoAlias;

    public bool debugSphere;

    public GameObject colliderDebug;
    public GameObject sphereDebug;
    
    public List<SpriteRenderer> rendererUIStatus = new List<SpriteRenderer>();
    public List<TMPro.TMP_Text> textUIStatus = new List<TMPro.TMP_Text>();
    public float updateRate = 60;
    private Coroutine UpateMarcadorCoroutine;
    public float diferencia;
    public float umbralGreen;
    public float umbralYellow;

    public Color statusColor;
    
    public Color statusColor1;
    public Color statusColor2;
    public Color statusColor3;
    
    public List<GameObject> MarcaSeleccionado;
    public List<GameObject> MarcaNoSeleccionado;
    
    public bool selectedSitio;

    public List<MeshRenderer> Bombas;
    
    public GameObject billboardObj;
    public Vector3 billboardSelectedPos;
    public Vector3 billboardUnSelectedPos;
    
    public bool dataInTime = false;

    public float timeToDobleClick;
    
    public void Start()
    {
        UpateMarcadorCoroutine = StartCoroutine(StatusUI());
        
        DeseleccionarSitio();
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
    
    public void SeleccionarSitio()
    {
        eventSelectSitio.Raise(this);

        foreach (var go in MarcaSeleccionado)
        {
            go.SetActive(true);
        }
        
        foreach (var go in MarcaNoSeleccionado)
        {
            go.SetActive(false);
        }

        selectedSitio = true;
    }
    
    public void DeseleccionarSitio()
    {
        foreach (var go in MarcaSeleccionado)
        {
            go.SetActive(false);
        }
        
        foreach (var go in MarcaNoSeleccionado)
        {
            go.SetActive(true);
        }
        
        selectedSitio = false;
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
}
