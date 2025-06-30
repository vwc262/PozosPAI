using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ControlUISitio : MonoBehaviour
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

    //public bool dataOverwrited;

    public Toggle toggleOverride;
    public Toggle toggleSelectForAnalitics;
    
    public Vector2 sizeDelta;

    public int UIStatus = -1;
    
    public GameObject panelBombas;
    public GameObject panelNivel;
    
    public virtual void Start()
    {
        inputFieldGasto.onValueChanged.AddListener(SetAforoGasto);
        inputFieldPresion.onValueChanged.AddListener(SetAforoPresion);
        
        if (panelNivel != null)
            panelNivel.SetActive(false);

        StartReset();
    }

    public virtual void StartReset(){}

    public void SetSelectedForAnalitics(bool val)
    {
        sitio.SelectedForAnalitics = val;
    }
    
    public void SetOverwriteSitioData(bool val)
    {
        sitio.dataAforo.isAforado = val;
        textGasto.gameObject.SetActive(!sitio.dataAforo.isAforado);
        textPresion.gameObject.SetActive(!sitio.dataAforo.isAforado);
        inputFieldGasto.gameObject.SetActive(sitio.dataAforo.isAforado);
        inputFieldPresion.gameObject.SetActive(sitio.dataAforo.isAforado);
        
        if (sitio.dataAforo.isAforado)
        {
            updateDataAforoUI();
        }
    }
    
    public void updateDataAforoUI()
    {
        inputFieldGasto.text = $"{sitio.dataAforo.gasto:F2}";
        inputFieldPresion.text = $"{sitio.dataAforo.presion:F2}";
    }

    public void SetAforoGasto(string val)
    {
        sitio.dataAforo.gasto = float.Parse(val);
    }
    
    public void SetAforoPresion(string val)
    {
        sitio.dataAforo.presion = float.Parse(val);
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

    public void SeleccionarSitio()
    {
        selectedBarsImage.SetActive(true);
        
        if (ControlSitiosUI_Lista.moveScrollBarOnSelect)
        {
            float scrollPos = 1 - transform.GetSiblingIndex() / (float)(transform.parent.childCount - 1);

            Scrollbar scrollbar = transform.parent.parent.parent.GetComponentInChildren<Scrollbar>();
            if (scrollbar != null)
                scrollbar.value = scrollPos;
        }
    }
    
    public void DeseleccionarSitio()
    {
        selectedBarsImage.SetActive(false);
    }

    public void SelectSitio()
    {
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.SetSelectedSitio(sitio);
    }
    
    public void SetStatusBomba(Image _statusBomba, int indexBomba)
    {
        var bombaSprite = statusBombaGrey;
        
        List<SignalBase> bomba = sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.BOMBA);

        if (bomba.Count > indexBomba)
        {
            bombaSprite = statusBombaRed;
            
            switch (bomba[indexBomba].Valor)
            {
                case 1:
                    bombaSprite = statusBombaGreen;
                    break;
                case 2:
                    bombaSprite = statusBombaGrey;
                    break;
                case 3:
                    bombaSprite = statusBombaBlue;
                    break;
            }
            
            _statusBomba.gameObject.SetActive(true);
            _statusBomba.sprite = bombaSprite;
        }
        else
        {
            _statusBomba.gameObject.SetActive(false);
        }
    }
}