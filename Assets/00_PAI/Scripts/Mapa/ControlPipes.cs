using Sirenix.OdinInspector;
using UnityEngine;

public class ControlPipes : Singleton<ControlPipes>
{
    public GameObject contenedorPipes;
    
    public float longitudPipes;
    public float latitudPipes;
    
    [Button]
    public void SetPositionPipes()
    {
        contenedorPipes.transform.position = transform.position + Gps2UnityConverter.GPS2Unity(latitudPipes, longitudPipes);
    }
}
