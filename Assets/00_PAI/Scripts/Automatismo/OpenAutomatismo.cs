using UnityEngine;

public class OpenAutomatismo : MonoBehaviour
{

    public void OpenAutomatismoToggle()
    {
        ControlAutomation.singleton.TogglePanelAutomation();
    }
}
