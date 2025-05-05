using System;
using UnityEngine;

[Serializable]
public class EstacionAutomatismo
{
    public int IdEstacion;
    public int IdSegmento;
    public int Secuencia;
    public int VNominal;
    public int BanderaArranqueFallido;
    public int Automatismo;
    public int Version;
    public int BanderaActualizacion;

    public void SetDataEstacion(EstacionAutomatismo estacionAux)
    {
        IdEstacion = estacionAux.IdEstacion;
        IdSegmento = estacionAux.IdSegmento;
        Version = estacionAux.Version;
        Secuencia = estacionAux.Secuencia;
        Automatismo = estacionAux.Automatismo;
        BanderaArranqueFallido = estacionAux.BanderaArranqueFallido;
        VNominal = estacionAux.VNominal;
        BanderaActualizacion = estacionAux.BanderaActualizacion;
    }
}
