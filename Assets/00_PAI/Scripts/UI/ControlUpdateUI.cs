using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ControlUpdateUI : Singleton<ControlUpdateUI>
{
    public UnityEvent<ControlSitio> SitioSeleccionado;
    
    public UnityEvent<ControlMarcadorSitio> SitioSeleccionadoSitioGPS;
    
    public UnityEvent<ControlSitio> SitioDeseleccionado;
    
    public UnityEvent<ControlSitio> ChangeIndexBomba;
    
    [FormerlySerializedAs("sitioSeleccionado")] public ControlMarcadorSitio controlMarcadorSitioSeleccionado;
    
    public void SetSelectedSitio(ControlSitio _DataSitio)
    {
        SitioSeleccionado.Invoke(_DataSitio);
    }
    
    public void SetSitioSeleccionado(ControlMarcadorSitio controlMarcadorSitio)
    {
        controlMarcadorSitioSeleccionado = controlMarcadorSitio;
        //SitioSeleccionadoSitioGPS.Invoke(controlMarcadorSitioSeleccionado);
    }

    public void deseleccionarSitio()
    {
        if (controlMarcadorSitioSeleccionado != null)
        {
            controlMarcadorSitioSeleccionado.DeselectMe();
            
            //SitioDeseleccionado.Invoke(controlMarcadorSitioSeleccionado);
            
            controlMarcadorSitioSeleccionado = null;
        }
    }
}
