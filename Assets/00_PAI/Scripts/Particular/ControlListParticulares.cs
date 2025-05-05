using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ControlListParticulares : MonoBehaviour
{
    public Particular particularDefault;

    private GameObject modeloParticular;
    
    public List<Particular> listParticulares;

    public void SetActiveParticularByID(int idParticular)
    {
        particularDefault.Modelo3D.SetActive(true);
        
        if (modeloParticular != null)
            Destroy(modeloParticular);

        foreach (var particular in listParticulares)
        {
            if (particular.idParticular == idParticular)
            {
                particularDefault.Modelo3D.SetActive(false);
                modeloParticular = Instantiate(particular.Modelo3D, this.transform);
                
                modeloParticular.transform.localPosition = Vector3.zero;
            }
        }
    }
}
