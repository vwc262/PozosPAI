using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlSitiosUI_Lista : Singleton<ControlSitiosUI_Lista>
{
    [TabGroup("SitiosUI")] public List<ControlUISitio> sitios = new List<ControlUISitio>();

    public float updateRate = 5;
    private float countdown;
    
    public PlayMakerFSM controlPanelSitios;
    public SitiosOrdenados sitiosOrdenados;

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
    
    public void ToggleListSitios()
    {
        if (controlPanelSitios != null)
        {
            if (controlPanelSitios.FsmVariables.GetFsmBool("isIn").Value)
            {
                //Debug.Log("hide");
                controlPanelSitios.SendEvent("hide");
            }
            else
            {
                //Debug.Log("show");
                controlPanelSitios.SendEvent("show");
            }
        }
    }
    
    public virtual void UpdateData() { }
    
    public virtual void SetSitioSelectUI_Prefab(ControlSitio sitio) { }

    public virtual void SetSitioSelectUI_GO(ControlSitio sitio) { }

    public void DeleteSitios()
    {
        foreach (var sitio in sitios)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(sitio.gameObject);
            }
            else
            {
                Destroy(sitio.gameObject);
            }
        }
        
        sitios.Clear();
    }
    
    public virtual void SetSitiosEnd()
    {
        
    }
}
