using UnityEngine;

public class ControlPantallaResumen : MonoBehaviour
{
    public GameObject panelBarrientos;
    public GameObject panelNoBarrientos;
    public bool GetShowResumen()
    {
        if (ControlAccesoPozosPAI._singletonExists)
            return ControlAccesoPozosPAI.singleton.configuration.showResumenInit;

        return true;
    }

    public void GetShowCaudales()
    {
        if (panelBarrientos != null)
            panelBarrientos.SetActive(ControlAccesoPozosPAI.singleton.configuration.habilitarBarrientos);
        
        if (panelNoBarrientos != null)
            panelNoBarrientos.SetActive(!(ControlAccesoPozosPAI.singleton.configuration.habilitarBarrientos));
        
    }
}
