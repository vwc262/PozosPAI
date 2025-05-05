using UnityEngine;

public class ControlSun : Singleton<ControlSun>
{
    public PlayMakerFSM SunFSM;
    
    public void SendSunFSMEvent(string eventName)
    {
        if (SunFSM != null)
            SunFSM.SendEvent(eventName);
    }
}
