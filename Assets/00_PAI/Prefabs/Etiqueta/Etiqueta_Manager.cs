using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Etiqueta_Manager : MonoBehaviour
{
    public float updateRate = 5;
    private float _countdown;

    //public GameObject panel;

    public int indexGasto = 0;
    
    
    public SpriteRenderer imageState;
    
    public TMPro.TMP_Text textGasto;
    public TMPro.TMP_Text textPresion;
    public TMPro.TMP_Text textNivel;

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
                imageState.color = colorConexion;
                line.material.SetColor("_Emision_color", colorConexion);
            }
            else
            {
                imageState.color = colorDesconexion;
                line.material.SetColor("_Emision_color", colorDesconexion);
            }
            
            List<SignalBase> gasto = ParticularManager.singleton.sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.GASTO);

            if (gasto.Count > 0)
            {
                if (gasto[indexGasto].DentroRango)
                {
                    textGasto.text = "Gasto: " + $"{gasto[indexGasto].Valor}" + " l/s";
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
                    textPresion.text = "Presión: " + $"{presion[0].Valor}" + " Kg/cm2";
                }
                else
                {
                    textPresion.text = "Presión: -";
                }
            }
            else
            {
                textPresion.text = "Presión: N/A";
            }
            
            List<SignalBase> nivel = ParticularManager.singleton.sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.NIVEL);
            
            if (presion.Count > 0)
            {
                if (nivel[0].DentroRango)
                {
                    textNivel.text = "Nivel: " + $"{nivel[0].Valor}" + " m";
                }
                else
                {
                    textNivel.text = "Nivel: -";
                }
            }
            else
            {
                textNivel.text = "Nivel: N/A";
            }
            
        }
        
    }
}
