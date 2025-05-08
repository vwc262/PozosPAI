using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ParticularManager : Singleton<ParticularManager>
{
    public ControlSitio sitio;

    public GameObject loadedScreen;
    public float loadTime;
    public bool loading;
    public bool unloading;
    
    public float updateRate = 5;
    private float countdown;
    
    [TabGroup("Particular")] public bool isParticularOpen;
    [TabGroup("Particular")] public string currentParticularSceneName;
    [TabGroup("Particular")] public GameObject[] disableObjects;
    [TabGroup("Particular")] public GameObject[] enableObjects;
    [TabGroup("Particular")] public bool[] previousStates;
    [TabGroup("Particular")] public List<sceneParticularInfo> sceneParticularInfos = new List<sceneParticularInfo>();

    [TabGroup("UI")] public TMPro.TMP_Text textParticularNombre;
    [TabGroup("UI")] public TMPro.TMP_Text textUltimaActualizacion;
    [TabGroup("UI")] public GameObject EstadoEnLinea;
    [TabGroup("UI")] public GameObject EstadoFueraDeLinea;
    
    [TabGroup("UI")] public List<GameObject> ControlBombaListGO;
    
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    
    public void Start()
    {
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);
        
        StartCoroutine(InitParticular());
    }

    public IEnumerator InitParticular()
    {
        yield return new WaitForSeconds(0.1f);
        
        for (int i = 0; i < enableObjects.Length; i++)
            enableObjects[i].SetActive(false);
    }
    
    private void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            UpdateUIParticular();
            countdown = updateRate;
        }            
    }
    
    public void UpdateInfoSitio(ControlSitio _sitio)
    {
        sitio = _sitio;
        
        if (isParticularOpen)
            ChangeParticularScene();
    }
    
    [TabGroup("Particular")] [Button]
    public void LoadParticularScene(string sceneName)
    {
        if (currentParticularSceneName == "")
        {
            CloseBombaControl();
            loading = false;
            loadedScreen.SetActive(true);
            StartCoroutine(waitLoading());
            currentParticularSceneName = sceneName;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            
            previousStates = new bool[disableObjects.Length];
            for (int i = 0; i < disableObjects.Length; i++)
            {
                previousStates[i] = disableObjects[i].activeSelf;
                disableObjects[i].SetActive(false);
            }
            for (int i = 0; i < enableObjects.Length; i++)
                enableObjects[i].SetActive(true);

            isParticularOpen = true;
        }
    }
    
    public void CloseBombaControl()
    {
        if (ControlCarrousel._singletonExists)
            ControlCarrousel.singleton.SendEventFSM("hide");
        
        if (ControlBombas_PAI._singletonExists)
            ((ControlBombas_PAI)ControlBombas_PAI.singleton).SendEventFSM("hide");
        
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.CloseLoginPanel();
    }
    
    IEnumerator waitLoading()
    {
        yield return new WaitForSeconds(loadTime);
        
        while (!loading)
        {
            yield return null;
        }
        
        loadedScreen.SetActive(false);
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
        loading = true;
    }

    void OnSceneUnloaded(Scene scene)
    {
        Debug.Log("OnSceneUnoaded: " + scene.name);
        unloading = true;
    }
    
    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    [TabGroup("Particular")] [Button]
    public void UnloadParticularScene()
    {
        if (currentParticularSceneName == "")
            return;

        CloseBombaControl();
            
        SceneManager.UnloadSceneAsync(currentParticularSceneName);
        currentParticularSceneName = "";

        for (int i = 0; i < disableObjects.Length; i++)
        {
            disableObjects[i].SetActive(previousStates[i]);
        }
        for (int i = 0; i < enableObjects.Length; i++)
            enableObjects[i].SetActive(false);
        
        isParticularOpen = false;
        
        loadedScreen.SetActive(true);
        StartCoroutine(waitUnLoading());
        unloading = false;
    }
    
    IEnumerator waitUnLoading()
    {
        yield return new WaitForSeconds(loadTime);
        
        while (!unloading)
        {
            yield return null;
        }
        
        loadedScreen.SetActive(false);
    }
    
    public void LoadParticularScene()
    {
        string sceneName = "";

        sceneParticularInfo aux = sceneParticularInfos
            .Find(item => item.id_sitio == sitio.dataSitio.idSitio);

        if (aux != null)
            sceneName = aux.nombreScene;
        
        if (sceneName != "")
        {
            LoadParticularScene(sceneName);
            SetUIParticular();
        }
    }

    public void ChangeParticularScene()
    {
        string sceneName = "";

        sceneParticularInfo aux = sceneParticularInfos
            .Find(item => item.id_sitio == sitio.dataSitio.idSitio);

        if (aux != null)
            sceneName = aux.nombreScene;

        if (sceneName != "" && currentParticularSceneName != sceneName)
        {
            loading = false;
            loadedScreen.SetActive(true);
            StartCoroutine(waitUnLoading());
            
            SceneManager.UnloadSceneAsync(currentParticularSceneName);
            
            currentParticularSceneName = sceneName;
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            
            SetUIParticular();
        }
    }

    public void SetUIParticular()
    {
        if (textParticularNombre != null)
            textParticularNombre.text = sitio.dataSitio.nombre;

        SetUIControlBomba();

        UpdateUIParticular();
    }

    public void SetUIControlBomba()
    {
        bool ControlBombaSitio = false;
        
        switch ((RequestAPI.Proyectos)sitio.dataSitio.Estructura)
        {
            case RequestAPI.Proyectos.Teoloyucan:
                ControlBombaSitio = ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                    ControlAccesoPozosPAI.Proyectos.Teoloyucan);
                break;

            case RequestAPI.Proyectos.PozosZumpango:
                ControlBombaSitio = ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                    ControlAccesoPozosPAI.Proyectos.PozosZumpango);
                break;

            // case RequestAPI.Proyectos.PozosReyesFerrocarril:
            //     ControlBombaSitio = ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
            //         ControlAccesoPozosPAI.Proyectos.PozosReyesFerrocarril);
            //     break;

            case RequestAPI.Proyectos.PozosAIFA:
                ControlBombaSitio = ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                    ControlAccesoPozosPAI.Proyectos.PozosAIFA);
                break;
        }

        foreach (var controlBombaGO in ControlBombaListGO)
        {
            controlBombaGO.SetActive(ControlBombaSitio);
        }
    }

    public void UpdateUIParticular()
    {
        if (sitio != null)
        {
            if (sitio.dataInTime)
            {
                if (EstadoEnLinea != null)
                    EstadoEnLinea.gameObject.SetActive(true);

                if (EstadoFueraDeLinea != null)
                    EstadoFueraDeLinea.gameObject.SetActive(false);
            }
            else
            {
                if (EstadoEnLinea != null)
                    EstadoEnLinea.gameObject.SetActive(false);

                if (EstadoFueraDeLinea != null)
                    EstadoFueraDeLinea.gameObject.SetActive(true);
            }

            if (textUltimaActualizacion != null)
                textUltimaActualizacion.text = "Última actualización: " + sitio.dataSitio.fecha;
        }
        
        
    }

    [Button]
    public void ResetDron()
    {
        //Debug.Log("Reset Dron");
        if (Particular_Reset_Pos._singletonExists)
            Particular_Reset_Pos.singleton.ResetPosition();
    }
}

[Serializable]
public class sceneParticularInfo
{
    public int id_sitio;
    public string nombreScene;
}
