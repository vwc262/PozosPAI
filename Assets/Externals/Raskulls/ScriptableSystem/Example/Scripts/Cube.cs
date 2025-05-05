using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Cube : MonoBehaviour
{
    public void OnSpawnEnemy(UnityEngine.Vector3 Value)
    {
        Debug.Log("2");
    }

    public void OnPostSpawnEnemy(UnityEngine.Vector3 Value)
    {
        Debug.Log("3");
    }
    public void OnPreSpawnEnemy(UnityEngine.Vector3 Value)
    {
        //Write Your Implementation Here
    }
}
