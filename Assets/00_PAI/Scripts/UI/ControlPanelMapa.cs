using System;
using UnityEngine;

public class ControlPanelMapa : MonoBehaviour
{
    public ControlMoveCameraMap MoveCamera;

    private void Start()
    {
        if (ControlMoveCamera._singletonExists)
            MoveCamera = ControlMoveCamera.singleton.moveCamera;
    }

    public void SetHome()
    {
        if (MoveCamera != null)
            MoveCamera.GoHome();
        
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.DeseleccionarSitio();
        
        if (ControlCarrousel._singletonExists)
            ControlCarrousel.singleton.SendEventFSM("hide");
        
        if (ControlBombas_PAI._singletonExists)
            ((ControlBombas_PAI)ControlBombas_PAI.singleton).SendEventFSM("hide");
        
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.CloseLoginPanel();
    }

    public void OpenGraficadorWeb()
    {
        if (ControlManager._singletonExists)
            ControlManager.singleton.OpenGraficadorWeb();
    }
}
