using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DataRequestAPI : ControlJSON
{
    public Infraestructura infraestructura;
    public UpdateUnitySites updateUnitySites;
    public RespuestaTotalizadosPorFecha totalizadosPorFecha;
    public Historicos historicosBySitio;
    public List<Region> regiones;
}

[Serializable]
public class Region
{
    public string nombre;
    public int idRegion;
}
