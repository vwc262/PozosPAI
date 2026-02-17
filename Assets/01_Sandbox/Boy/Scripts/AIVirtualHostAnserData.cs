using System;
using UnityEngine;

[Serializable]
public class AIVirtualHostAnserData
{
    public EstadoAnimo animo;
    public Acciones accion;
    public Tareas tares;
    public string dialogo;
    public Vector2 direccionMovimiento;
    public float distanciaMovimiento;
}

public enum EstadoAnimo
{
    Alegria,
    Tristeza,
    Miedo,
    Ira,
    Desagrado,
    Sorpresa
}

public enum Acciones
{
    SolicitudNoProgramada,
    MostrarInfo,
    RealizarTarea,
    Platicar
}

public enum Tareas
{
    NoTarea,
    Movimiento,
    Ataque,
    Defensa,
    Curacion,
    Bailar
}

