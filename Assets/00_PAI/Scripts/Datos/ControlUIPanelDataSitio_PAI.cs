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
    
    public override void UpdateInfoUISitio(ControlMarcadorSitio controlMarcadorSitio)
    {
        if (textPresion != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.presion.Count > 0)
            {
                if (controlMarcadorSitio.sitio.dataSitio.presion[0].DentroRango)
                    textPresion.text = GetString2decimals(controlMarcadorSitio.sitio.dataSitio.presion[0].Valor) + " Kg/cm2";
                else
                    textPresion.text = "-";
            }
            else
                textPresion.text = "N/A";
        }

        if (controlMarcadorSitio.statusDataInTime == 1)
            SetPointsColor(Color.green);
        else
            SetPointsColor(Color.red);
        
        UpdateUIBomba(controlMarcadorSitio.sitio);
        
        if (textGasto != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.gasto.Count > 0)
            {
                if (controlMarcadorSitio.sitio.dataSitio.gasto[0].DentroRango)
                    textGasto.text = GetString2decimals(controlMarcadorSitio.sitio.dataSitio.gasto[0].Valor) + " L/s";
                else
                    textGasto.text = "-";
            }
            else
                textGasto.text = "N/A";
        }
        
        if (textTotalizado != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.totalizado.Count > 0)
            {
                if (controlMarcadorSitio.sitio.dataSitio.gasto[0].DentroRango)
                    textTotalizado.text = $"{controlMarcadorSitio.sitio.dataSitio.totalizado[0].Valor:F0}" + " m3";
                else
                    textTotalizado.text = "-";
            }
            else
                textTotalizado.text = "N/A";
        }
        
        if (textBateria != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.Baterias.Count > 0)
            {
                textBateria.text = "";
                
                foreach (var bateria in controlMarcadorSitio.sitio.dataSitio.Baterias)
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
                textBateria.text = GetString2decimals(controlMarcadorSitio.sitio.dataSitio.voltaje) + " V";
            }
        }
        
        if (GO_Nivel != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.nivel.Count > 0)
            {
                GO_Nivel.SetActive(true);
                
                if (textNivel != null)
                {
                    if (controlMarcadorSitio.sitio.dataSitio.nivel[0].DentroRango)
                        textNivel.text = GetString2decimals(controlMarcadorSitio.sitio.dataSitio.nivel[0].Valor) + " m";
                    else
                        textNivel.text = "-\n";
                }
                
                if (controlMarcadorSitio.sitio.dataSitio.nivel.Count > 1)
                {
                    if (textNivel_estatico != null)
                    {
                        if (controlMarcadorSitio.sitio.dataSitio.nivel[1].DentroRango)
                            textNivel_estatico.text = GetString2decimals(controlMarcadorSitio.sitio.dataSitio.nivel[1].Valor) + " m";
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
            if (controlMarcadorSitio.sitio.dataSitio.Voltajes_Motor.Count > 0)
            {
                if (controlMarcadorSitio.sitio.dataSitio.Voltajes_Motor[0].DentroRango)
                    text_voltage_L1.text = $"{controlMarcadorSitio.sitio.dataSitio.Voltajes_Motor[0].Valor:F0}" + " V";
                else
                    text_voltage_L1.text = "-";
            }
            else
                text_voltage_L1.text = "N/A";
        }
        
        if (text_voltage_L2 != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.Voltajes_Motor.Count > 1)
            {
                if (controlMarcadorSitio.sitio.dataSitio.Voltajes_Motor[1].DentroRango)
                    text_voltage_L2.text = $"{controlMarcadorSitio.sitio.dataSitio.Voltajes_Motor[1].Valor:F0}" + " V";
                else
                    text_voltage_L2.text = "-";
            }
            else
                text_voltage_L2.text = "N/A";
        }
        
        if (text_voltage_L3 != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.Voltajes_Motor.Count > 2)
            {
                if (controlMarcadorSitio.sitio.dataSitio.Voltajes_Motor[2].DentroRango)
                    text_voltage_L3.text = $"{controlMarcadorSitio.sitio.dataSitio.Voltajes_Motor[2].Valor:F0}" + " V";
                else
                    text_voltage_L3.text = "-";
            }
            else
                text_voltage_L3.text = "N/A";
        }
        
        if (text_corriente_L1 != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.Corrientes_Motor.Count > 0)
            {
                if (controlMarcadorSitio.sitio.dataSitio.Corrientes_Motor[0].DentroRango)
                    text_corriente_L1.text = $"{controlMarcadorSitio.sitio.dataSitio.Corrientes_Motor[0].Valor:F0}" + " Amp";
                else
                    text_corriente_L1.text = "-";
            }
            else
                text_corriente_L1.text = "N/A";
        }
        
        if (text_corriente_L2 != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.Corrientes_Motor.Count > 1)
            {
                if (controlMarcadorSitio.sitio.dataSitio.Corrientes_Motor[1].DentroRango)
                    text_corriente_L2.text = $"{controlMarcadorSitio.sitio.dataSitio.Corrientes_Motor[1].Valor:F0}" + " Amp";
                else
                    text_corriente_L2.text = "-";
            }
            else
                text_corriente_L2.text = "N/A";
        }
        
        if (text_corriente_L3 != null)
        {
            if (controlMarcadorSitio.sitio.dataSitio.Corrientes_Motor.Count > 2)
            {
                if (controlMarcadorSitio.sitio.dataSitio.Corrientes_Motor[2].DentroRango)
                    text_corriente_L3.text = $"{controlMarcadorSitio.sitio.dataSitio.Corrientes_Motor[2].Valor:F0}" + " Amp";
                else
                    text_corriente_L3.text = "-";
            }
            else
                text_corriente_L3.text = "N/A";
        }
    }
}
