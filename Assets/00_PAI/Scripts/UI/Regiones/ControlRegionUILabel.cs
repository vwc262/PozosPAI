using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ControlRegionUILabel : MonoBehaviour
{
    public SitiosOrdenados sitiosOrdenados;

    public Image foldButtonRegion;
    public int region;
    public int regionID;
    public bool IsOn;
    
    public GameObject RegionButtonCollapse;
    public Text TextNameRegional;
    public Text TextNoActRegional;
    public Text TextActRegional;
    
    public int coutNoActRegional;
    public int coutActRegional;

    public OnValueIsOnChange onValueIsOnChange;

    public void SetNameRegional(string _name)
    {
        if (TextNameRegional != null)
            TextNameRegional.text = _name;
    }

    public void ToggleRegion()
    {
        sitiosOrdenados.ToggleRegion((int)region - 1);
    }

    public void ToggleValueIsOn()
    {
        IsOn = !IsOn;
        
        onValueIsOnChange.Invoke(IsOn);
    }

    public void SetIsOn(bool val)
    {
        IsOn = val;
        onValueIsOnChange.Invoke(IsOn);
    }

    public void DobleClickRegion()
    {
        if (VWC_MoveCamera_PAI._singletonExists)
            VWC_MoveCamera_PAI.singleton.SetMoveCameraByRigionID(regionID);
    }
}

[System.Serializable]
public class OnValueIsOnChange : UnityEvent<bool>
{
}
