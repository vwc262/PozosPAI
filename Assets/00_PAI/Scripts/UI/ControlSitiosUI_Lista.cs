using UnityEngine;

public class ControlSitiosUI_Lista : MonoBehaviour
{
    public PlayMakerFSM controlPanelSitios;
    
    public void SendGUIFSMEvent(string eventName)
    {
        if (BoyRoboticsAnimationManager._singletonExists)
            BoyRoboticsAnimationManager.singleton.SendGUIFSMEvent(eventName);
    }
    
    public void SendCameraFSMEvent(string eventName)
    {
        if (BoyRoboticsAnimationManager._singletonExists)
            BoyRoboticsAnimationManager.singleton.SendCameraFSMEvent(eventName);
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
}
