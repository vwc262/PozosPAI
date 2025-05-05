using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlSelectedSitio : MonoBehaviour
{
    public SitioGPS sitioSelected;

    public void SetSelectedSitio(SitioGPS _sitio)
    {
        if (sitioSelected != null)
            sitioSelected.DeselectMe();
        
        sitioSelected = _sitio;
        
        sitioSelected.SelectMe();
        
        if (ControlBombas_PAI._singletonExists)
            ((ControlBombas_PAI)ControlBombas_PAI.singleton).SendEventFSM("hide");
        
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.CloseLoginPanel();
    }
}
