using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoySitioLOD : MonoBehaviour
{
    // Hola Boy
    private GameObject cameraMove;
    public float distance;
    public float distanceLOD0;
    public float distanceLOD1;

    public List<GameObject> LOD0;
    public List<GameObject> LOD1;

    public bool LOD0_active = true;
    public bool LOD1_active = true;
    
    
    void Start()
    {
	    cameraMove = FindObjectOfType<VWC_MoveCamera>().CameraGimbal;

        LOD0_active = false;
        LOD1_active = false;
        foreach (var item in LOD0)
            item.SetActive(LOD0_active);
        foreach (var item in LOD1)
            item.SetActive(LOD1_active);
    }

    
    void Update()
    {
        distance = Vector3.Distance(cameraMove.gameObject.transform.position, transform.position);

        if (distance < distanceLOD0)
        {
            //Deshabililitar
            if (LOD1_active)
            {
                LOD1_active = false;

                foreach (var item in LOD1)
                    item.SetActive(LOD1_active);
            }

            if (!LOD0_active)
            {
                LOD0_active = true;
                
                foreach (var item in LOD0)
                    item.SetActive(LOD0_active);
            }
        }
        else
        {
            //Deshabililitar
            if (LOD0_active)
            {
                LOD0_active = false;
                foreach (var item in LOD0)
                    item.SetActive(LOD0_active);
            }

            if (distance < distanceLOD1)
            {
                if (!LOD1_active)
                {
                    LOD1_active = true;
                    foreach (var item in LOD1)
                        item.SetActive(LOD1_active);
                }
            }
            else
            {
                //Deshabililitar
                if (LOD1_active)
                {
                    LOD1_active = false;
                    foreach (var item in LOD1)
                        item.SetActive(LOD1_active);
                }
            }
        }
    }
}
