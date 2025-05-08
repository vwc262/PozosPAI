using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class ControlSitio
{
    public static float umbralGreen = 15;
    
    public float diferencia;
    public bool dataInTime;
    
    public DataSitio dataSitio;
    public DataSitioAforo dataAforo = new DataSitioAforo();
    public ControlMarcadorSitio controlMarcadorMap;
    public ControlUISitio controlUIsitio;
    public int indexBomba;

    public bool SelectedForAnalitics;
    public float timeLastCommand;

    public bool GetStatusConexionSitio()
    {
        dataInTime = false;
        
        DateTime parsedDate;

        if (DateTime.TryParse(dataSitio.fecha, out parsedDate))
        {
            diferencia = (float)(DateTime.Now - parsedDate).TotalMinutes;

            if (diferencia < umbralGreen)
            {
                dataInTime = true;
            }
        }
        
        return dataInTime;
    }

    public void SeleccionarSitio()
    {
        if (controlUIsitio != null)
            controlUIsitio.SeleccionarSitio();
        
        if (controlMarcadorMap != null)
            controlMarcadorMap.SeleccionarSitio();
    }

    public void DeseleccionarSitio()
    {
        if (controlUIsitio != null)
            controlUIsitio.DeseleccionarSitio();
        
        if (controlMarcadorMap != null)
            controlMarcadorMap.DeseleccionarSitio();
    }

    public float GetGasto()
    {
        if (dataSitio.gasto.Count>0)
            if (dataSitio.gasto[0].DentroRango)
                return dataSitio.gasto[0].Valor;

        return 0;
    }
    
    public float GetPresion()
    {
        if (dataSitio.presion.Count>0)
            if (dataSitio.presion[0].DentroRango)
                return dataSitio.presion[0].Valor;

        return 0;
    }
    
    public float GetTotalizado()
    {
        if (dataSitio.totalizado.Count>0)
            if (dataSitio.totalizado[0].DentroRango)
                return dataSitio.totalizado[0].Valor;

        return 0;
    }

    public float GetBomba()
    {
        if (dataSitio.bomba.Count>0)
            //if (dataSitio.bomba[0].DentroRango)
                return dataSitio.bomba[0].Valor;

        return 0;
    }

    public float GetGastoAnalitics()
    {
        if (dataAforo.isAforado)
            return dataAforo.gasto;

        return GetGasto();
    }
    
    public void incrementIndexBomba()
    {
        indexBomba++;
        
        if (indexBomba >= dataSitio.bomba.Count)
            indexBomba = 0;
        
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.ChangeIndexBomba.Invoke(this);
    }
    
    public void SetIndexBomba(int index)
    {
        indexBomba = index;
        
        if (indexBomba >= dataSitio.bomba.Count)
            indexBomba = 0;
        
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.ChangeIndexBomba.Invoke(this);
    }
    
    public string smallDescription
    {
        get
        {
            string descrip = $"{dataSitio.nombre}   /  {dataSitio.abreviacion}   /   {dataSitio.Estructura}";
            return descrip;
        }
    } 
}
