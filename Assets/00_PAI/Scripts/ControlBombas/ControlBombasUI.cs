using UnityEngine;
using UnityEngine.Serialization;

public class ControlBombasUI : MonoBehaviour
{
    public float updateRate = 5;
    public ControlSitio selectedControlSitio;
    
    private float countdown;
    
    public void Start()
    {
        if (ControlSelectedSitio._singletonExists)
        {
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);
            ControlSelectedSitio.singleton.ChangeIndexBomba.AddListener(UpdateUISitio);
        }
    }
    
    public void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            UpdateUISitio(selectedControlSitio);
            countdown = updateRate;
        }            
    }

    public void UpdateInfoSitio(ControlSitio sitio)
    {
        selectedControlSitio = sitio;
        UpdateUISitio(selectedControlSitio);
    }
    
    public virtual void UpdateUISitio(ControlSitio controlMarcadorSitio)
    {
        
    }
}
