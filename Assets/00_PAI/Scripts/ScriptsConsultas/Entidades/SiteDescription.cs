using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SiteDescription : SiteBase
{
    [SerializeField] public string Nombre;
    [SerializeField] public string Abreviacion;
    [SerializeField] public float Latitud;
    [SerializeField] public float Longitud;
    [SerializeField] public int Grupo;
    [SerializeField] public int TipoEstacion;
    [SerializeField] public string NombreCompleto;
    [SerializeField] public string NombreCorto;
    [SerializeField] public List<SignalsDescriptionContainerC> SignalsDescriptionContainer;
    //[SerializeField] public string Direccion;
    //[SerializeField] public string PseudoNombre;
    //[SerializeField] public int Estructura;
    //[SerializeField] public int TipoLumbrera;
    //[SerializeField] public int IdWeb;
    
    public string smallDescription
    {
        get
        {
            string descrip = $"{Nombre}   /  {Abreviacion}  /  {((TipoSitioPozo)TipoEstacion).ToString()}";
            return descrip;
        }
    } 
}

public enum TipoSitioPozo
{
    none = 0,
    Pozo,
    Repetidor,
    EnConstruccion
}