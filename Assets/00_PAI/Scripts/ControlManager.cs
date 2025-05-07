using System.Collections;
using UnityEngine;

public class ControlManager : Singleton<ControlManager>
{
    public PlayMakerFSM mainFSM;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RequestAPI.Instance != null)
        {
            RequestAPI.Instance.infraestructuraActualizada.AddListener(() =>
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

        yield return new WaitForSeconds(2f);
        
        SendEventMainFSM("infraestructuraActualizada");
    }
    
    public void InicioProyecto() {}

    public void InicioRequest()
    {
        if (RequestAPI.Instance != null)
            RequestAPI.Instance.IniciarPoleo();
        
        if (RequestAPI_Auto._singletonExists)
            RequestAPI_Auto.singleton.IniciarPoleo();
    }

    public void InicioDatos()
    {
        if (ControlDatos._singletonExists)
            ControlDatos.singleton.IniciarUpdateData();
    }
    public void InicioParticulares() {}
    public void InicioMapa() {}
    public void InicioMarcadores3D() {}
    public void InicioLista() {}
}
