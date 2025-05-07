using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BoyQRPozo : MonoBehaviour
{
	public string baseQR = "https:/maps.google.com/?q=";
	public string gpsQR = "9.9,9.9";
	public Color darkColor = Color.black;
	public Color lightColor = Color.white;

	public Text gpsText;
	
	void Start()
	{
		if (ControlUpdateUI._singletonExists)
			ControlUpdateUI.singleton.SitioSeleccionadoSitioGPS.AddListener(UpdateGPS);

	}

	private void UpdateGPS(ControlMarcadorSitio marcador)
	{
		gpsQR = $"{marcador.sitio.dataSitio.latitud},{marcador.sitio.dataSitio.longitud}";

		gpsText.text = $"{ConvertDecimelToGrades(marcador.sitio.dataSitio.latitud)}, {ConvertDecimelToGrades(marcador.sitio.dataSitio.longitud)}";
		
		Texture2D qrTexture = QRGenerator.EncodeString(baseQR+gpsQR, darkColor, lightColor);
		
		GetComponent<Renderer>().material.mainTexture = qrTexture;
		
	}


	public string ConvertDecimelToGrades(float _angle)
	{
		

		int angle = (int)_angle;
		int minutes = (int)((_angle - angle)*60);
		float seconds = ((_angle - angle)*60 - minutes)*60;

		string res = $"{angle}°,{minutes}',{seconds}''";
		return res;
	}
}
