using Sirenix.OdinInspector;
using UnityEngine;

public class ControlPrefabs : Singleton<ControlPrefabs>
{
    public bool useGenericPrefab = true;
    
    [TabGroup("Prefabs")]public GameObject prefabMarcadorSitio;
    [TabGroup("Prefabs")]public GameObject prefabMarcadorPozo;
    [TabGroup("Prefabs")]public GameObject prefabMarcadorRepetidor;
    [TabGroup("Prefabs")]public GameObject prefabMarcadorSitioEnConstruccion;
    [TabGroup("Prefabs")]public GameObject prefabPanelUISitio;
    [TabGroup("Prefabs")]public GameObject prefabPanelUIPozo;
    [TabGroup("Prefabs")]public GameObject prefabPanelUIRepetidor;
    [TabGroup("Prefabs")]public GameObject prefabPanelUISitioEnConstruccion;
    [TabGroup("Prefabs")]public GameObject prefabUIRegionaLabel;
    [TabGroup("Prefabs")]public GameObject prefabUIRegionaList;
    
    public GameObject GetPrefabMarcadorSitio(TipoSitio tipoSitio)
    {
        if (useGenericPrefab)
            return prefabMarcadorSitio;
        
        switch (tipoSitio)
        {
            case TipoSitio.Pozo:
                return prefabMarcadorPozo;
            case TipoSitio.Repetidor:
                return prefabMarcadorRepetidor;
            case TipoSitio.EnConstruccion:
                return prefabMarcadorSitioEnConstruccion;
            default:
                return prefabMarcadorSitio;
        }
    }

    public GameObject GetPrefabUIListSitio(TipoSitio tipoSitio)
    {
        if (useGenericPrefab)
            return prefabPanelUISitio;
        
        switch (tipoSitio)
        {
            case TipoSitio.Pozo:
                return prefabPanelUIPozo;
            case TipoSitio.Repetidor:
                return prefabPanelUIRepetidor;
            case TipoSitio.EnConstruccion:
                return prefabPanelUISitioEnConstruccion;
            default:
                return prefabPanelUISitio; 
        }
    }
}
