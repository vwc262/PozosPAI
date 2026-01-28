using UnityEngine;
using UnityEngine.UI;

public class ControlUISignal : MonoBehaviour
{
    public ControlUISitio UISitio;

    public SignalBase.TipoSignalEnum signal;
    public int index;

    public Image imageConexion;
    public Sprite online;
    public Sprite offline;
    
    //public TMPro.TMP_Text textSignal;
    public TMPro.TMP_Text textDataSignal;
    public Text textUpdateFecha;
    
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
        // if (textUpdateFecha != null)
        // {
        //     textUpdateFecha.text = UISitio.sitio.dataInTime;
        // }

        if (imageConexion != null)
        {
            if (UISitio.sitio.GetStatusConexionSitio())
                imageConexion.sprite = online;
            else
                imageConexion.sprite = offline;
        }
        
        if (textDataSignal != null && UISitio != null)
        {
            switch (signal)
            {
                case SignalBase.TipoSignalEnum.NIVEL:
                    if (textDataSignal != null) textDataSignal.text = $"{UISitio.sitio.GetNivel(index) + " m"}";
                    break;
                
                case SignalBase.TipoSignalEnum.GASTO:
                    {
                        if (textDataSignal != null) textDataSignal.text = $"{UISitio.sitio.GetGastoBarrientos(index)}" + " m³/s";
                    }
                    break;
                case SignalBase.TipoSignalEnum.TOTALIZADO:
                {
                      if (textDataSignal != null) textDataSignal.text = $"{UISitio.sitio.GetTotalizado(index)}" + " m³";
                }
                    break;
            }
        }
    }
}
