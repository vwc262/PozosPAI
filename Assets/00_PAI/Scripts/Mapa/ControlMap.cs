using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMap : Singleton<ControlMap>
{
    // Hola Boy
	public bool showNormalMap = true;

    public bool createMapOnStart;
    public bool createPropsOnStart;
    public GameObject prefab_normaMap;
    public GameObject prefab_roadsMap;
    public GameObject prefab_props;
    
    public GameObject zonasRegiones;
    public GameObject normaMap;
    public GameObject roadsMap;
    public GameObject props;

    public VWC_MoveCamera moveCamera;

    public float DespZoomPivot = 1000;
    
    public Vector3 normalMaxScaleMarcador = new Vector3(1,1,1);
    public Vector3 mapaMaxScaleMarcador = new Vector3(0.3f,0.3f,0.3f);

    public void Start()
    {
        if (createMapOnStart)
        {
            if (normaMap != null) Destroy(normaMap);
            if (roadsMap != null) Destroy(roadsMap);

            LoadMaps();
        }

        if (createPropsOnStart)
        {
            DestroyProps();

            LoadProps();
        }
    }

    public void ToggleMap()
    {
        showNormalMap = !showNormalMap;
        
        if (normaMap != null) normaMap.SetActive(showNormalMap);
        if (zonasRegiones != null) zonasRegiones.SetActive(showNormalMap);
        if (roadsMap != null) roadsMap.SetActive(!showNormalMap);

        if (moveCamera != null)
        {
            if (showNormalMap)
                moveCamera.zoomDownPivot.transform.localPosition += Vector3.up * DespZoomPivot;
            else
                moveCamera.zoomDownPivot.transform.localPosition += Vector3.down * DespZoomPivot;
        }
        
        if (showNormalMap)
        {
            VWCBillboardSitio.maxScale = normalMaxScaleMarcador;
        }
        else
        {
            VWCBillboardSitio.maxScale = mapaMaxScaleMarcador;
        }
    }

    // public void SetActiveColliderMap(bool _active)
    // {
    //     if (normaMap != null)
    //     {
    //         DisableTerrainMeshCollider controlCollider = normaMap.GetComponentInChildren<DisableTerrainMeshCollider>();
    //
    //         if (controlCollider != null)
    //             controlCollider.SetEnableMeshColliders(_active);
    //     }
    // }
    //
    // public void ChangeActiveColliderMap()
    // {
    //     DisableTerrainMeshCollider controlCollider = normaMap.GetComponentInChildren<DisableTerrainMeshCollider>();
    //     
    //     if (controlCollider != null)
    //         controlCollider.ChangeEnabledMeshCollider();
    // }

    public void LoadMaps()
    {
        if (prefab_normaMap != null)
        {
            normaMap = Instantiate(prefab_normaMap, this.transform);
            
        }

        if (prefab_roadsMap != null)
        {
            roadsMap = Instantiate(prefab_roadsMap, this.transform);
        }
        
        if (normaMap != null) normaMap.SetActive(showNormalMap);
        if (zonasRegiones != null) zonasRegiones.SetActive(showNormalMap);
        if (roadsMap != null) roadsMap.SetActive(!showNormalMap);
    }
    
    public void LoadProps()
    {
        if (prefab_props != null)
        {
            props = Instantiate(prefab_props, this.transform);
        }
    }

    public void DestroyProps()
    {
        if (props != null) Destroy(props);
    }
}
