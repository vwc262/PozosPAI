using UnityEngine;

public class ControlBombasUI : MonoBehaviour
{
    public float updateRate = 5;
    public SitioGPS SelectedSitio;
    
    private float countdown;
    
    public void Start()
    {
        if (ControlUpdateUI._singletonExists)
        {
            ControlUpdateUI.singleton.SitioSeleccionadoSitioGPS.AddListener(UpdateInfoSitio);
            ControlUpdateUI.singleton.ChangeIndexBomba.AddListener(UpdateUISitio);
        }
    }
    
    public void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            UpdateUISitio(SelectedSitio);
            countdown = updateRate;
        }            
    }

    public void UpdateInfoSitio(SitioGPS _sitio)
    {
        SelectedSitio = _sitio;
        UpdateUISitio(SelectedSitio);
    }
    
    public virtual void UpdateUISitio(SitioGPS _sitio)
    {
        
    }
}
