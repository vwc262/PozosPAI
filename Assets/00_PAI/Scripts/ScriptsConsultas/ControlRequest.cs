using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class ControlRequest : Singleton<ControlRequest>
{
    [TabGroup("Events")] public UnityEvent infraestructuraActualizada;
    [TabGroup("Events")] public UnityEvent InitializeVersionEvent;
    [TabGroup("Events")] public UnityEvent UpdateVersionEvent;
    
    public EstructurasAPI.Proyectos sistemaLogin = EstructurasAPI.Proyectos.SistemaCutzamala;
    
    public List<RequestAPI> listRequestAPI;

    public bool respInfraestructura;

    public void InitPoleo()
    {
        StartCoroutine(GetInfraestructura());
    }

    public IEnumerator GetInfraestructura()
    {
        respInfraestructura = false;
        
        foreach (var request in listRequestAPI)
        {
            request.IniciarPoleo();
        }
        
        yield return new WaitForSeconds(0.1f);

        while (listRequestAPI.Count(x => !x.respInfraestructura) > 0)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        respInfraestructura = true;
        infraestructuraActualizada.Invoke();
    }

    public string GetAddressByMethod(string method, EstructurasAPI.Proyectos sistema)
    {
        RequestAPI api = listRequestAPI.First(x => x.sistema == sistema);
        
        if (api != null)
            return api.GetAddressByMethod(method);
        
        return "";
    }

    public bool errorInfreastructuraHTML()
    {
        if (listRequestAPI.Count(x => x.errorInfraestructuraHTML) > 0)
            return true;
        return false;
    }
    
    public bool errorUpdateHTML()
    {
        if (listRequestAPI.Count(x => x.errorUpdateHTML) > 0)
            return true;
        return false;
    }

    public bool GetUpdateStatus()
    {
        if (listRequestAPI.Count(x => !x.respUpdateSites) > 0)
            return false;
        return true;
    }
}
