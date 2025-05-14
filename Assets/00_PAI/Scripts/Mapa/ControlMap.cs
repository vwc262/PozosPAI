using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlMap : Singleton<ControlMap>
{
    [TabGroup("Map")]public float longitud0;
    [TabGroup("Map")]public float latitud0;
    [TabGroup("Map")]public float longitudCenterPozos;
    [TabGroup("Map")]public float latitudCenterPozos;
    [TabGroup("Map")]public float longitudOffset;
    [TabGroup("Map")]public float latitudOffset;
    [TabGroup("Map")]public float spanLongitud;
    [TabGroup("Map")]public float spanLatitud;
    
    [TabGroup("Contenedores")]public GameObject contenedorMapa;
    [TabGroup("Contenedores")]public GameObject contenedorMarcadores;
    
    public float longitudMapa;
    public float latitudMapa;
    
    public void SetGlobalDataMapa(bool moveMapa)
    {
        longitudCenterPozos = ControlDatos.singleton.minLongitud +
                              ((ControlDatos.singleton.maxLongitud - ControlDatos.singleton.minLongitud) / 2f);
        latitudCenterPozos = ControlDatos.singleton.minLatitud +
                             ((ControlDatos.singleton.maxLatitud - ControlDatos.singleton.minLatitud) / 2f);
            
        if (moveMapa)
        {
            longitud0 = longitudCenterPozos;
            latitud0 = latitudCenterPozos;
            longitud0 += longitudOffset;
            latitud0 += latitudOffset;
        }
        else
        {
            longitud0 = longitudMapa;
            latitud0 = latitudMapa;
        }
        
        Gps2UnityConverter.longitud0 = longitud0;
        Gps2UnityConverter.latitud0 = latitud0;
        Gps2UnityConverter.Altitude = 0;
        Gps2UnityConverter.spanLongitud = spanLongitud;
        Gps2UnityConverter.spanLatitud = spanLatitud;
    }
    
    [Button]
    public void SetPositionMapa()
    {
        contenedorMapa.transform.position = transform.position + Gps2UnityConverter.GPS2Unity(latitudMapa, longitudMapa);
    }
}
