using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ControlSelectSitio_PAI : ControlSelectSitio
{
    private RectTransform rt;
    private RectTransform rt_button;
    
    public Button buttonSeleccion;
    
    public Image statusBomba_1;
    public Image statusBomba_2;
    public Image statusBomba_3;
    public Image statusBomba_4;
    public Image statusBomba_5;

    public bool ControlAccesSitio;
    public GameObject PanelDisabledSitio;
    
    public GameObject perillaOff;
    public GameObject perillaRemoto;
    public GameObject perillaLocal;
    public GameObject automatismoOff;
    public GameObject automatismoOn;
    public GameObject automatismoError;

    public override void Start()
    {
        base.Start();
        
        if (RequestAPI_Auto._singletonExists)
            RequestAPI_Auto.singleton.datosAutomatismoActualizados.AddListener(UpdateAutomatismo);
    }

    public override  void UpdateData()
    {
        if (sitio != null)
        {
            textID.text = $"{SitioGPS_PAI.GetIDSitiosPAI(sitio.MyDataSitio.abreviacion)}";
            textAlias.text = sitio.MyDataSitio.abreviacion;
            textNombre.text = sitio.MyDataSitio.nombre;
            DataInTime = sitio.dataInTime;
            onlineStatusImage.sprite = DataInTime ? onlineSprite : offlineSprite;
            statusColor = sitio.statusColor;
            statusColor.a = 1;

            if (sitio.MyDataSitio.bomba.Count == 1)
            {
                if (UIStatus != 1)
                {
                    statusBomba.gameObject.SetActive(true);
                    UIStatus = 1;
                }

                SetStatusBomba(statusBomba, 0);
            }
            else if (sitio.MyDataSitio.bomba.Count > 1)
            {
                UpdateNumBombas();
            }
            else
            {
                if (UIStatus != 0)
                {
                    statusBomba.gameObject.SetActive(false);
                    UIStatus = 0;
                }
            }

            textFecha.text = ControlDateTime_PAI.GetDateFormat_DMAH(sitio.MyDataSitio.fecha);

            if (textoVoltaje != null)
                textoVoltaje.text = $"{sitio.MyDataSitio.voltaje:F2} V";

            if (!dataOverwrited)
            {
                if (sitio.MyDataSitio.gasto.Count > 0)
                {
                    if (sitio.MyDataSitio.gasto[0].DentroRango)
                    {
                        textGasto.text = $"{sitio.MyDataSitio.gasto[0].Valor:F2}";

                        if (progressBarGasto != null)
                        {
                            progressBarGasto.transform.parent.gameObject.SetActive(true);
                            progressBarGasto.fillAmount = sitio.MyDataSitio.gasto[0].Valor / MaxGasto;
                        }
                    }
                    else
                    {
                        textGasto.text = "-";

                        if (progressBarGasto != null)
                        {
                            progressBarGasto.transform.parent.gameObject.SetActive(false);
                            progressBarGasto.fillAmount = 0;
                        }
                    }
                }
                else
                {
                    textGasto.text = "N/A";

                    if (progressBarGasto != null)
                    {
                        progressBarGasto.transform.parent.gameObject.SetActive(false);
                        progressBarGasto.fillAmount = 0;
                    }
                }

                if (sitio.MyDataSitio.presion.Count > 0)
                {
                    if (sitio.MyDataSitio.presion[0].DentroRango)
                    {
                        textPresion.text = $"{sitio.MyDataSitio.presion[0].Valor:F2}";

                        if (progressBarPresion != null)
                        {
                            progressBarPresion.transform.parent.gameObject.SetActive(true);
                            progressBarPresion.fillAmount = sitio.MyDataSitio.presion[0].Valor / MaxPresion;
                        }
                    }
                    else
                    {
                        textPresion.text = "-";

                        if (progressBarPresion != null)
                        {
                            progressBarPresion.transform.parent.gameObject.SetActive(false);
                            progressBarPresion.fillAmount = 0;
                        }
                    }
                }
                else
                {
                    textPresion.text = "N/A";

                    if (progressBarPresion != null)
                    {
                        progressBarPresion.transform.parent.gameObject.SetActive(false);
                        progressBarPresion.fillAmount = 0;
                    }
                }
            }
            else
            {
                if (progressBarGasto != null)
                {
                    progressBarGasto.transform.parent.gameObject.SetActive(true);
                    progressBarGasto.fillAmount = float.Parse(inputFieldGasto.text) / MaxGasto;
                }

                if (progressBarPresion != null)
                {
                    progressBarPresion.transform.parent.gameObject.SetActive(true);
                    progressBarPresion.fillAmount = float.Parse(inputFieldPresion.text) / MaxPresion;
                }

            }

            if (sitio.MyDataSitio.totalizado.Count > 0)
            {
                textTotalizado.text = sitio.MyDataSitio.totalizado[0].DentroRango
                    ? $"{sitio.MyDataSitio.totalizado[0].Valor:F0}"
                    : "-";
            }
            else
            {
                textTotalizado.text = "N/A";
            }

            if (textNivel != null)
            {
                if (sitio.MyDataSitio.nivel.Count > 0)
                {
                    if (sitio.MyDataSitio.nivel[0].DentroRango)
                    {
                        textNivel.text = $"Nivel: {sitio.MyDataSitio.nivel[0].Valor} m";
                    }
                    else
                    {
                        textNivel.text = $"Nivel: 0 m";
                    }
                }
            }

            UpdatePerilla();
            UpdateAutomatismo();
        }
    }
    
    public void UpdateNumBombas()
    {
        if (UIStatus != 2)
        {
            statusBomba.gameObject.SetActive(false);

            //RectTransform rt = this.GetComponent<RectTransform>();
            rt.sizeDelta = new Vector2(sizeDelta.x, sizeDelta.y * 2.2f);

            if (buttonSeleccion != null)
            {
                rt_button.sizeDelta = new Vector2(rt_button.sizeDelta.x, sizeDelta.y * 2.2f);
                
                VerticalLayoutGroup VLayout = gameObject.transform.parent.GetComponent<VerticalLayoutGroup>();

                if (VLayout != null)
                {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(VLayout.GetComponent<RectTransform>());
                    Debug.Log("Refresh UI");
                }
            }

            if (panelBombas != null)
                panelBombas.SetActive(true);

            if (panelNivel != null)
                panelNivel.SetActive(true);
            
            
            UIStatus = 2;
        }

        if (statusBomba_1 != null)
        {
            SetStatusBomba(statusBomba_1, 0);
            SetStatusBomba(statusBomba_2, 1);
            SetStatusBomba(statusBomba_3, 2);
            SetStatusBomba(statusBomba_4, 3);
            SetStatusBomba(statusBomba_5, 4);
        }
    }
    
    public override void StartReset()
    {
        rt = this.GetComponent<RectTransform>();
        sizeDelta = rt.sizeDelta;
        
        if (buttonSeleccion != null) 
            rt_button = buttonSeleccion.GetComponent<RectTransform>();
    }
    
    public override void SetSitio(SitioGPS _sitio)
    {
        sitio = _sitio;

        if (_sitio != null)
        {
            if (textID != null)
            {
                textID.text = $"{_sitio.MyDataSitio.idSitioUnity}";
            }
            
            if (textAlias != null)
            {
                textAlias.text = _sitio.MyDataSitio.abreviacion;
            }
            
            if (textNombre != null)
            {
                textNombre.text = _sitio.MyDataSitio.nombre;
            }

            SetControlAccesoUI();
        }
    }

    public void SetControlAccesoUI()
    {
        if (PanelDisabledSitio != null)
        {
            //Debug.Log("Panel Disabled Sitio: " + (RequestAPI.Proyectos)sitio.MyDataSitio.Estructura);
            if (ControlAccesoPozosPAI.singleton.isInteractableAllUISitios)
            {
                PanelDisabledSitio.SetActive(false);
            }
            else
            {
                switch ((RequestAPI.Proyectos)sitio.MyDataSitio.Estructura)
                {
                    case RequestAPI.Proyectos.Teoloyucan:
                        ControlAccesSitio = ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                            ControlAccesoPozosPAI.Proyectos.Teoloyucan);
                        break;

                    case RequestAPI.Proyectos.PozosZumpango:
                        ControlAccesSitio = ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                            ControlAccesoPozosPAI.Proyectos.PozosZumpango);
                        break;

                    // case RequestAPI.Proyectos.PozosReyesFerrocarril:
                    //     ControlAccesSitio = ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                    //         ControlAccesoPozosPAI.Proyectos.PozosReyesFerrocarril);
                    //     break;

                    case RequestAPI.Proyectos.PozosAIFA:
                        ControlAccesSitio = ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                            ControlAccesoPozosPAI.Proyectos.PozosAIFA);
                        break;
                }

                PanelDisabledSitio.SetActive(!ControlAccesSitio);

                if (ControlAccesoPozosPAI.singleton.DisableUISitio)
                    gameObject.SetActive(ControlAccesSitio);
            }
        }
    }

    public void UpdatePerilla()
    {
        if (sitio.MyDataSitio.PerillaBomba.Count > 0)
        {
            switch (sitio.MyDataSitio.PerillaBomba[0].Valor)
            {
                case 0://OFF
                    if (perillaOff != null) perillaOff.SetActive(true);
                    if (perillaRemoto != null) perillaRemoto.SetActive(false);
                    if (perillaLocal != null) perillaLocal.SetActive(false);
                    break;
                case 1://Remoto
                    if (perillaOff != null) perillaOff.SetActive(false);
                    if (perillaRemoto != null) perillaRemoto.SetActive(true);
                    if (perillaLocal != null) perillaLocal.SetActive(false);
                    break;
                case 2://Local
                    if (perillaOff != null) perillaOff.SetActive(false);
                    if (perillaRemoto != null) perillaRemoto.SetActive(false);
                    if (perillaLocal != null) perillaLocal.SetActive(true);
                    break;
            }
        }
    }

    public void UpdateAutomatismo()
    {
        if (sitio.MyDataSitio.automationData.AutomationError)
        {
            if (automatismoError != null) automatismoError.SetActive(true);
            if (automatismoOn != null) automatismoOn.SetActive(false);
            if (automatismoOff != null) automatismoOff.SetActive(false);
        }
        else if (sitio.MyDataSitio.automationData.isActiveAutomation)
        {
            if (automatismoError != null) automatismoError.SetActive(false);
            if (automatismoOn != null) automatismoOn.SetActive(true);
            if (automatismoOff != null) automatismoOff.SetActive(false);
        }
        else
        {
            if (automatismoError != null) automatismoError.SetActive(false);
            if (automatismoOn != null) automatismoOn.SetActive(false);
            if (automatismoOff != null) automatismoOff.SetActive(true);
        }
    }

    public void OpenParticular()
    {
        if (ParticularManager._singletonExists)
            ParticularManager.singleton.LoadParticularScene();
    }
}
