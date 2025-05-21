using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlParticular : Singleton<ControlParticular>
{
    public bool isActiveParticular;
    public ControlSitio sitio;

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
        if (ControlSelectedSitio._singletonExists)
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(UpdateInfoSitio);

        DeactivateParticular();

        StartCoroutine(UpdateUIPozo());
    }
    
    public void UpdateInfoSitio(ControlSitio _sitio)
    {
        sitio = _sitio;

        if (nombrePozo != null)
            nombrePozo.text = sitio.dataSitio.nombre;
    }

    public IEnumerator UpdateUIPozo()
    {
        while (UpdateLoop)
        {
            if (sitio != null && datosPozo != null)
            {
                datosPozo.text = "Abreviatura: " + sitio.dataSitio.abreviacion + "\n";

                List<SignalBase> gasto = sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.GASTO);
                
                if (gasto.Count>0)
                    if (gasto[0].DentroRango)
                        datosPozo.text += "\nGasto: " + gasto[0].Valor + "  l/s";

                List<SignalBase> presion = sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.PRESION);
                
                if (presion.Count > 0)
                    if (presion[0].DentroRango)
                        datosPozo.text += "\nPresion: " + presion[0].Valor + " km/cm2";

                List<SignalBase> totalizado = sitio.dataSitio.GetSignal(SignalBase.TipoSignalEnum.TOTALIZADO);
                
                if (totalizado.Count > 0)
                    if (totalizado[0].DentroRango)
                        datosPozo.text += "\nTotalizado: " + totalizado[0].Valor + " m3";
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
        
        if (controlParticualres != null && sitio != null)
            controlParticualres.SetActiveParticularByID(sitio.dataSitio.idSitio);
        
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