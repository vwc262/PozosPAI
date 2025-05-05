using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ControlUIPanelDataSitio_PAI : ControlUIPanelDataSitio
{
    [TabGroup("UI_voltages")] public Text text_voltage_L1;
    [TabGroup("UI_voltages")] public Text text_voltage_L2;
    [TabGroup("UI_voltages")] public Text text_voltage_L3;
    [TabGroup("UI_voltages")] public Text text_corriente_L1;
    [TabGroup("UI_voltages")] public Text text_corriente_L2;
    [TabGroup("UI_voltages")] public Text text_corriente_L3;
    
    [TabGroup("UI")] public Text textNivel_estatico;
    
    public override void UpdateInfoUISitio(SitioGPS sitio)
    {
        if (textPresion != null)
        {
            if (sitio.MyDataSitio.presion.Count > 0)
            {
                if (sitio.MyDataSitio.presion[0].DentroRango)
                    textPresion.text = GetString2decimals(sitio.MyDataSitio.presion[0].Valor) + " Kg/cm2";
                else
                    textPresion.text = "-";
            }
            else
                textPresion.text = "N/A";
        }

        if (sitio.statusDataInTime == 1)
            SetPointsColor(Color.green);
        else
            SetPointsColor(Color.red);
        
        UpdateUIBomba(sitio);
        
        if (textGasto != null)
        {
            if (sitio.MyDataSitio.gasto.Count > 0)
            {
                if (sitio.MyDataSitio.gasto[0].DentroRango)
                    textGasto.text = GetString2decimals(sitio.MyDataSitio.gasto[0].Valor) + " L/s";
                else
                    textGasto.text = "-";
            }
            else
                textGasto.text = "N/A";
        }
        
        if (textTotalizado != null)
        {
            if (sitio.MyDataSitio.totalizado.Count > 0)
            {
                if (sitio.MyDataSitio.gasto[0].DentroRango)
                    textTotalizado.text = $"{sitio.MyDataSitio.totalizado[0].Valor:F0}" + " m3";
                else
                    textTotalizado.text = "-";
            }
            else
                textTotalizado.text = "N/A";
        }
        
        if (textBateria != null)
        {
            if (sitio.MyDataSitio.Baterias.Count > 0)
            {
                textBateria.text = "";
                
                foreach (var bateria in sitio.MyDataSitio.Baterias)
                {
                    if (textBateria.text != "")
                        textBateria.text += "\n";
                        
                    if (bateria.DentroRango)
                        textBateria.text += GetString2decimals(bateria.Valor) + " V";
                    else
                        textBateria.text += "-";
                }
            }
            else
            {
                textBateria.text = GetString2decimals(sitio.MyDataSitio.voltaje) + " V";
            }
        }
        
        if (GO_Nivel != null)
        {
            if (sitio.MyDataSitio.nivel.Count > 0)
            {
                GO_Nivel.SetActive(true);
                
                if (textNivel != null)
                {
                    if (sitio.MyDataSitio.nivel[0].DentroRango)
                        textNivel.text = GetString2decimals(sitio.MyDataSitio.nivel[0].Valor) + " m";
                    else
                        textNivel.text = "-\n";
                }
                
                if (sitio.MyDataSitio.nivel.Count > 1)
                {
                    if (textNivel_estatico != null)
                    {
                        if (sitio.MyDataSitio.nivel[1].DentroRango)
                            textNivel_estatico.text = GetString2decimals(sitio.MyDataSitio.nivel[1].Valor) + " m";
                        else
                            textNivel_estatico.text = "-\n";
                    }
                }
            }
            else
            {
                GO_Nivel.SetActive(false);
            }
        }

        if (text_voltage_L1 != null)
        {
            if (sitio.MyDataSitio.Voltajes_Motor.Count > 0)
            {
                if (sitio.MyDataSitio.Voltajes_Motor[0].DentroRango)
                    text_voltage_L1.text = $"{sitio.MyDataSitio.Voltajes_Motor[0].Valor:F0}" + " V";
                else
                    text_voltage_L1.text = "-";
            }
            else
                text_voltage_L1.text = "N/A";
        }
        
        if (text_voltage_L2 != null)
        {
            if (sitio.MyDataSitio.Voltajes_Motor.Count > 1)
            {
                if (sitio.MyDataSitio.Voltajes_Motor[1].DentroRango)
                    text_voltage_L2.text = $"{sitio.MyDataSitio.Voltajes_Motor[1].Valor:F0}" + " V";
                else
                    text_voltage_L2.text = "-";
            }
            else
                text_voltage_L2.text = "N/A";
        }
        
        if (text_voltage_L3 != null)
        {
            if (sitio.MyDataSitio.Voltajes_Motor.Count > 2)
            {
                if (sitio.MyDataSitio.Voltajes_Motor[2].DentroRango)
                    text_voltage_L3.text = $"{sitio.MyDataSitio.Voltajes_Motor[2].Valor:F0}" + " V";
                else
                    text_voltage_L3.text = "-";
            }
            else
                text_voltage_L3.text = "N/A";
        }
        
        if (text_corriente_L1 != null)
        {
            if (sitio.MyDataSitio.Corrientes_Motor.Count > 0)
            {
                if (sitio.MyDataSitio.Corrientes_Motor[0].DentroRango)
                    text_corriente_L1.text = $"{sitio.MyDataSitio.Corrientes_Motor[0].Valor:F0}" + " Amp";
                else
                    text_corriente_L1.text = "-";
            }
            else
                text_corriente_L1.text = "N/A";
        }
        
        if (text_corriente_L2 != null)
        {
            if (sitio.MyDataSitio.Corrientes_Motor.Count > 1)
            {
                if (sitio.MyDataSitio.Corrientes_Motor[1].DentroRango)
                    text_corriente_L2.text = $"{sitio.MyDataSitio.Corrientes_Motor[1].Valor:F0}" + " Amp";
                else
                    text_corriente_L2.text = "-";
            }
            else
                text_corriente_L2.text = "N/A";
        }
        
        if (text_corriente_L3 != null)
        {
            if (sitio.MyDataSitio.Corrientes_Motor.Count > 2)
            {
                if (sitio.MyDataSitio.Corrientes_Motor[2].DentroRango)
                    text_corriente_L3.text = $"{sitio.MyDataSitio.Corrientes_Motor[2].Valor:F0}" + " Amp";
                else
                    text_corriente_L3.text = "-";
            }
            else
                text_corriente_L3.text = "N/A";
        }
    }
}
