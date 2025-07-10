using System.Collections;
using System.Linq;
using UnityEngine;

public class ControlGraphFull : Singleton<ControlGraphFull>
{
    public GameObject RootGrafica;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DisableGraphCoroutine());
    }

    public IEnumerator DisableGraphCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        SetEnableGraph(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetEnableGraph(bool enable)
    {
        if (RootGrafica != null)
            RootGrafica.SetActive(enable);
    }
}
