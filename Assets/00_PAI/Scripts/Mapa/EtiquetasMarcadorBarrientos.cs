using UnityEngine;

public class EtiquetasMarcadorBarrientos : MonoBehaviour
{
    public float updateRate = 5;
    private float countdown;
    
    
    public TMPro.TMP_Text promedioGastoTeoloyucan;
    public TMPro.TMP_Text promedioGastoFFCC;
    
    public TMPro.TMP_Text promedioGastoChalmita;
    public TMPro.TMP_Text promedioGastoNzt;
    
    float tempInstantaneoChalmita = 0;
    float temInstantaneoNzt = 0;
    
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
        float temInstantaneoChalmita = ControlDatos.singleton.listSitios.Find(x => x.dataSitio.idSitio == 1421)
            .GetGastoBarrientos(2)+  ControlDatos.singleton.listSitios.Find(x => x.dataSitio.idSitio == 1421)
            .GetGastoBarrientos(3)+ ControlDatos.singleton.listSitios.Find(x => x.dataSitio.idSitio == 1421)
            .GetGastoBarrientos(4);

        float temInstantaneoNzt = ControlDatos.singleton.listSitios.Find(x => x.dataSitio.idSitio == 1421)
            .GetGastoBarrientos(5) + ControlDatos.singleton.listSitios.Find(x => x.dataSitio.idSitio == 1421)
            .GetGastoBarrientos(6);
            
        if (ResumenTanques.singleton.ControlDatosBarrientos != null)
        {
            if (promedioGastoChalmita != null)
            {
                promedioGastoChalmita.text =temInstantaneoChalmita.ToString() + " [m³/s]";
            }
            
            if (promedioGastoNzt != null)
            {
                promedioGastoNzt.text = temInstantaneoNzt.ToString() + " [m³/s]";
            }
            
            if (promedioGastoTeoloyucan != null)
            {
                promedioGastoTeoloyucan.text = ControlDatos.singleton.listSitios.Find(x => x.dataSitio.idSitio == 1421).GetGastoBarrientos(0).ToString() + " [m³/s]" ;
            }
            
            if (promedioGastoFFCC != null)
            {
                promedioGastoFFCC.text = ControlDatos.singleton.listSitios.Find(x => x.dataSitio.idSitio == 1421).GetGastoBarrientos(1).ToString() + " [m³/s]";
            }
        }
        
        
    }
    
    
}
