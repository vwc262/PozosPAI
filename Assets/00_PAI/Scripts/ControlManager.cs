using System.Collections;
using UnityEngine;

public class ControlManager : Singleton<ControlManager>
{
    public PlayMakerFSM mainFSM;

    public bool useCenterSitios;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RequestAPI._singletonExists)
        {
            RequestAPI.singleton.infraestructuraActualizada.AddListener(() =>
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
    
    public void InicioProyecto() {}

    public void InicioRequest()
    {
        if (RequestAPI._singletonExists)
            RequestAPI.singleton.IniciarPoleo();
        
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

        if (VWC_MoveCamera_PAI._singletonExists)
        {
            VWC_MoveCamera_PAI.singleton.SetLimitsCameraMovement();

            if (!useCenterSitios)
                VWC_MoveCamera_PAI.singleton.ResetCenterCamera();
            else
                VWC_MoveCamera_PAI.singleton.ResetCamera();
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
}
