using System;
using UnityEngine;

[Serializable]
public class EstacionesSistema
{
    public RequestAPI.Proyectos sistema;
    public EstacionesAutomatismo estacionesAutomatismo;
    
    public Coroutine corrutinaEstaciones;
    public Coroutine corrutinaEstacionesConf;
}
