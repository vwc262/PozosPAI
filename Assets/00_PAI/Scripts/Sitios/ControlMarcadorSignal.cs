using System;
using UnityEngine;

public class ControlMarcadorSignal : MonoBehaviour
{
    public Signal signal;
    public int indexSignal;
    public MeshRenderer MeshRendererBase;
    public TMPro.TMP_Text TextSignal;
    public TMPro.TMP_Text TextSignalValue;

    public void SetSignal(Signal signal, int _indexSignal = 0)
    {
        this.signal = signal;
        indexSignal = _indexSignal;

        switch (signal.tipoSignal)
        {
            case SignalBase.TipoSignalEnum.NIVEL:
            {
                if (TextSignal != null)
                {
                    if (indexSignal == 0)
                        TextSignal.text = "NIVEL DINAMICO";
                    else
                        TextSignal.text = "NIVEL ESTATICO";
                }
            }
                break;
            case SignalBase.TipoSignalEnum.VOLTAJE:
            {
                if (TextSignal != null)
                {
                    if (indexSignal == 0)
                        TextSignal.text = "Voltaje bateria";
                    else
                        TextSignal.text = "Voltaje fuente";
                }
            }
                break;
            case SignalBase.TipoSignalEnum.VOLTAJE_RANGO:
            {
                if (TextSignal != null)
                {
                    switch (indexSignal)
                    {
                        case 0: TextSignal.text = "Voltage L1-L2"; break;
                        case 1: TextSignal.text = "Voltage L2-L3"; break;
                        case 2: TextSignal.text = "Voltage L3-L1"; break;
                        case 3: TextSignal.text = "Voltage promedio"; break;
                    }
                }
            }
                break;
            case SignalBase.TipoSignalEnum.CORRIENTE_RANGO:
            {
                if (TextSignal != null)
                {
                    switch (indexSignal)
                    {
                        case 0: TextSignal.text = "Corriente L1"; break;
                        case 1: TextSignal.text = "Corriente L2"; break;
                        case 2: TextSignal.text = "Corriente L3"; break;
                        case 3: TextSignal.text = "Corriente promedio"; break;
                    }
                }
            }
                break;
            default:
            {
                if (TextSignal != null)
                {
                    if (signal.signals.Count == 1)
                        TextSignal.text = signal.tipoSignal.ToString();
                    else
                        TextSignal.text = $"{signal.tipoSignal.ToString()} ({indexSignal})";
                }
            }
                break;
        }
    }

    private void Update()
    {
        if (MeshRendererBase != null)
        {
            switch (signal.tipoSignal)
            {
                case SignalBase.TipoSignalEnum.BOMBA:
                {
                    switch (signal.signals[indexSignal].Valor)
                    {
                        case 0:
                            MeshRendererBase.material.color = new Color(0.9f, 0.9f, 0.9f, 1f);
                            break;
                        case 1:
                            MeshRendererBase.material.color = Color.green;
                            break;
                        case 2:
                            MeshRendererBase.material.color = Color.red;
                            break;
                        case 3:
                            MeshRendererBase.material.color = Color.blue;
                            break;
                    }
                }
                    break;
                case SignalBase.TipoSignalEnum.PUERTA_ABIERTA:
                case SignalBase.TipoSignalEnum.FALLAC:
                case SignalBase.TipoSignalEnum.MANTENIMIENTO:
                case SignalBase.TipoSignalEnum.VOLTAJE:
                {
                    if (TextSignalValue != null)
                    {
                        TextSignalValue.text = signal.signals[indexSignal].Valor.ToString();
                    }
                }
                    break;
                default:
                {
                    if (TextSignalValue != null)
                    {
                        if (signal.signals[indexSignal].DentroRango)
                            TextSignalValue.text = signal.signals[indexSignal].Valor.ToString();
                        else
                            TextSignalValue.text = "-";
                    }
                }
                    break;
            }
        }
    }
}
