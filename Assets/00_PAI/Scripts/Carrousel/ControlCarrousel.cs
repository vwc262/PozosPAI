using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlCarrousel : Singleton<ControlCarrousel>
{
    public PlayMakerFSM FSMControlCarrousel;

    public Button ControlBombaButton;

    private void Start()
    {
        FSMControlCarrousel = GetComponent<PlayMakerFSM>();
    }

    public void SendEventFSM(string eventName)
    {
        if (FSMControlCarrousel != null)
            FSMControlCarrousel.SendEvent(eventName);
    }

    public void SetSelectedSitioGPS(SitioGPS sitioGPS)
    {
        switch ((RequestAPI.Proyectos)sitioGPS.MyDataSitio.Estructura)
        {
            case RequestAPI.Proyectos.Teoloyucan:
                if (ControlBombaButton != null)
                    ControlBombaButton.gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                        ControlAccesoPozosPAI.Proyectos.Teoloyucan));
                break;
            case RequestAPI.Proyectos.PozosZumpango:
                if (ControlBombaButton != null)
                    ControlBombaButton.gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                        ControlAccesoPozosPAI.Proyectos.PozosZumpango));
                break;
            case RequestAPI.Proyectos.PozosReyesFerrocarril:
                if (ControlBombaButton != null)
                    ControlBombaButton.gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                        ControlAccesoPozosPAI.Proyectos.PozosReyesFerrocarril));
                break;
            case RequestAPI.Proyectos.PozosAIFA:
                if (ControlBombaButton != null)
                    ControlBombaButton.gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                        ControlAccesoPozosPAI.Proyectos.PozosAIFA));
                break;
        }
    }

    public void CloseControlCarrousel()
    {
        if (ControlBombas_PAI._singletonExists)
            ((ControlBombas_PAI)ControlBombas_PAI.singleton).SendEventFSM("hide");
        
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.CloseLoginPanel();
    }
}
