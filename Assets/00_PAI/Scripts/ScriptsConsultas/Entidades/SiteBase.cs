using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class SiteBase
{
    [SerializeField] public int Id;
    [SerializeField] public bool Enlace;
    [SerializeField] public string Tiempo;
    [SerializeField] public bool FallaAC;
    [SerializeField] public float Voltaje;
    [SerializeField] public List<SignalsContainer> SignalsContainer;
}
