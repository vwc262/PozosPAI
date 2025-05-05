using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Observaciones : ControlJSON
{
    public List<Observacion> ListObservaciones;
}

[Serializable]
public class Observacion
{
    public int id;
    public string observacion;
}


