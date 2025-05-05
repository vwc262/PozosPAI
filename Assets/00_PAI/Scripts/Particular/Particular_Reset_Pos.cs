using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

public class Particular_Reset_Pos : Singleton<Particular_Reset_Pos>
{
    public DroneManager droneManager;
    public Vector3 posInicial;
    public Vector3 rotacionInicial;
   
    void Start()
    {
        posInicial = gameObject.transform.localPosition;
        rotacionInicial = gameObject.transform.localEulerAngles;
        
        droneManager = GetComponent<DroneManager>();
    }

    [Button]
    public void ResetPosition()
    {
        StartCoroutine(ResetPositionCoroutine());
    }
    
    public IEnumerator ResetPositionCoroutine()
    {
        if (droneManager != null)
        {
            droneManager.enabled = false;
            yield return new WaitForSeconds(0.1f);
            gameObject.transform.SetLocalPositionAndRotation(posInicial, Quaternion.Euler(rotacionInicial));
            yield return new WaitForSeconds(0.1f);
            droneManager.enabled = true;
        }
    }
}