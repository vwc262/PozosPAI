using UnityEngine;

public class ControlButtonsUIParticular : MonoBehaviour
{

    public GameObject buttonsUIPozo;
    public GameObject buttonsUIPlanta;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckUIButtons()
    {
        if (ParticularManager._singletonExists)
        {
            if (!(ParticularManager.singleton.sitio.dataSitio.idSitio == 1421))
            {
                buttonsUIPozo.SetActive(true);
                buttonsUIPlanta.SetActive(false);
            }
            else
            {
                buttonsUIPozo.SetActive(false);
                buttonsUIPlanta.SetActive(true);
            }
        }
    }
}
