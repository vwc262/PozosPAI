using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ControlSelectSitio : MonoBehaviour
{
    public Text textID;
    public Text textAlias;
    public Text textNombre;
    public Text textGasto;
    public InputField inputFieldGasto;
    public Text textPresion;
    public InputField inputFieldPresion;
    public Text textTotalizado;
    public Text textoVoltaje;
    public Text textFecha;
    public Text textNivel;
    
    public Image statusBomba;
    
    public Image selectedImage;
    public GameObject selectedBarsImage;
    
    public ControlSitio sitio;
    
    public Color statusColor;
    public bool DataInTime;
    public List<Image> imageUIStatus = new List<Image>();
    
    private Coroutine corrutinaTime;
    public float updateRate = 5;
    private float countdown;

    public Sprite statusBombaGreen;
    public Sprite statusBombaRed;
    public Sprite statusBombaBlue;
    public Sprite statusBombaGrey;

    public Image progressBarGasto;
    public Image progressBarPresion;
    
    public float MaxGasto = 120;
    public float MaxPresion = 10;

    public Sprite onlineSprite;
    public Sprite offlineSprite;

    public Image onlineStatusImage;

    public bool dataOverwrited;

    public bool selectedForAnalitics;
    
    //public DataPozoAnalitics dataPozo;
    public DataPozoAnalitics dataAforo;
    
    public Toggle toggleOverride;
    public Toggle toggleSelectForAnalitics;
    //public SimpleTooltip TooltipOverride;
    
    public Vector2 sizeDelta;

    public int UIStatus = -1;
    
    public GameObject panelBombas;
    public GameObject panelNivel;
    
    public virtual void Start()
    {
        sitio.controlMarcadorMap.sitioSeleccionadoEvent += ControlMarcadorSitioOnControlMarcadorSitioSeleccionadoEvent;
        sitio.controlMarcadorMap.controlSelectSitio = this;
        
        inputFieldGasto.onValueChanged.AddListener(SetAforoGasto);
        inputFieldPresion.onValueChanged.AddListener(SetAforoPresion);
        
        // if (TooltipOverride != null)
        //     TooltipOverride.enabled = false;
        
        if (panelNivel != null)
            panelNivel.SetActive(false);

        StartReset();
    }

    public virtual void StartReset(){}

    private void OnDestroy()
    {
        sitio.controlMarcadorMap.sitioSeleccionadoEvent -= ControlMarcadorSitioOnControlMarcadorSitioSeleccionadoEvent;
    }

    public void SetSelectedForAnalitics(bool val)
    {
        selectedForAnalitics = val;
    }
    
    public void SetOverwriteSitioData(bool val)
    {
        dataOverwrited = val;
        textGasto.gameObject.SetActive(!dataOverwrited);
        textPresion.gameObject.SetActive(!dataOverwrited);
        inputFieldGasto.gameObject.SetActive(dataOverwrited);
        inputFieldPresion.gameObject.SetActive(dataOverwrited);
        
        if (dataOverwrited)
        {
            updateDataAforoUI();
        }
        else
        {

        }
    }
    
    public void updateDataAforoUI()
    {
        inputFieldGasto.text = $"{dataAforo.gasto:F2}";
        inputFieldPresion.text = $"{dataAforo.presion:F2}";
    }

    public void SetAforoGasto(string val)
    {
        dataAforo.gasto = float.Parse(val);
    }
    
    public void SetAforoPresion(string val)
    {
        dataAforo.presion = float.Parse(val);
    }
     
    private void ControlMarcadorSitioOnControlMarcadorSitioSeleccionadoEvent(bool value)
    {
        selectedBarsImage.SetActive(value);
        
        if (value && ControlSitioUI.moveScrollBarOnSelect)
        {
            float scrollPos = 1 - transform.GetSiblingIndex() / (float)(transform.parent.childCount - 1);

            Scrollbar scrollbar = transform.parent.parent.parent.GetComponentInChildren<Scrollbar>();
            if (scrollbar != null)
                scrollbar.value = scrollPos;
        }
    }

    private void OnEnable()
    {
        UpdateData();
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            UpdateData();
            countdown = updateRate;
        }

        /*if (dataOverwrited)
        {
            dataPozo.fecha = dataAforo.fecha;
            dataPozo.gasto = dataAforo.gasto;
            dataPozo.presion = dataAforo.presion;
            dataPozo.totalizado = dataAforo.totalizado;
        }
        else
        {
            if (sitio.MyDataSitio.tipoSitioPozo == TipoSitioPozo.PozoLerma1
                || sitio.MyDataSitio.tipoSitioPozo == TipoSitioPozo.PozoLerma2)
            {
         
                if (sitio.MyDataSitio.gasto.Count > 0)
                    dataPozo.gasto = sitio.MyDataSitio.gasto[0].Valor;
                else
                    dataPozo.gasto = 0;
                
                if (sitio.MyDataSitio.presion.Count > 0)
                    dataPozo.presion = sitio.MyDataSitio.presion[0].Valor;
                else
                    dataPozo.presion = 0;
                
                if (sitio.MyDataSitio.totalizado.Count > 0)
                    dataPozo.totalizado = sitio.MyDataSitio.totalizado[0].Valor;
                else
                    dataPozo.totalizado = 0;
                
                dataPozo.fecha = sitio.MyDataSitio.fecha;;
            }

            if (sitio.MyDataSitio.tipoSitioPozo == TipoSitioPozo.EnConstruccion)
            {
                dataPozo.fecha = "---"; 
                dataPozo.gasto = 0;
                dataPozo.presion = 0;
                dataPozo.totalizado = 0;
            }
        }*/
    }

    public virtual void UpdateData(){ }

    public virtual void SetSitio(ControlSitio _controlMarcadorSitio)
    {
        this.sitio = _controlMarcadorSitio;

        if (sitio != null)
        {
            if (textID != null)
            {
                textID.text = $"{sitio.dataSitio.idSitioUnity}";
            }
            
            if (textAlias != null)
            {
                textAlias.text = sitio.dataSitio.abreviacion;
            }
            
            if (textNombre != null)
            {
                textNombre.text = sitio.dataSitio.nombre;
            }
        }
    }

    //[Button]
    public void SetSelectedInGUI(bool val)
    {
        var color = selectedImage.color;
        color.a = val ? 0.25f : 0;
        selectedImage.color = color;
    }

    public void SelectSitio()
    {
        sitio.controlMarcadorMap.SetSelectedSitio();
        sitio.controlMarcadorMap.AddToSelectanbles();
    }
    
    public void SetStatusBomba(Image _statusBomba, int indexBomba)
    {
        var bombaSprite = statusBombaGrey;

        if (sitio.dataSitio.bomba.Count > indexBomba)
        {
            bombaSprite = statusBombaGrey;

            //if (sitio.dataInTime)
            //{
            // if (sitio.MyDataSitio.bomba[0].DentroRango)
            // {
            switch (sitio.dataSitio.bomba[indexBomba].Valor)
            {
                case 1:
                    bombaSprite = statusBombaGreen;
                    break;
                case 2:
                    bombaSprite = statusBombaRed;
                    break;
                case 3:
                    bombaSprite = statusBombaBlue;
                    break;
            }
            // }
            //}
            
            _statusBomba.gameObject.SetActive(true);
            _statusBomba.sprite = bombaSprite;
        }
        else
        {
            _statusBomba.gameObject.SetActive(false);
        }
    }
}