using UnityEngine;
using UnityEngine.UI;

public class ControlUISignal : MonoBehaviour
{
    public ControlUISitio UISitio;

    public SignalBase.TipoSignalEnum signal;
    public int index;

    public TMPro.TMP_Text textDataSignal;
    
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
        if (textDataSignal != null && UISitio != null)
        {
            switch (signal)
            {
                case SignalBase.TipoSignalEnum.NIVEL:
                    textDataSignal.text = $"Nivel {index + 1}: {UISitio.sitio.GetNivel(index)}";
                    break;
                
                case SignalBase.TipoSignalEnum.GASTO:
                    textDataSignal.text = $"Gasto {index + 1}: {UISitio.sitio.GetGasto(index)}";
                    break;
            }
        }
    }
}
