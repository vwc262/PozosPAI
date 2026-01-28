using UnityEngine;

public class ResumenTanques : Singleton<ResumenTanques>
{
    public float updateRate = 5;
    private float countdown;
    
    public ControlSitio ControlDatosBarrientos;
    
    public TMPro.TMP_Text promedioGastoChalmita;
    public TMPro.TMP_Text promedioGastoNzt;
    public TMPro.TMP_Text estampaTiempoChalmita;
    public TMPro.TMP_Text estampaTiempoNzt;
    public TMPro.TMP_Text gastoInstantaneoChalmita;
    public TMPro.TMP_Text gastoInstantaneoNzt;
    public TMPro.TMP_Text gasto1;
    public TMPro.TMP_Text gasto2;
    public TMPro.TMP_Text gasto3;
    public TMPro.TMP_Text gasto4;
    public TMPro.TMP_Text gasto5;
    public TMPro.TMP_Text totalPromedio;
    public TMPro.TMP_Text totalInstantaneo;
    
    void Start()
    {
        UpdateData();
    }

 
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            UpdateData();
            countdown = updateRate;
        }
    }

    public void UpdateData()
    {
        if (ControlDatosBarrientos != null)
        {
            float tempInstantaneoChalmita = 0;
            float temInstantaneoNzt = 0;
            float tempTotalInstantaneo = 0;

            tempInstantaneoChalmita = ControlDatosBarrientos.GetGastoBarrientos(2) +
                                      ControlDatosBarrientos.GetGastoBarrientos(3) +
                                      ControlDatosBarrientos.GetGastoBarrientos(4);

            temInstantaneoNzt = ControlDatosBarrientos.GetGastoBarrientos(5) +
                                ControlDatosBarrientos.GetGastoBarrientos(6);
            
            tempTotalInstantaneo = tempInstantaneoChalmita + temInstantaneoNzt;
            
            
            //Gastos Salida
            if (gasto1 != null)
            {
                gasto1.text = ControlDatosBarrientos.GetGastoBarrientos(2).ToString() + "<color=yellow> [m³/s]</color>";
            }
            
            if (gasto2 != null)
            {
                gasto2.text = ControlDatosBarrientos.GetGastoBarrientos(3).ToString() + "<color=yellow> [m³/s]</color>";
            }
            
            if (gasto3 != null)
            {
                gasto3.text = ControlDatosBarrientos.GetGastoBarrientos(4).ToString() + "<color=yellow> [m³/s]</color>";
            }
            
            if (gasto4 != null)
            {
                gasto4.text = ControlDatosBarrientos.GetGastoBarrientos(5).ToString() + "<color=yellow> [m³/s]</color>";
            }
            
            if (gasto5 != null)
            {
                gasto5.text = ControlDatosBarrientos.GetGastoBarrientos(6).ToString() + "<color=yellow> [m³/s]</color>";
            }
            
            //Gastos instantaneos
            if (gastoInstantaneoChalmita != null)
            {
                gastoInstantaneoChalmita.text = tempInstantaneoChalmita.ToString() + "<color=yellow> [m³/s]</color>";
                
            }

            if (gastoInstantaneoNzt != null)
            {
                gastoInstantaneoNzt.text = temInstantaneoNzt.ToString() + "<color=yellow> [m³/s]</color>";
            }
            
            //Totales
            if (totalInstantaneo != null)
            {
                totalInstantaneo.text = tempTotalInstantaneo.ToString() + "<color=yellow> [m³/s]</color>";
            }
            
            
            
        }

    }

    public void InitResumen()
    {
        ControlDatosBarrientos = ControlDatos.singleton.listSitios.Find(x =>  x.dataSitio.idSitio == 1421);
    }
}

