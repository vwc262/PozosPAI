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
    //public DataSitio MyDataSitio;
    public ControlSitio sitio;
    
    public SE_SelectSitio eventSelectSitio;
    
    public static LeanSelectByFinger leanSelectByFinger;
    private LeanSelectable selectable;

    public ControlSitioUI controlSitioUI;

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
    private Coroutine corrutinaTime;
    public float diferencia;
    public float umbralGreen;
    public float umbralYellow;

    public Color statusColor;
    //-- 1 - green
    //-- 2 - yellow
    //-- 3 - red
    public int statusDataInTime;
    
    public Color statusColor1;
    public Color statusColor2;
    public Color statusColor3;
    
    public List<GameObject> MarcaSeleccionado;
    public List<GameObject> MarcaNoSeleccionado;
    
    public bool selectedSitio;

    public List<MeshRenderer> Bombas;

    public event Action<bool> sitioSeleccionadoEvent;
    
    public ControlSelectSitio controlSelectSitio;
    public GameObject billboardObj;
    public Vector3 billboardSelectedPos;
    public Vector3 billboardUnSelectedPos;
    
    public ControlMarcadorUI controlMarcadorUI;
    
    public bool dataInTime = false;
    //public int indexBomba = 0;

    public float timeToDobleClick;
    
    private void Start()
    {
        this.corrutinaTime = StartCoroutine(StatusUI());
        
        DeselectMe();

        //textoIdSitioUnity.text = $"{MyDataSitio.idSitioUnity}";
    }

    public virtual IEnumerator StatusUI()
    {
        yield return new WaitForSeconds(updateRate);
    }
    
    public void SetColorBombaMap2D(int color)
    {
        // 0 - gris
        // 1 - verde
        // 2 - rojo
        // 3 - Azul
        if (controlMarcadorUI != null)
            controlMarcadorUI.SetColorBomba(color);
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
        if (ControlUpdateUI._singletonExists)
            ControlUpdateUI.singleton.SetSelectedSitio(sitio);
        
        eventSelectSitio.Raise(this);

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
    public void SelectMe()
    {
        foreach (var go in MarcaSeleccionado)
        {
            go.SetActive(true);
        }
        
        foreach (var go in MarcaNoSeleccionado)
        {
            go.SetActive(false);
        }

        selectedSitio = true;
        sitioSeleccionadoEvent?.Invoke(selectedSitio);
        // if(billboardObj != null)
        //     billboardObj.transform.localPosition = billboardSelectedPos;
    }
    
    public void DeselectMe()
    {
        // foreach (var selectable in GetComponentsInChildren<ChangeVisualSelectable>())
        // {
        //     selectable.SetDefaultColor();
        // }
        
        foreach (var go in MarcaSeleccionado)
        {
            go.SetActive(false);
        }
        
        foreach (var go in MarcaNoSeleccionado)
        {
            go.SetActive(true);
        }
        
        selectedSitio = false;
        sitioSeleccionadoEvent?.Invoke(selectedSitio);
        // if(billboardObj != null)
        //     billboardObj.transform.localPosition = billboardUnSelectedPos;

    }

    public virtual void SetDataSitio(DataSitio _DataSitio) { }

    public void CreateSphere()
    {
        if(!debugSphere)
            return;
        
        var go = Instantiate(sphereDebug,colliderDebug.transform.position, Quaternion.identity);
        go.SetActive(true);
    }
    
    // public void SetActiveMarcadaorSitio(bool _val)
    // {
    //     if (cantrolMarcadorUI != null)
    //         cantrolMarcadorUI.gameObject.SetActive(_val);
    //     
    //     this.gameObject.SetActive(_val);
    // }
    
    public bool GetSitioSelectedForAnalitics()
    {
        if (controlSelectSitio != null)
            return controlSelectSitio.selectedForAnalitics;

        return false;
    }

    // public float GetGastoSitio()
    // {
    //     if (MyDataSitio.gasto.Count > 0)
    //     {
    //         if (MyDataSitio.gasto[0].DentroRango)
    //             return MyDataSitio.gasto[0].Valor;
    //     }
    //
    //     return 0;
    // }
    
    // public float GetPresionSitio()
    // {
    //     if (MyDataSitio.presion.Count > 0)
    //     {
    //         if (MyDataSitio.presion[0].DentroRango)
    //             return MyDataSitio.presion[0].Valor;
    //     }
    //
    //     return 0;
    // }
    
    // public float GetTotalizadoSitio()
    // {
    //     if (MyDataSitio.totalizado.Count > 0)
    //     {
    //         if (MyDataSitio.totalizado[0].DentroRango)
    //             return MyDataSitio.totalizado[0].Valor;
    //     }
    //
    //     return 0;
    // }
    
    public IEnumerator CountDownDobleClick()
    {
        while (timeToDobleClick > 0)
        {
            timeToDobleClick -= Time.deltaTime;
            yield return null;
        }
    }
}
