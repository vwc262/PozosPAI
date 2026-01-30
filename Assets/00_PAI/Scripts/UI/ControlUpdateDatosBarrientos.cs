using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUpdateDatosBarrientos : MonoBehaviour
{
    public ControlUISitio UISitio;
    
    public List<Text> textUpdateFecha;
    
    public List<Image> imageConexion;
   
    public Sprite online;
    public Sprite offline;
    
    public float updateRate = 1;
    private float countdown;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            UpdateData();
            countdown = updateRate;
        } 
    }

    public void UpdateData()
    {
        if (textUpdateFecha != null)
        {
            foreach (var date in textUpdateFecha)
            {
                date.text = FuncAuxDateTime.GetDateFormat_DMAH(UISitio.sitio.dataSitio.fecha);
            }
        }

        if (imageConexion != null)
        {
            if (UISitio.sitio.GetStatusConexionSitio())
            {
                foreach (var img in imageConexion)
                {
                    img.sprite = online;
                }
            }
            else
                foreach (var img in imageConexion)
                {
                    img.sprite = offline;
                }
        }
    }
}
