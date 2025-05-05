using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gps2UnityConverter
{
    // Hola Boy
	
    public static float longitud0 = -99.6029f;
    public static float latitud0 = 19.4272f;
    public static float spanLongitud = 105100;
    public static float spanLatitud = 111300;
    public static float maxAltitude = 1000;

    public static Vector3 GPS2Unity(float latitud, float longitud)
    {
	    var pos = (Vector3.right * (longitud - longitud0)) * spanLongitud +
						(Vector3.forward * (latitud - latitud0)) * spanLatitud +
						(Vector3.up * maxAltitude);

	    return pos;
    }
    
    public static Vector3 Unity2GPS(Vector3 posUnity)
    {
	    var pos = new Vector3(
		    (posUnity.z/spanLatitud)+latitud0,
		    (posUnity.x/spanLongitud)+longitud0,
	              0);

	    return pos;
    }
}
