using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ControlSelectedSitio : Singleton<ControlSelectedSitio>
{
    public ControlSitio SitioSeleccionado;
    public ControlSitio SitioSeleccionadoDobleClick;
    
    public UnityEvent<ControlSitio> ChangeSitioSeleccionado;
    public UnityEvent<ControlSitio> ChangeSitioSeleccionadoDobleClick;
    
    public UnityEvent<ControlSitio> ChangeIndexBomba;

    public void SetSelectedSitio(ControlSitio sitio)
    {
        //Deseleccionar sitio
        if (SitioSeleccionado != null)
            SitioSeleccionado.DeseleccionarSitio();
        
        //Seleccionar sitio
        this.SitioSeleccionado = sitio;
        
        SitioSeleccionado.SeleccionarSitio();
        
        ChangeSitioSeleccionado.Invoke(SitioSeleccionado);
    }
    
    public void SetSelectedSitioDobleClick(ControlSitio sitio)
    {
        this.SitioSeleccionadoDobleClick = sitio;
        ChangeSitioSeleccionadoDobleClick.Invoke(SitioSeleccionadoDobleClick);
    }
    
    public void DeseleccionarSitio()
    {
        //Deseleccionar sitio
        if (SitioSeleccionado != null)
            SitioSeleccionado.DeseleccionarSitio();
        
        SitioSeleccionado = null;
        
        ChangeSitioSeleccionado.Invoke(null);
    }
}
