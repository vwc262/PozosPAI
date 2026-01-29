using System;
using System.Collections.Generic;

[Serializable]
public class ListAverages
{
    public int IdProyecto;
    public List<AverageSitio> Items;
}

[Serializable]
public class AverageSitio
{
    public int IdSignal;
    public int IndexSignal;
    public string IdEstacion;
    public string nombre;
    public string TipoSignal;
    public string Fecha;
    public float Promedio;
}
