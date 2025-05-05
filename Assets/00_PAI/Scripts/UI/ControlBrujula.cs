using UnityEngine;

public class ControlBrujula : MonoBehaviour
{
    public VWC_MoveCamera MoveCamera;
    
    public void SetHome()
    {
        if (MoveCamera != null)
            MoveCamera.GoHome();
        
        if (ControlUpdateUI._singletonExists)
            ControlUpdateUI.singleton.deseleccionarSitio();
        
        if (ControlCarrousel._singletonExists)
            ControlCarrousel.singleton.SendEventFSM("hide");
        
        if (ControlBombas_PAI._singletonExists)
            ((ControlBombas_PAI)ControlBombas_PAI.singleton).SendEventFSM("hide");
        
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.CloseLoginPanel();
    }
}
