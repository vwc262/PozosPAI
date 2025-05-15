using System;
using UnityEngine;

[Serializable]
public class EstacionesSistema
{
    public EstructurasAPI.Proyectos sistema;
    public EstacionesAutomatismo estacionesAutomatismo;
    
    public Coroutine corrutinaEstaciones;
    public Coroutine corrutinaEstacionesConf;
}
