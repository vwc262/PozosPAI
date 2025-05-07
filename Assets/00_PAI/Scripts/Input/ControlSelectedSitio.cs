using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlSelectedSitio : MonoBehaviour
{
    [FormerlySerializedAs("sitioSelected")] public ControlMarcadorSitio controlMarcadorSitioSelected;

    public void SetSelectedSitio(ControlMarcadorSitio controlMarcadorSitio)
    {
        if (controlMarcadorSitioSelected != null)
            controlMarcadorSitioSelected.DeselectMe();
        
        controlMarcadorSitioSelected = controlMarcadorSitio;
        
        controlMarcadorSitioSelected.SelectMe();
        
        if (ControlBombas_PAI._singletonExists)
            ((ControlBombas_PAI)ControlBombas_PAI.singleton).SendEventFSM("hide");
        
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.CloseLoginPanel();
    }
}
