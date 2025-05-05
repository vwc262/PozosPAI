using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SignalsDescriptionContainerC
{
    [SerializeField] public int TipoSignal;
    [SerializeField] public List<SignalDescription> SignalsDescription;

    // public SignalsDescriptionContainer()
    // {
    //     SignalsDescription = new List<SignalDescription>();
    // }
}
