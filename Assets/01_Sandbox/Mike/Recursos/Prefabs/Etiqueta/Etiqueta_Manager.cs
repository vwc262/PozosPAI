using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Etiqueta_Manager : MonoBehaviour
{
    public float updateRate = 5;
    private float _countdown;

    public GameObject panel;
    public Sprite imageStateOff;
    public Sprite imageStateOn;
    public Text textGasto;
    public Text textPresion;

    public LineRenderer line;
    public Color colorConexion;
    public Color colorDesconexion;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _countdown -= Time.deltaTime;
        if(_countdown <= 0)
        {
            UpdateEtiqueta();
            _countdown = updateRate;
        }            
    }

    public void UpdateEtiqueta()
    {
        if (ParticularManager._singletonExists)
        {
            if (ParticularManager.singleton.sitio.dataInTime)
            {
                panel.GetComponent<Image>().sprite = imageStateOn;
                line.material.color = colorConexion;
            }
            else
            {
                panel.GetComponent<Image>().sprite = imageStateOff;
                line.material.color = colorDesconexion;
            }
            
            List<SignalBase> gasto = ParticularManager.singleton.sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.GASTO);

            if (gasto.Count > 0)
            {
                if (gasto[0].DentroRango)
                {
                    textGasto.text = "Gasto: " + $"{gasto[0].Valor}" + " LPS";
                }
                else
                {
                    textGasto.text = "Gasto: -";
                }
            }
            else
            {
                textGasto.text = "Gasto: N/A";
            }
            
            List<SignalBase> presion = ParticularManager.singleton.sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.PRESION);
            
            if (presion.Count > 0)
            {
                if (presion[0].DentroRango)
                {
                    textPresion.text = "Presíon: " + $"{presion[0].Valor}" + " Kg/cm2";
                }
                else
                {
                    textPresion.text = "Presíon: -";
                }
            }
            else
            {
                textPresion.text = "Presíon: N/A";
            }
            
        }
        
    }
}
