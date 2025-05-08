using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlUIPanelTitleSitio : MonoBehaviour
{
    public ControlSitio sitio;
    
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
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);
    }
    
    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            if (sitio != null)
                UpdateStatusSitio(sitio);
            countdown = updateRate;
        }
    }

    public void UpdateInfoSitio(ControlSitio _sitio)
    {
        sitio = _sitio;
        UpdateStatusSitio(sitio);
    }
    
    public void UpdateStatusSitio(ControlSitio _sitio)
    {
        if (Nombre != null)
            Nombre.text = _sitio.dataSitio.nombre;
        
        if (Fecha != null)
            Fecha.text = ControlDateTime_PAI.GetDateFormat_DMAH(_sitio.dataSitio.fecha);

        if (statusImage != null)
        {
            if (_sitio.dataInTime)
                statusImage.sprite = imageStatusConectado;
            else
                statusImage.sprite = imageStatusNoConectado;
        }
    }
}
