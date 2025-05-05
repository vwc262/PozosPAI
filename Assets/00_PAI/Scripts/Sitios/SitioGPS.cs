using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Common;
using Lean.Touch;
using Raskulls.ScriptableSystem;
using UnityEngine;
using UnityEngine.Serialization;

public class SitioGPS : MonoBehaviour
{
    public DataSitio MyDataSitio;
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
    public int indexBomba = 0;

    public float timeToDobleClick;
    public float timeLastCommand;
    
    private void Start()
    {
        this.corrutinaTime = StartCoroutine(StatusUI());
        
        DeselectMe();

        //textoIdSitioUnity.text = $"{MyDataSitio.idSitioUnity}";

        timeLastCommand = Time.time-60;
    }

    public virtual IEnumerator StatusUI()
    {
        yield return new WaitForSeconds(updateRate);
    }

    //public IEnumerator StatusUI()
    // {
    //     if (rendererUIStatus.Count > 0)
    //     {
    //         while (true)
    //         {
    //             DateTime parsedDate;
    //
    //             if (DateTime.TryParse(MyDataSitio.fecha, out parsedDate))
    //             {
    //                 diferencia = (float)(DateTime.Now - parsedDate).TotalMinutes;
    //
    //                 if (diferencia < umbralGreen)
    //                 {
    //                     dataInTime = true;
    //                     statusColor = statusColor1;
    //                     statusDataInTime = 1;
    //                 }
    //                 // else if (diferencia < umbralYellow)
    //                 // {
    //                 //     dataInTime = false;
    //                 //     statusColor = statusColor2;
    //                 //     statusDataInTime = 2;
    //                 // }
    //                 else
    //                 {
    //                     dataInTime = false;
    //                     statusColor = statusColor3;
    //                     statusDataInTime = 3;
    //                 }
    //
    //                 if (MyDataSitio.idSitio == 16 || MyDataSitio.idSitio == 30)
    //                 {
    //                     SetColorMeshBombas(Color.green);
    //                     SetColorBombaMap2D(1);
    //                 }
    //                 else if (MyDataSitio.bomba.Count > 0)
    //                 {
    //                     // if (MyDataSitio.bomba[indexBomba].DentroRango)
    //                     // {
    //                         switch (MyDataSitio.bomba[indexBomba].Valor)
    //                         {
    //                             case 1:
    //                                 SetColorMeshBombas(Color.green);
    //                                 SetColorBombaMap2D(1);
    //                                 break;
    //                             case 2:
    //                                 SetColorMeshBombas(Color.red);
    //                                 SetColorBombaMap2D(2);
    //                                 break;
    //                             case 3:
    //                                 SetColorMeshBombas(Color.blue);
    //                                 SetColorBombaMap2D(3);
    //                                 break;
    //                         }
    //                     // }
    //                     // else
    //                     // {
    //                     //     SetColorMeshBombas(Color.gray);
    //                     //     SetColorBombaMap2D(0);
    //                     // }
    //                 }
    //                 else
    //                 {
    //                     SetColorMeshBombas(Color.gray);
    //                     SetColorBombaMap2D(0);
    //                 }
    //                 
    //                 rendererUIStatus.ForEach(item => item.color = statusColor);
    //                 textUIStatus.ForEach(item => item.color = statusColor);
    //             }
    //             else
    //             {
    //                 Debug.Log("Invalid date format");
    //             }
    //             
    //             if (MyDataSitio.idSitio == 15)
    //             {
    //                 if (MyDataSitio.nivel.Count > 0)
    //                 {
    //                     foreach (var render in rendererNivel)
    //                     {
    //                         render.gameObject.SetActive(false);
    //                     }
    //                     
    //                     if (MyDataSitio.nivel[0].DentroRango)
    //                     {
    //                         switch (MyDataSitio.nivel[0].IndiceImagen)
    //                         {
    //                             case 0:
    //                             case 1:
    //                                 rendererNivel[1].gameObject.SetActive(true);
    //                                 break;
    //
    //                             case 2:
    //                             case 3:
    //                                 rendererNivel[2].gameObject.SetActive(true);
    //                                 break;
    //
    //                             case 4:
    //                             case 5:
    //                                 rendererNivel[3].gameObject.SetActive(true);
    //                                 break;
    //
    //                             case 6:
    //                             case 7:
    //                                 rendererNivel[4].gameObject.SetActive(true);
    //                                 break;
    //
    //                             default:
    //                                 rendererNivel[5].gameObject.SetActive(true);
    //                                 break;
    //                         }
    //                     }
    //                     else
    //                     {
    //                         rendererNivel[0].gameObject.SetActive(true);
    //                     }
    //                 } 
    //             }
    //
    //             yield return new WaitForSeconds(updateRate);
    //         }
    //     }
    // }
    
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
            ControlUpdateUI.singleton.SetSelectedSitio(MyDataSitio);
        
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

    public float GetGastoSitio()
    {
        if (MyDataSitio.gasto.Count > 0)
        {
            if (MyDataSitio.gasto[0].DentroRango)
                return MyDataSitio.gasto[0].Valor;
        }

        return 0;
    }
    
    public float GetPresionSitio()
    {
        if (MyDataSitio.presion.Count > 0)
        {
            if (MyDataSitio.presion[0].DentroRango)
                return MyDataSitio.presion[0].Valor;
        }

        return 0;
    }
    
    public float GetTotalizadoSitio()
    {
        if (MyDataSitio.totalizado.Count > 0)
        {
            if (MyDataSitio.totalizado[0].DentroRango)
                return MyDataSitio.totalizado[0].Valor;
        }

        return 0;
    }

    public void incrementIndexBomba()
    {
        indexBomba++;
        
        if (indexBomba >= MyDataSitio.bomba.Count)
            indexBomba = 0;
        
        if (ControlUpdateUI._singletonExists)
            ControlUpdateUI.singleton.ChangeIndexBomba.Invoke(this);
    }
    
    public void SetIndexBomba(int index)
    {
        indexBomba = index;
        
        if (indexBomba >= MyDataSitio.bomba.Count)
            indexBomba = 0;
        
        if (ControlUpdateUI._singletonExists)
            ControlUpdateUI.singleton.ChangeIndexBomba.Invoke(this);
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
