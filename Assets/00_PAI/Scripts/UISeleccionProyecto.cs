using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISeleccionProyecto : MonoBehaviour
{
    public TMPro.TMP_Dropdown proyectosDropdown;
    public PlayMakerFSM sceneChangerFSM;

    private void Start()
    {
        if (proyectosDropdown != null)
        {
            proyectosDropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> optios = new List<TMP_Dropdown.OptionData>();

            foreach (var proyecto in Enum.GetValues(typeof(EstructurasAPI.Proyectos)))
            {
                optios.Add(new TMP_Dropdown.OptionData(proyecto.ToString()));
            }
            
            proyectosDropdown.AddOptions(optios);
            
            if (ControlConfiguration._singletonExists)
                proyectosDropdown.value = (int)ControlConfiguration.singleton.proyecto;
        }
    }

    public void SetProyecto(int proyecto)
    {
        if (ControlConfiguration._singletonExists)
            ControlConfiguration.singleton.SetProyecto((EstructurasAPI.Proyectos)proyecto);
    }

    public void SendEventFSM(string eventName)
    {
        if (sceneChangerFSM != null)
            sceneChangerFSM.SendEvent(eventName);
    }
}
