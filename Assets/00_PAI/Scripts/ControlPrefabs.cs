using Sirenix.OdinInspector;
using UnityEngine;

public class ControlPrefabs : Singleton<ControlPrefabs>
{
    [TabGroup("Prefabs")]public GameObject prefabMarcadorSitio;
    [TabGroup("Prefabs")]public GameObject prefabMarcadorRepetidor;
    [TabGroup("Prefabs")]public GameObject prefabMarcadorSitioEnConstruccion;
    [TabGroup("Prefabs")]public GameObject prefabPanelUISitio;
    [TabGroup("Prefabs")]public GameObject prefabPanelUIRepetidor;
    [TabGroup("Prefabs")]public GameObject prefabPanelUISitioEnConstruccion;
    [TabGroup("Prefabs")]public GameObject prefabUIRegionaLabel;
    [TabGroup("Prefabs")]public GameObject prefabUIRegionaList;
}
