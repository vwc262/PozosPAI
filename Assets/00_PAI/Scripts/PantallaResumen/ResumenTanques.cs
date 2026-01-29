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
            
            float tempPromedioGastoChalmita = 0;
            float tempPromedioGastoNzt = 0;
            float tempTotalPromedio = 0;
            
            
            //Calculos
            tempInstantaneoChalmita = ControlDatosBarrientos.GetGastoBarrientos(2) +
                                      ControlDatosBarrientos.GetGastoBarrientos(3) +
                                      ControlDatosBarrientos.GetGastoBarrientos(4);

            temInstantaneoNzt = ControlDatosBarrientos.GetGastoBarrientos(5) +
                                ControlDatosBarrientos.GetGastoBarrientos(6);
            
            tempTotalInstantaneo = tempInstantaneoChalmita + temInstantaneoNzt;

            tempPromedioGastoChalmita =
                ((ControlDatos_PAI.singleton.listAverages.Items.Find(x => x.IndexSignal == 3).Promedio +
                ControlDatos_PAI.singleton.listAverages.Items.Find(x => x.IndexSignal == 4).Promedio +
                ControlDatos_PAI.singleton.listAverages.Items.Find(x => x.IndexSignal == 5).Promedio)/1000);

            tempPromedioGastoNzt =
                ((ControlDatos_PAI.singleton.listAverages.Items.Find(x => x.IndexSignal == 6).Promedio +
                ControlDatos_PAI.singleton.listAverages.Items.Find(x => x.IndexSignal == 7).Promedio)/1000);
            
            tempTotalPromedio = tempPromedioGastoChalmita + tempPromedioGastoNzt;
            
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
            
            //Promedios
            if (promedioGastoChalmita != null)
            {
                promedioGastoChalmita.text = tempPromedioGastoChalmita.ToString() + "<color=yellow> [m³/s]</color>";
            }
            
            if (promedioGastoNzt != null)
            {
                promedioGastoNzt.text = tempPromedioGastoNzt.ToString() + "<color=yellow> [m³/s]</color>";
            }
            
            //Estampa de tiempo
            if (estampaTiempoChalmita != null)
            {
                estampaTiempoChalmita.text =
                    ControlDatos_PAI.singleton.listAverages.Items.Find(x => x.IndexSignal == 3).Fecha;
            }
            
            if (estampaTiempoNzt != null)
            {
                estampaTiempoNzt.text =
                    ControlDatos_PAI.singleton.listAverages.Items.Find(x => x.IndexSignal == 6).Fecha;
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

            if (totalPromedio != null)
            {
                totalPromedio.text = tempTotalPromedio.ToString() + "<color=yellow> [m³/s]</color>";
            }
            
        }

    }

    public void InitResumen()
    {
        ControlDatosBarrientos = ControlDatos.singleton.listSitios.Find(x =>  x.dataSitio.idSitio == 1421);
    }
}

