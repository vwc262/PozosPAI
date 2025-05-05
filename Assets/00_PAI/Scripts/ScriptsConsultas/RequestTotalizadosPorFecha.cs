using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestTotalizadosPorFecha
{
	public DateTime fechaInicial;
	public DateTime fechaFinal;
}

[Serializable]
public class TotalizadoPorSitio
{
	public int ID = 0;
	public float valorInicial;

	public float valorFinal;

	public float Diferencia;
	public bool HasNull;

}

[Serializable]
public class RespuestaTotalizadosPorFecha
{
	public List<TotalizadoPorSitio> ListaTotalizadoPorSitio = new List<TotalizadoPorSitio>();
}






