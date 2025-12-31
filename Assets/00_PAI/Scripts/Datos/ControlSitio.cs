using System;
using System.Collections.Generic;
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

    public bool isSelected;
    public bool SelectedForAnalitics;
    public float timeLastCommand;

    public int indexRequestAPI;

    
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
        isSelected = true;
        
        if (controlUIsitio != null)
            controlUIsitio.SeleccionarSitio();
        
        if (controlMarcadorMap != null)
            controlMarcadorMap.SeleccionarSitio();
        
        if (ControlBombas_PAI._singletonExists)
            ((ControlBombas_PAI)ControlBombas_PAI.singleton).SendEventFSM("hide");
        
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.CloseLoginPanel();
    }

    public void DeseleccionarSitio()
    {
        isSelected = false;
        
        if (controlUIsitio != null)
            controlUIsitio.DeseleccionarSitio();
        
        if (controlMarcadorMap != null)
            controlMarcadorMap.DeseleccionarSitio();
    }

    public float GetGasto(int index = 0)
    {
        List<SignalBase> gasto = dataSitio.GetSignal(SignalBase.TipoSignalEnum.GASTO);
        
        if (gasto.Count > index)
            if (gasto[index].DentroRango)
                return gasto[index].Valor;

        return 0;
    }
    
    public float GetNivel(int index = 0)
    {
        List<SignalBase> signal = dataSitio.GetSignal(SignalBase.TipoSignalEnum.NIVEL);
        
        if (signal.Count > index)
            if (signal[index].DentroRango)
                return signal[index].Valor;

        return 0;
    }
    
    public int GetIndiceNivel(int index = 0)
    {
        List<SignalBase> signal = dataSitio.GetSignal(SignalBase.TipoSignalEnum.NIVEL);
        
        if (signal.Count > index)
            if (signal[index].DentroRango)
                return signal[index].IndiceImagen;

        return 0;
    }
    
    public float GetPresion()
    {
        List<SignalBase> presion = dataSitio.GetSignal(SignalBase.TipoSignalEnum.PRESION);
        
        if (presion.Count>0)
            if (presion[0].DentroRango)
                return presion[0].Valor;

        return 0;
    }
    
    public float GetTotalizado(int index = 0)
    {
        List<SignalBase> totalizado = dataSitio.GetSignal(SignalBase.TipoSignalEnum.TOTALIZADO);
        
        if (totalizado.Count > index)
            if (totalizado[index].DentroRango)
                return totalizado[index].Valor;

        return 0;
    }

    public float GetBomba()
    {
        List<SignalBase> bomba = dataSitio.GetSignal(SignalBase.TipoSignalEnum.BOMBA);
        
        if (bomba.Count>0)
            //if (bomba[0].DentroRango)
                return bomba[0].Valor;

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
        List<SignalBase> bomba = dataSitio.GetSignal(SignalBase.TipoSignalEnum.BOMBA);
        
        indexBomba++;
        
        if (indexBomba >= bomba.Count)
            indexBomba = 0;
        
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.ChangeIndexBomba.Invoke(this);
    }
    
    public void SetIndexBomba(int index)
    {
        List<SignalBase> bomba = dataSitio.GetSignal(SignalBase.TipoSignalEnum.BOMBA);
        
        indexBomba = index;
        
        if (indexBomba >= bomba.Count)
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
