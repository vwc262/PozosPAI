using UnityEngine;

public class ControlPantallaResumen : MonoBehaviour
{
    public bool GetShowResumen()
    {
        if (ControlAccesoPozosPAI._singletonExists)
            return ControlAccesoPozosPAI.singleton.configuration.showResumenInit;

        return true;
    }
}
