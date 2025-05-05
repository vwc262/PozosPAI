using System;
using UnityEngine;

[Serializable]
public class SignalBase
{
    public enum TipoSignalEnum
    {
        Default,
        NIVEL,
        PRESION,
        GASTO,
        TOTALIZADO,
        VALVULA_ANALOGICA,
        VALVULA_DISCRETA,
        BOMBA,
        PERILLA_BOMBA,
        PERILLA_GENERAL,
        VOLTAJE,
        ENLACE,
        FALLAC,
        TIEMPO,
        MANTENIMIENTO,
        PUERTA_ABIERTA,
        VOLTAJE_RANGO,
        CORRIENTE_RANGO,
        POTENCIA_TOTAL,
        FACTOR_POTENCIA,
        PRECIPITACION,
        TEMPERATURA,
        HUMEDAD,
        RADIACIONSOLAR,
        INTENSIDAD,
        DIRECCION
    }
    
    [SerializeField] public int IdSignal;
    [SerializeField] public float Valor;
    [SerializeField] public bool DentroRango;
    [SerializeField] public int IndiceImagen;
    [SerializeField] public int TipoSignal;
}
