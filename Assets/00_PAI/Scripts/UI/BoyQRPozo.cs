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
		if (ControlSelectedSitio._singletonExists)
			ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);
	}

	private void UpdateInfoSitio(ControlSitio _sitio)
	{
		gpsQR = $"{_sitio.dataSitio.latitud},{_sitio.dataSitio.longitud}";

		gpsText.text = $"{ConvertDecimelToGrades(_sitio.dataSitio.latitud)}, {ConvertDecimelToGrades(_sitio.dataSitio.longitud)}";
		
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
