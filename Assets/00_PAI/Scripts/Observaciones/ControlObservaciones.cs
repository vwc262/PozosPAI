using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlObservaciones : MonoBehaviour
{
    public ControlSitio sitio;
    
    public float updateRate = 5;
    private float countdown;
    
    public TMPro.TMP_Text textObservaciones;
    public TMPro.TMP_InputField inputFieldObservaciones;
    public GameObject buttonEdit;
    public GameObject buttonSave;

    public bool useLocalData;
    public Observaciones observaciones;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ControlSelectedSitio._singletonExists)
        {
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);
            //ControlSelectedSitio.singleton.SitioDeseleccionado.AddListener(DeseleccionarSitioGPS);
        }

        observaciones.ReadJSON_DataFile();
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            UpdateUIObservaciones();
            countdown = updateRate;
        }
    }
    
    public void UpdateInfoSitio(ControlSitio _sitio)
    {
        DisableEditObservaciones();
        
        sitio = _sitio;
        
        UpdateUIObservaciones();
    }

    public void DeseleccionarSitioGPS(ControlSitio _sitio)
    {
        sitio = null;
    }

    public void UpdateUIObservaciones()
    {
        if (sitio != null)
        {
            if (textObservaciones != null)
            {
                if (useLocalData)
                {
                    Observacion observacion = observaciones.ListObservaciones.Find(
                        item => item.id == sitio.dataSitio.idSitio);
                    
                    if (textObservaciones != null && observacion != null)
                        textObservaciones.text = observacion.observacion;
                    else
                        textObservaciones.text = "";
                }
                else
                {
                    textObservaciones.text = sitio.dataSitio.observaciones;
                }
            }
        }
        else
        {
            if (textObservaciones != null)
            {
                textObservaciones.text = "";
            }
        }
    }

    public void EditarObservacion()
    {
        if (sitio != null)
        {
            EnableEditObservaciones();

            inputFieldObservaciones.text = textObservaciones.text;
        }
    }
    
    public void SaveObservacion()
    {
        if (sitio != null)
        {
            DisableEditObservaciones();
            
            Observacion observacionAux = new Observacion();

            observacionAux.id = sitio.dataSitio.idSitio;
            observacionAux.observacion = inputFieldObservaciones.text;
            
            int observacion = observaciones.ListObservaciones.FindIndex(
                item => item.id == sitio.dataSitio.idSitio);
            
            if (observacion >= 0)
                observaciones.ListObservaciones[observacion] = observacionAux;
            else
                observaciones.ListObservaciones.Add(observacionAux);
            
            observaciones.SaveJSON_DataFile();

            UpdateUIObservaciones();
        }
    }

    public void EnableEditObservaciones()
    {
        if (FlyCamera._singletonExists)
            FlyCamera.singleton.enableInputKeyboard = false;
        
        buttonEdit.SetActive(false);
        buttonSave.SetActive(true);
        inputFieldObservaciones.gameObject.SetActive(true);
    }

    public void DisableEditObservaciones()
    {
        if (FlyCamera._singletonExists)
            FlyCamera.singleton.enableInputKeyboard = true;
        
        buttonEdit.SetActive(true);
        buttonSave.SetActive(false);
        inputFieldObservaciones.gameObject.SetActive(false);
    }
}
