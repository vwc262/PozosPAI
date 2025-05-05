using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ControlUpdateUI : Singleton<ControlUpdateUI>
{
    public UnityEvent<DataSitio> SitioSeleccionado;
    public UnityEvent<SitioGPS> SitioSeleccionadoSitioGPS;
    
    public UnityEvent<SitioGPS> SitioDeseleccionado;
    
    public UnityEvent<SitioGPS> ChangeIndexBomba;
    
    public SitioGPS sitioSeleccionado;
    
    public void SetSelectedSitio(DataSitio _DataSitio)
    {
        SitioSeleccionado.Invoke(_DataSitio);
    }
    
    public void SetSitioSeleccionado(SitioGPS _sitio)
    {
        sitioSeleccionado = _sitio;
        SitioSeleccionadoSitioGPS.Invoke(sitioSeleccionado);
    }

    public void deseleccionarSitio()
    {
        if (sitioSeleccionado != null)
        {
            sitioSeleccionado.DeselectMe();
            
            SitioDeseleccionado.Invoke(sitioSeleccionado);
            
            sitioSeleccionado = null;
        }
    }
}
