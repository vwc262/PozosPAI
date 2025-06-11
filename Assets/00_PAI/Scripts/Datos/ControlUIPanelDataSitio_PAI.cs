using System.Collections.Generic;
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
    
    public override void UpdateInfoUISitio(ControlSitio _sitio)
    {
        if (_sitio != null)
        {
            if (textPresion != null)
            {
                List<SignalBase> presion = _sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.PRESION);
                
                if (presion != null && presion.Count > 0)
                {
                    if (presion[0].DentroRango)
                        textPresion.text = GetString2decimals(presion[0].Valor) + " Kg/cm2";
                    else
                        textPresion.text = "-";
                }
                else
                    textPresion.text = "N/A";
            }

            if (_sitio.dataInTime)
                SetPointsColor(Color.green);
            else
                SetPointsColor(Color.red);

            UpdateUIBomba(_sitio);

            if (textGasto != null)
            {
                List<SignalBase> gasto = _sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.GASTO);
                
                if (gasto.Count > 0)
                {
                    if (gasto[0].DentroRango)
                        textGasto.text = GetString2decimals(gasto[0].Valor) + " L/s";
                    else
                        textGasto.text = "-";
                }
                else
                    textGasto.text = "N/A";
            }

            if (textTotalizado != null)
            {
                List<SignalBase> totalizado = _sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.TOTALIZADO);
                
                if (totalizado.Count > 0)
                {
                    if (totalizado[0].DentroRango)
                        textTotalizado.text = $"{totalizado[0].Valor:F0}" + " m3";
                    else
                        textTotalizado.text = "-";
                }
                else
                    textTotalizado.text = "N/A";
            }
            
            List<SignalBase> baterias = _sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.VOLTAJE);
            
            if (baterias.Count > 0)
            {
                if (textBateria != null)
                {
                    textBateria.text = GetString2decimals(baterias[0].Valor) + " V";
                }
                
                if (baterias.Count > 1)
                {
                    if (textFuente != null)
                    {
                        textFuente.text = GetString2decimals(baterias[1].Valor) + " V";
                    }
                }
            }
            else
            {
                if (textBateria != null)
                    textBateria.text = "-";
                
                if (textFuente != null)
                    textFuente.text = GetString2decimals(_sitio.dataSitio.voltaje) + " V";
            }

            if (GO_Nivel != null)
            {
                List<SignalBase> nivel = _sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.NIVEL);
                
                if (nivel.Count > 0)
                {
                    GO_Nivel.SetActive(true);

                    if (textNivel != null)
                    {
                        if (nivel[0].DentroRango)
                            textNivel.text = GetString2decimals(nivel[0].Valor) + " m";
                        else
                            textNivel.text = "-";
                    }

                    if (nivel.Count > 1)
                    {
                        if (textNivel_estatico != null)
                        {
                            if (nivel[1].DentroRango)
                                textNivel_estatico.text = GetString2decimals(nivel[1].Valor) + " m";
                            else
                                textNivel_estatico.text = "-";
                        }
                    }
                }
                else
                {
                    GO_Nivel.SetActive(false);
                }
            }
            
            List<SignalBase> voltajesMotor = _sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.VOLTAJE_RANGO);

            if (text_voltage_L1 != null)
            {
                if (voltajesMotor.Count > 0)
                {
                    if (voltajesMotor[0].DentroRango)
                        text_voltage_L1.text = $"{voltajesMotor[0].Valor:F0}" + " V";
                    else
                        text_voltage_L1.text = "-";
                }
                else
                    text_voltage_L1.text = "N/A";
            }

            if (text_voltage_L2 != null)
            {
                if (voltajesMotor.Count > 1)
                {
                    if (voltajesMotor[1].DentroRango)
                        text_voltage_L2.text = $"{voltajesMotor[1].Valor:F0}" + " V";
                    else
                        text_voltage_L2.text = "-";
                }
                else
                    text_voltage_L2.text = "N/A";
            }

            if (text_voltage_L3 != null)
            {
                if (voltajesMotor.Count > 2)
                {
                    if (voltajesMotor[2].DentroRango)
                        text_voltage_L3.text = $"{voltajesMotor[2].Valor:F0}" + " V";
                    else
                        text_voltage_L3.text = "-";
                }
                else
                    text_voltage_L3.text = "N/A";
            }
            
            List<SignalBase> corrientesMotor = _sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.CORRIENTE_RANGO);

            if (text_corriente_L1 != null)
            {
                if (corrientesMotor.Count > 0)
                {
                    if (corrientesMotor[0].DentroRango)
                        text_corriente_L1.text = $"{corrientesMotor[0].Valor:F0}" + " A";
                    else
                        text_corriente_L1.text = "-";
                }
                else
                    text_corriente_L1.text = "N/A";
            }

            if (text_corriente_L2 != null)
            {
                if (corrientesMotor.Count > 1)
                {
                    if (corrientesMotor[1].DentroRango)
                        text_corriente_L2.text = $"{corrientesMotor[1].Valor:F0}" + " A";
                    else
                        text_corriente_L2.text = "-";
                }
                else
                    text_corriente_L2.text = "N/A";
            }

            if (text_corriente_L3 != null)
            {
                if (corrientesMotor.Count > 2)
                {
                    if (corrientesMotor[2].DentroRango)
                        text_corriente_L3.text = $"{corrientesMotor[2].Valor:F0}" + " A";
                    else
                        text_corriente_L3.text = "-";
                }
                else
                    text_corriente_L3.text = "N/A";
            }
        }
    }
}
