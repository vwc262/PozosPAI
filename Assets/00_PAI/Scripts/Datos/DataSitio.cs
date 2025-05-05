using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class DataSitio
{
    public int idSitioUnity;
    public int idSitio;
    public string nombre;
    public string abreviacion;
    public string fecha;
    public bool enlace;
    public bool fallaAC;
    
    public float voltaje;

    public TipoSitioPozo tipoSitioPozo;
    
    public int Estructura;
    
    public float longitud;
    public float latitud;
    
    public List<SignalBase> nivel = new List<SignalBase>();
    public List<SignalBase> bomba = new List<SignalBase>();
    public List<SignalBase> presion = new List<SignalBase>();
    public List<SignalBase> gasto = new List<SignalBase>();
    public List<SignalBase> totalizado = new List<SignalBase>();
    public List<SignalBase> Baterias = new List<SignalBase>();
    public List<SignalBase> PerillaBomba = new List<SignalBase>();
    public List<SignalBase> PerillaGeneral = new List<SignalBase>();
    
    public List<SignalBase> Voltajes_Motor = new List<SignalBase>();
    public List<SignalBase> Corrientes_Motor = new List<SignalBase>();

    public string observaciones;
    
    //---
    public Automation automationData = new Automation();
    
    public void SetDataSitio(DataSitio _data)
    {
        this.idSitioUnity = _data.idSitioUnity;
        this.idSitio = _data.idSitio;
        this.nombre = _data.nombre;
        this.abreviacion = _data.abreviacion;
        this.fecha = _data.fecha;
        this.enlace = _data.enlace;
        this.fallaAC = _data.fallaAC;
        this.voltaje = _data.voltaje;
        this.Estructura = _data.Estructura;
        this.tipoSitioPozo = _data.tipoSitioPozo;
        this.longitud = _data.longitud;
        this.latitud = _data.latitud;
        this.nivel = _data.nivel;
        this.bomba = _data.bomba;
        this.presion = _data.presion;
        this.gasto = _data.gasto;
        this.totalizado = _data.totalizado;
        this.Baterias = _data.Baterias;
        this.PerillaBomba = _data.PerillaBomba;
        this.PerillaGeneral = _data.PerillaGeneral;
        this.Voltajes_Motor = _data.Voltajes_Motor;
        this.Corrientes_Motor = _data.Corrientes_Motor;
    }
    
    public static string GetStringBombaStatus(int valor)
    {
        string status = "-";

        switch (valor)
        {
            case 0: status = "No disponible"; break;
            case 1: status = "Encendida"; break;
            case 2: status = "Apagada"; break;
            case 3: status = "Con falla"; break;
        }

        return status;
    }
    
    public string smallDescription
    {
        get
        {
            string descrip = $"{nombre}   /  {abreviacion}   /   {tipoSitioPozo.ToString()}";
            return descrip;
        }
    } 
}