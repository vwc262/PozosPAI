using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlParticular : Singleton<ControlParticular>
{
    public bool isActiveParticular;
    [FormerlySerializedAs("sitioSeleccionado")] public ControlMarcadorSitio controlMarcadorSitioSeleccionado;

    public GameObject CameraParticular;
    public GameObject PanelUIParticular;

    public List<CoustomKeyboard> coustomKeyboardList_particular;
    public List<CoustomKeyboard> coustomKeyboardList_Navigation;

    //public ControlBombasUI_3D controlBombas;
    
    public ControlListParticulares controlParticualres;

    [TabGroup("UI")] public float waitUpdateUITime = 3;
    [TabGroup("UI")] public bool UpdateLoop = true;
    [TabGroup("UI")] public TMPro.TMP_Text nombrePozo;
    [TabGroup("UI")] public TMPro.TMP_Text datosPozo;
    
    private void Start()
    {
        if (ControlUpdateUI._singletonExists)
        {
            ControlUpdateUI.singleton.SitioSeleccionadoSitioGPS.AddListener(UpdateInfoSitio);
        }

        DeactivateParticular();

        StartCoroutine(UpdateUIPozo());
    }
    
    public void UpdateInfoSitio(ControlMarcadorSitio controlMarcadorSitio)
    {
        controlMarcadorSitioSeleccionado = controlMarcadorSitio;

        if (nombrePozo != null)
            nombrePozo.text = controlMarcadorSitioSeleccionado.sitio.dataSitio.nombre;
    }

    public IEnumerator UpdateUIPozo()
    {
        while (UpdateLoop)
        {
            if (controlMarcadorSitioSeleccionado != null && datosPozo != null)
            {
                datosPozo.text = "Abreviatura: " + controlMarcadorSitioSeleccionado.sitio.dataSitio.abreviacion + "\n";

                if (controlMarcadorSitioSeleccionado.sitio.dataSitio.gasto.Count>0)
                    if (controlMarcadorSitioSeleccionado.sitio.dataSitio.gasto[0].DentroRango)
                        datosPozo.text += "\nGasto: " + controlMarcadorSitioSeleccionado.sitio.dataSitio.gasto[0].Valor + "  l/s";

                if (controlMarcadorSitioSeleccionado.sitio.dataSitio.presion.Count > 0)
                    if (controlMarcadorSitioSeleccionado.sitio.dataSitio.presion[0].DentroRango)
                        datosPozo.text += "\nPresion: " + controlMarcadorSitioSeleccionado.sitio.dataSitio.presion[0].Valor + " km/cm2";

                if (controlMarcadorSitioSeleccionado.sitio.dataSitio.totalizado.Count > 0)
                    if (controlMarcadorSitioSeleccionado.sitio.dataSitio.totalizado[0].DentroRango)
                        datosPozo.text += "\nTotalizado: " + controlMarcadorSitioSeleccionado.sitio.dataSitio.totalizado[0].Valor + " m3";
            }

            yield return new WaitForSeconds(waitUpdateUITime);
        }
    }

    [Button]
    public void ActivateParticular()
    {
        isActiveParticular = true;
        SetActiveParticular(isActiveParticular);
    }

    [Button]
    public void DeactivateParticular()
    {
        isActiveParticular = false;
        SetActiveParticular(isActiveParticular);
    }
    
    [Button]
    public void ChangeActiveParticular()
    {
        isActiveParticular = !isActiveParticular;
        SetActiveParticular(isActiveParticular);
    }
    
    public void SetActiveParticular(bool _active)
    {
        CameraParticular.SetActive(_active);
        PanelUIParticular.SetActive(_active);
        
        if (controlParticualres != null && controlMarcadorSitioSeleccionado != null)
            controlParticualres.SetActiveParticularByID(controlMarcadorSitioSeleccionado.sitio.dataSitio.idSitio);
        
        foreach(var coustomKeyboard in coustomKeyboardList_particular)
        {
            coustomKeyboard.SetEnable(_active);
        }
        
        foreach(var coustomKeyboard in coustomKeyboardList_Navigation)
        {
            coustomKeyboard.SetEnable(!_active);
        }
        
        // if (controlBombas!= null)
        //     controlBombas.SetEnableCollidersControl(!_active);
    }

    public void InitCoroutineActivateParticular()
    {
        StartCoroutine(CoroutineActivateParticular());
    }

    public IEnumerator CoroutineActivateParticular()
    {
        yield return new WaitForSeconds(0.1f);

        ActivateParticular();
    }
}