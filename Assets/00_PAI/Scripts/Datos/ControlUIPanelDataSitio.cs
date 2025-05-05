using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ControlUIPanelDataSitio : MonoBehaviour
{
    public SitioGPS sitioSeleccionado;
    
    public List<GameObject> points = new List<GameObject>();
    
    [TabGroup("UI")] public Text textPresion;
    [TabGroup("UI")] public Text EtiquetaBomba;
    [TabGroup("UI")] public Text textBomba;
    [TabGroup("UI")] public Text textGasto;
    [TabGroup("UI")] public Text textTotalizado;
    [TabGroup("UI")] public Text textBateria;
    [TabGroup("UI")] public Text textNivel;
    
    public GameObject GO_Nivel;

    public GameObject bomba_0;
    public GameObject bomba_1;
    public GameObject bomba_2;
    public GameObject bomba_3;
    
    public float updateRate = 5;
    private float countdown;
    
    // Start is called before the first frame update
    public void Start()
    {
        if (ControlUpdateUI._singletonExists)
        {
            ControlUpdateUI.singleton.SitioSeleccionadoSitioGPS.AddListener(UpdateInfoSitio);
            ControlUpdateUI.singleton.ChangeIndexBomba.AddListener(UpdateUIIndexBomba);
        }
    }

    public void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            if (sitioSeleccionado != null)
                UpdateInfoUISitio(sitioSeleccionado);
            countdown = updateRate;
        }
    }

    public void UpdateInfoSitio(SitioGPS sitio)
    {
        sitioSeleccionado = sitio;
        UpdateInfoUISitio(sitioSeleccionado);
    }

    public virtual void UpdateInfoUISitio(SitioGPS sitio)
    {
        if (textPresion != null)
        {
            if (sitio.MyDataSitio.presion.Count > 0)
            {
                if (sitio.MyDataSitio.presion[0].DentroRango)
                    textPresion.text = GetString2decimals(sitio.MyDataSitio.presion[0].Valor) + " Kg/cm2";
                else
                    textPresion.text = "-";
            }
            else
                textPresion.text = "N/A";
        }

        if (sitio.statusDataInTime == 1)
            SetPointsColor(Color.green);
        else
            SetPointsColor(Color.red);
        
        UpdateUIBomba(sitio);
        
        if (textGasto != null)
        {
            if (sitio.MyDataSitio.gasto.Count > 0)
            {
                if (sitio.MyDataSitio.gasto[0].DentroRango)
                    textGasto.text = GetString2decimals(sitio.MyDataSitio.gasto[0].Valor) + " L/s";
                else
                    textGasto.text = "-";
            }
            else
                textGasto.text = "N/A";
        }
        
        if (textTotalizado != null)
        {
            if (sitio.MyDataSitio.totalizado.Count > 0)
            {
                if (sitio.MyDataSitio.gasto[0].DentroRango)
                    textTotalizado.text = $"{sitio.MyDataSitio.totalizado[0].Valor:F0}" + " m3";
                else
                    textTotalizado.text = "-";
            }
            else
                textTotalizado.text = "N/A";
        }
        
        if (textBateria != null)
        {
            if (sitio.MyDataSitio.Baterias.Count > 0)
            {
                textBateria.text = "";
                
                foreach (var bateria in sitio.MyDataSitio.Baterias)
                {
                    if (textBateria.text != "")
                        textBateria.text += "\n";
                        
                    if (bateria.DentroRango)
                        textBateria.text += GetString2decimals(bateria.Valor) + " V";
                    else
                        textBateria.text += "-";
                }
            }
            else
            {
                textBateria.text = GetString2decimals(sitio.MyDataSitio.voltaje) + " V";
            }
        }
        
        if (textNivel != null && GO_Nivel != null)
        {
            if (sitio.MyDataSitio.nivel.Count > 0)
            {
                GO_Nivel.SetActive(true);
                
                if (sitio.MyDataSitio.nivel[0].DentroRango)
                    textNivel.text = GetString2decimals(sitio.MyDataSitio.nivel[0].Valor) + " m";
                else
                    textNivel.text = "-\n";
            }
            else
            {
                GO_Nivel.SetActive(false);
            }
        }
    }

    public void UpdateUIBomba(SitioGPS sitio)
    {
        if (bomba_0 != null) bomba_0.SetActive(true);
        if (bomba_1 != null) bomba_1.SetActive(false);
        if (bomba_2 != null) bomba_2.SetActive(false);
        if (bomba_3 != null) bomba_3.SetActive(false);
        
        if (EtiquetaBomba != null)
        {
            EtiquetaBomba.text = $"Bomba [{sitio.indexBomba + 1}]";
        }
        
        if (textBomba != null)
        {
            if (sitio.MyDataSitio.bomba.Count > sitio.indexBomba)
            {
                //textBomba.text = DataSitio.GetStringBombaStatus((int)_DataSitio.bomba[0].Valor);

                // if (sitio.MyDataSitio.bomba[sitio.indexBomba].DentroRango)
                // {
                    textBomba.text = DataSitio.GetStringBombaStatus((int)sitio.MyDataSitio.bomba[sitio.indexBomba].Valor);

                    switch (sitio.MyDataSitio.bomba[sitio.indexBomba].Valor)
                    {
                        case 0:
                            if (bomba_0 != null) bomba_0.SetActive(true);
                            break;
                        case 1:
                            if (bomba_0 != null) bomba_0.SetActive(false);
                            if (bomba_1 != null) bomba_1.SetActive(true);
                            break;
                        case 2:
                            if (bomba_0 != null) bomba_0.SetActive(false);
                            if (bomba_2 != null) bomba_2.SetActive(true);
                            break;
                        case 3:
                            if (bomba_0 != null) bomba_0.SetActive(false);
                            if (bomba_3 != null) bomba_3.SetActive(true);
                            break;
                    }
                //}
                // else
                // {
                //     textBomba.text = "-";
                // }
            }
            else
                textBomba.text = "N/A";
        }
    }

    [Button]
    public void incrementIndexBomba()
    {
        sitioSeleccionado.incrementIndexBomba();
    }

    public void UpdateUIIndexBomba(SitioGPS sitio)
    {
        UpdateUIBomba(sitio);
    }

    public string GetString2decimals(float value)
    {
        return $"{value:F2}";
    }
    
    public void SetPointsColor(Color _color)
    {
        foreach (var point in points)
        {
            var image = point.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
                image.color = _color;
        }
    }
}
