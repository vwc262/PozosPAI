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

        if (ControlSelectedSitio._singletonExists)
        {
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);
        }
    }

    public void UpdateInfoSitio(ControlSitio sitio)
    {
        if (sitio != null)
        {
            if (!(sitio.dataSitio.idSitio == 1421)) //Diferente de Barrientos
            {
                SendEventFSM("show");
                SetSelectedSitioGPS(sitio);
            }
            else
            {
                SendEventFSM("hide");
            }
            
        }
    }

    public void SendEventFSM(string eventName)
    {
        if (FSMControlCarrousel != null)
            FSMControlCarrousel.SendEvent(eventName);
    }

    public void SetSelectedSitioGPS(ControlSitio sitio)
    {
        switch ((EstructurasAPI.Proyectos)sitio.dataSitio.Estructura)
        {
            case EstructurasAPI.Proyectos.Teoloyucan:
                if (ControlBombaButton != null)
                    ControlBombaButton.gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                        ControlAccesoPozosPAI.Proyectos.Teoloyucan));
                break;
            case EstructurasAPI.Proyectos.PozosZumpango:
                if (ControlBombaButton != null)
                    ControlBombaButton.gameObject.SetActive(ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                        ControlAccesoPozosPAI.Proyectos.PozosZumpango));
                break;
            case EstructurasAPI.Proyectos.PozosAIFA:
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
