using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SignalsContainer
{
    [SerializeField] public int TipoSignal;
    [SerializeField] public List<SignalBase> Signals;
    // public SignalsContainer()
    // {
    //     Signals = new List<SignalBase>();
    // }
}
