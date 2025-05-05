using System;
using UnityEngine;

[Serializable]
public class SignalDescription : SignalBase
{
    [SerializeField] public int IdTipoSignal;
    [SerializeField] public string Nombre;
    [SerializeField] public int Ordinal;
    [SerializeField] public int Linea;
    [SerializeField] public Semaforo Semaforo;
}
