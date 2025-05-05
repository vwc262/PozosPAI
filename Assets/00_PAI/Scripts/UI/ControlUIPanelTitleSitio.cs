using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUIPanelTitleSitio : MonoBehaviour
{
    public SitioGPS sitioSeleccionado;
    
    public TMPro.TMP_Text Nombre;
    public TMPro.TMP_Text Fecha;
    
    public UnityEngine.UI.Image statusImage;
    public Sprite imageStatusConectado;
    public Sprite imageStatusNoConectado;
    
    public float updateRate = 5;
    private float countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        if (ControlUpdateUI._singletonExists)
            ControlUpdateUI.singleton.SitioSeleccionadoSitioGPS.AddListener(UpdateInfoSitio);
    }
    
    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            if (sitioSeleccionado != null)
                UpdateStatusSitio(sitioSeleccionado);
            countdown = updateRate;
        }
    }

    public void UpdateInfoSitio(SitioGPS _Sitio)
    {
        sitioSeleccionado = _Sitio;
        UpdateStatusSitio(sitioSeleccionado);
    }
    
    public void UpdateStatusSitio(SitioGPS _Sitio)
    {
        if (Nombre != null)
            Nombre.text = _Sitio.MyDataSitio.nombre;
        
        if (Fecha != null)
            Fecha.text = ControlDateTime.GetDateFormat_DMAH(_Sitio.MyDataSitio.fecha);

        if (statusImage != null)
        {
            if (_Sitio.statusDataInTime == 1)
                statusImage.sprite = imageStatusConectado;
            else
                statusImage.sprite = imageStatusNoConectado;
        }
    }
}
