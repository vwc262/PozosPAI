using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ControlManager : Singleton<ControlManager>
{
    public PlayMakerFSM mainFSM;

    public bool useCenterSitios;
    
    public string url = "https://www.google.com.mx"; 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (ControlRequest._singletonExists)
        {
            ControlRequest.singleton.infraestructuraActualizada.AddListener(() =>
            {
                SendEventMainFSM("actualizarInfraestructura");
                StartCoroutine(ActualizarInfraestructura());
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendEventMainFSM(string eventName)
    {
        if (mainFSM != null)
            mainFSM.SendEvent(eventName);
    }

    public IEnumerator ActualizarInfraestructura()
    {
        if (ControlDatos._singletonExists)
            ControlDatos.singleton.ActualizarInfraestructura();

        yield return new WaitForSeconds(0.1f);
        
        SendEventMainFSM("infraestructuraActualizada");
    }

    public void InicioProyecto()
    {
        // if (ControlConfiguration._singletonExists && ControlRequest._singletonExists)
        // {
        //     RequestAPI.singleton.sistema = ControlConfiguration.singleton.proyecto;
        // }
    }

    public void InicioRequest()
    {
        if (ControlRequest._singletonExists)
            ControlRequest.singleton.InitPoleo();
        
        if (RequestAPI_Auto._singletonExists)
            RequestAPI_Auto.singleton.IniciarPoleo();
    }

    public void InicioDatos()
    {
        if (ControlDatos._singletonExists)
            ControlDatos.singleton.IniciarUpdateData();
    }
    public void InicioParticulares() {}

    public void InicioMapa()
    {
        if (ControlDatos._singletonExists)
            ControlDatos.singleton.SetGlobalDataSitios();

        if (ControlMap._singletonExists)
        {
            ControlMap.singleton.SetGlobalDataMapa(useCenterSitios);
            
            if (useCenterSitios)
                ControlMap.singleton.SetPositionMapa();
        }
        
        if (ControlPipes._singletonExists)
            ControlPipes.singleton.SetPositionPipes();

        if (ControlMoveCamera._singletonExists)
        {
            ControlMoveCamera.singleton.SetLimitsCameraMovement();

            if (!useCenterSitios)
                ControlMoveCamera.singleton.ResetCenterCamera();
            else
                ControlMoveCamera.singleton.ResetCamera();
        }
    }

    public void InicioMarcadores3D()
    {
        if (ControlDatos._singletonExists)
            ControlDatos.singleton.CreateMaracadoresSitioMap();
    }

    public void InicioLista()
    {
        if (ControlDatos._singletonExists)
            ControlDatos.singleton.RecreateUIListSitios();
    }
    
    public void OpenGraficadorWeb()
    {
        //Application.OpenURL(url);
        //Process.Start("chrome.exe", "--kiosk " + url);
        Process.Start("chrome.exe", "--app=\"" + url + "\" --start-maximized ");
    }
}
