using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class RequestBoy
{
    public enum TipoPromedio
    {
        HORAS,
        DIAS,
        MES
    }

    public int IdSignal;
    public string FechaInicial;
    public string FechaFinal;
    //public TipoPromedio tipoPromedio;
}

// [Serializable]
// public class DatoHistorico
// {
//     public float Valor;
//     public string Tiempo;
//     public int IdSignal;
// }

[Serializable]
public class Historicos
{
    public List<Reporte> Presion = new List<Reporte>();
    public List<Reporte> Gasto = new List<Reporte>();
    public List<Reporte> Totalizado = new List<Reporte>();
    public List<Reporte> Bomba = new List<Reporte>();
    public List<Reporte> Nivel = new List<Reporte>();
    public string RequestResult;
}
