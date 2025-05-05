using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControlDebug : MonoBehaviour
{
    public KeyCode debugKey;
    public bool debugEnabled;
    
    public GameObject debugPanel;
    public GameObject proxy;

    //public DisableTerrainMeshCollider terrainColliders;
    
    public Text fpsText;
    public float deltaTime;

    public bool isActiveRenderMap;
    public GameObject RenderMap;
    
    public bool isActiveRenderLupa;
    public GameObject RenderLupa;
    //public GameObject propsRoot;
    
    private void Start()
    {
        if (debugPanel != null)
            debugPanel.SetActive(debugEnabled);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(debugKey))
        {
            ToggleShowPanel();
        }

        if (debugEnabled)
        {
            showFPS();
        }
    }

    public void ToggleShowPanel()
    {
        debugEnabled = !debugEnabled;
        if (debugPanel != null)
            debugPanel.SetActive(debugEnabled);
    }

    public void showFPS()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        fpsText.text = Mathf.Ceil (fps).ToString ();
    }

    [Button]
    public void SetActiveProxy(bool _active)
    {
        if (proxy != null)
            proxy.SetActive(_active);
    }
    
    [Button]
    public void ChangeActiveProxy()
    {
        if (proxy != null)
            proxy.SetActive(!proxy.activeSelf);
    }

    [Button]
    // public void ChangeActiveTerrainColliders()
    // {
    //     if (ControlMap._singletonExists)
    //         ControlMap.singleton.ChangeActiveColliderMap();
    // }

    public void ToggleRenderMap()
    {
        isActiveRenderMap = !isActiveRenderMap;
        
        if (RenderMap != null) RenderMap.SetActive(isActiveRenderMap);
    }
    
    public void ToggleRenderLupa()
    {
        isActiveRenderLupa = !isActiveRenderLupa;
        
        if (RenderLupa != null) RenderLupa.SetActive(isActiveRenderLupa);
    }

    public void SetActiveProps(bool val)
    {
        if (ControlMap._singletonExists)
        {
            if (val)
                ControlMap.singleton.LoadProps();
            else
                ControlMap.singleton.DestroyProps();
        }
    }
    
    public void ToggleLermaChiconautla()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1) // Lerma
        {
            SceneManager.LoadScene(2); // Chiconautla
            return;
        }
        if (SceneManager.GetActiveScene().buildIndex == 2) // Chiconautla
        {
            SceneManager.LoadScene(1); // Lerma
            return;
        }
    }
}
