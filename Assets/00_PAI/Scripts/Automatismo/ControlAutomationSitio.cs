using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ControlAutomationSitio : MonoBehaviour
{
    public DataSitio dataSitio;
    
    public ControlAutomationSubestacion subestacion;
    
    public TMPro.TMP_Text texto_Nombre;
    public TMPro.TMP_Text texto_Orden;
    public TMPro.TMP_Text texto_Voltage;
    public TMPro.TMP_InputField Input_Voltage;
    public TMPro.TMP_Text texto_Tolerance;
    public TMPro.TMP_Text texto_StartupTime;
    public TMPro.TMP_Text texto_StabilitationTime;
    public TMPro.TMP_Text texto_WindowTime;
    
    public Toggle toggle_Automation;
    public Button Button_UP;
    public Button Button_DOWN;
    public Button Button_Alarma;

    public float updateRate = 5;
    private float countdown;

    public bool isDataChanged;
    public bool isActiveAutomation = false;
    public int index;
    public int VNominal;

    // private void Start()
    // {
    //     SetEnableEditors(false);
    // }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0)
        {
            UpdateData();
            countdown = updateRate;
        }            
    }

    public void recreateIndex(int _index)
    {
        index = _index;

        if (dataSitio.automationData.ConfIndex != index)
        {
            print($"Indice cambiado: {dataSitio.nombre}");
            isDataChanged = true;
        }
    }

    public void UpdateEditores()
    {
        if (texto_Orden != null)
            texto_Orden.text = GetIndex();
        
        if (toggle_Automation != null)
            toggle_Automation.isOn = GetIsActiveAutomation();
        
        if (Input_Voltage != null)
            Input_Voltage.text = GetNominalVoltage();
    }

    public void UpdateData()
    {
        if (dataSitio != null)
        {
            if (dataSitio.automationData != null)
            {
                if (texto_Orden != null)
                    texto_Orden.text = GetIndex();
                
                if (texto_Tolerance != null)
                    texto_Tolerance.text = GetTolarence();

                if (texto_StartupTime != null)
                    texto_StartupTime.text = GetStartupTime();

                if (texto_StabilitationTime != null)
                    texto_StabilitationTime.text = dataSitio.automationData.stabilitationTime.ToString();

                if (texto_WindowTime != null)
                    texto_WindowTime.text = GetWindowTime();
                
                if (Button_Alarma != null)
                    Button_Alarma.gameObject.SetActive(dataSitio.automationData.AutomationError);
            }
        }
    }

    public void SetDataSitio(DataSitio _dataSitio, ControlAutomationSubestacion _subEstacion)
    {
        dataSitio = _dataSitio;
        subestacion = _subEstacion;

        if (texto_Nombre != null)
            texto_Nombre.text = dataSitio.nombre;

        if (dataSitio.automationData != null)
        {
            index = dataSitio.automationData.ConfIndex;
            
            if (texto_Orden != null)
                texto_Orden.text = GetIndex();

            isActiveAutomation = GetIsActiveAutomation();
            
            if (toggle_Automation != null)
                toggle_Automation.isOn = GetIsActiveAutomation();

            VNominal = dataSitio.automationData.nominalVoltage;
                
            if (texto_Voltage != null)
                texto_Voltage.text = GetNominalVoltage();
            
            if (Input_Voltage != null)
                Input_Voltage.text = GetNominalVoltage();

            if (texto_Tolerance != null)
                texto_Tolerance.text = GetTolarence();

            if (texto_StartupTime != null)
                texto_StartupTime.text = GetStartupTime();

            if (texto_StabilitationTime != null)
                texto_StabilitationTime.text = dataSitio.automationData.stabilitationTime.ToString();

            if (texto_WindowTime != null)
                texto_WindowTime.text = GetWindowTime();
        }
    }

    public string GetIndex()
    {
        return subestacion.inputEnable? index.ToString() : 
            subestacion.useConfigurationData ?
            dataSitio.automationData.ConfIndex.ToString() :
            dataSitio.automationData.index.ToString();
    }
    
    public bool GetIsActiveAutomation()
    {
        return subestacion.useConfigurationData ?
            dataSitio.automationData.ConfIsActiveAutomation:
            dataSitio.automationData.isActiveAutomation;
    }
    
    public string GetWindowTime()
    {
        // return subestacion.useConfigurationData ?
        //     dataSitio.automationData.ConfWindowTime.ToString():
        //     dataSitio.automationData.windowTime.ToString();
        
        return dataSitio.automationData.ConfWindowTime.ToString();
    }

    public string GetStartupTime()
    {
        // return subestacion.useConfigurationData ?
        //     dataSitio.automationData.ConfStarupTime.ToString():
        //     dataSitio.automationData.starupTime.ToString();
        
        return dataSitio.automationData.ConfStarupTime.ToString();
    }

    public string GetTolarence()
    {
        // return subEstacion.useConfigurationData ?
        //     dataSitio.automationData.ConfToleranceVoltage.ToString():
        //     dataSitio.automationData.toleranceVoltage.ToString();
        
        return dataSitio.automationData.ConfToleranceVoltage.ToString();
    }
    
    public string GetNominalVoltage()
    {
        return subestacion.useConfigurationData ?
            dataSitio.automationData.ConfNominalVoltage.ToString():
            dataSitio.automationData.nominalVoltage.ToString();
    }

    public void SetActiveAutomation(bool active)
    {
        isActiveAutomation = active;

        if (dataSitio.automationData.ConfIsActiveAutomation != isActiveAutomation)
        {
            print($"Automatizacion cambiado: {dataSitio.nombre}");
            isDataChanged = true;
        }
    }

    public void SetIndex(int _index)
    {
        index = _index;
        
        if (texto_Orden != null)
            texto_Orden.text = GetIndex();
        
        if (dataSitio.automationData.ConfIndex != index)
        {
            print($"Indice cambiado: {dataSitio.nombre}");
            isDataChanged = true;
        }
    }

    public void MoveUp()
    {
        if (subestacion != null)
            subestacion.MoveUP(this);
    }
    
    public void MoveDown()
    {
        if (subestacion != null)
            subestacion.MoveDown(this);
    }

    public void SetNominalVoltage(string voltage)
    {
        if (voltage.Contains("-"))
        {
            if (Input_Voltage != null)
                Input_Voltage.text = voltage.Replace("-", "");
        }
        else
        {
            VNominal = ControlAutomation.ValidaInputPositiveInt(voltage);

            if (texto_Voltage != null)
                texto_Voltage.text = VNominal.ToString();

            if (dataSitio.automationData.ConfNominalVoltage != VNominal)
            {
                print($"Voltage nominal cambiado: {dataSitio.nombre}");
                isDataChanged = true;
            }
        }
    }

    public void SetToleranceVoltage(float porcentage)
    {
        this.dataSitio.automationData.toleranceVoltage = porcentage;
        
        if (texto_Tolerance != null)
            texto_Tolerance.text = this.dataSitio.automationData.toleranceVoltage.ToString();
    }

    public void SetStartupTime(int startupTime)
    {
        this.dataSitio.automationData.starupTime = startupTime;
        
        if (texto_StartupTime != null)
            texto_StartupTime.text = this.dataSitio.automationData.starupTime.ToString();
    }

    public void SetStabilitationTime(int stabilitationTime)
    {
        this.dataSitio.automationData.stabilitationTime = stabilitationTime;
        
        if (texto_StabilitationTime != null)
            texto_StabilitationTime.text = this.dataSitio.automationData.stabilitationTime.ToString();
    }

    public void SetWindowTime(int windowTime)
    {
        this.dataSitio.automationData.windowTime = windowTime;
        
        if (texto_WindowTime != null)
            texto_WindowTime.text = this.dataSitio.automationData.windowTime.ToString();
    }

    public void SetEnableEditors(bool enable)
    {
        toggle_Automation.interactable = enable;
        Input_Voltage.interactable = enable;
        Button_UP.interactable = enable;
        Button_DOWN.interactable = enable;
        Button_Alarma.interactable = enable;
    }
    
    public void ActivateLoginUI()
    {
        if (ControlLogin._singletonExists)
            ControlLogin.singleton.ActivateLoginPanel();
    }

    public void SendDataReconocimientoAutomatismo()
    {
        subestacion.SendDataReconocimientoAutomatismo(dataSitio.idSitio % 100);
        
        if (ControlAutomation._singletonExists)
            ControlAutomation.singleton.ActiveAnimation();
    }
}
