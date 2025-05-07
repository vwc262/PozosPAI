using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlUIPanelTitleSitio : MonoBehaviour
{
    public ControlMarcadorSitio controlMarcadorSitioSeleccionado;
    
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
            if (controlMarcadorSitioSeleccionado != null)
                UpdateStatusSitio(controlMarcadorSitioSeleccionado);
            countdown = updateRate;
        }
    }

    public void UpdateInfoSitio(ControlMarcadorSitio controlMarcadorSitio)
    {
        controlMarcadorSitioSeleccionado = controlMarcadorSitio;
        UpdateStatusSitio(controlMarcadorSitioSeleccionado);
    }
    
    public void UpdateStatusSitio(ControlMarcadorSitio controlMarcadorSitio)
    {
        if (Nombre != null)
            Nombre.text = controlMarcadorSitio.sitio.dataSitio.nombre;
        
        if (Fecha != null)
            Fecha.text = ControlDateTime_PAI.GetDateFormat_DMAH(controlMarcadorSitio.sitio.dataSitio.fecha);

        if (statusImage != null)
        {
            if (controlMarcadorSitio.statusDataInTime == 1)
                statusImage.sprite = imageStatusConectado;
            else
                statusImage.sprite = imageStatusNoConectado;
        }
    }
}
