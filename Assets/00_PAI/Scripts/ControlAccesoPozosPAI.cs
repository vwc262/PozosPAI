using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ControlAccesoPozosPAI : Singleton<ControlAccesoPozosPAI>
{
    [Flags]
    public enum Proyectos
    {
        GustavoAMadero = 1 << 0,
        Padierna = 1 << 1,
        Lerma = 1 << 2,
        Yaqui = 1 << 3,
        Chalmita = 1 << 4,
        Encharcamientos = 1 << 5,
        SistemaCutzamala = 1 << 6,
        Lumbreras8Sitios = 1 << 7,
        SantaCatarina = 1 << 8,
        Chiconautla = 1 << 9,
        Sorpasso = 1 << 10,
        EscudoNacional = 1 << 11,
        ClimatologicasHidrometricas = 1 << 12,
        Teoloyucan = 1 << 13,
        Pruebas = 1 << 14,
        LineaMorada = 1 << 15,
        PozosAIFA = 1 << 16,
        PozosZumpango = 1 << 17,
        PozosReyesFerrocarril = 1 << 18,
        PozosPAI = 1 << 19,
        Todos = GustavoAMadero |
                Padierna |
                Lerma |
                Yaqui |
                Chalmita |
                Encharcamientos |
                SistemaCutzamala |
                Lumbreras8Sitios |
                SantaCatarina |
                Chiconautla |
                Sorpasso |
                EscudoNacional |
                ClimatologicasHidrometricas |
                Teoloyucan |
                Pruebas |
                LineaMorada |
                PozosAIFA |
                PozosZumpango |
                PozosReyesFerrocarril |
                PozosPAI
    }

    [TabGroup("Ramales")] public Proyectos proyectos;
    [TabGroup("Configuracion")] public Configuration configuration;

    [TabGroup("Ramales")] public bool isInteractableAllUISitios;
    [TabGroup("Ramales")] public bool colapseList;
    [TabGroup("Ramales")] public bool DisableUISitio;
    [TabGroup("UI")] public string password;
    [TabGroup("UI")] public GameObject panelControlAcceso;
    [TabGroup("UI")] public GameObject panelPassword;
    [TabGroup("UI")] public GameObject panelAcceso;
    [TabGroup("UI")] public TMP_InputField inputFieldPassword;
    [TabGroup("UI")] public Toggle toggleTeoloyucan;
    [TabGroup("UI")] public Toggle toggleAIFA;
    [TabGroup("UI")] public Toggle toggleZumpango;
    [TabGroup("UI")] public Toggle toggleAplicationInFocus;
    [TabGroup("UI")] public KeyCode tecla1,tecla2,tecla3;

    protected override void Awake()
    {
        base.Awake();
        
        LoadConfigurationProyectos();
        LoadConfiguration();
        
        if (panelControlAcceso != null)
            panelControlAcceso.SetActive(false);
        if (panelPassword != null)
            panelPassword.SetActive(true);
        if (panelAcceso != null)
            panelAcceso.SetActive(false);
        
        if (AplicationControl._singletonExists)
            AplicationControl.singleton.validaAplicationInFocus = configuration.validaAplicationInFocus;
        
        if (toggleTeoloyucan != null) toggleTeoloyucan.isOn = singleton.proyectos.HasFlag(
            Proyectos.Teoloyucan);
        
        if (toggleAIFA != null) toggleAIFA.isOn = singleton.proyectos.HasFlag(
            Proyectos.PozosAIFA);
        
        if (toggleZumpango != null) toggleZumpango.isOn = singleton.proyectos.HasFlag(
            Proyectos.PozosZumpango);

        if (toggleAplicationInFocus != null)
            toggleAplicationInFocus.isOn = configuration.validaAplicationInFocus;
    }

    private void Update()
    {
        if (Input.GetKey(tecla1) && 
            Input.GetKey(tecla2) &&
            Input.GetKeyDown(tecla3))
        {
            if (panelControlAcceso != null)
            {
                panelControlAcceso.SetActive(!panelControlAcceso.activeSelf);
                
                if (panelPassword != null)
                    panelPassword.SetActive(true);
                if (panelAcceso != null)
                    panelAcceso.SetActive(false);
            }
        }
    }

    [TabGroup("Ramales")] [Button]
    public void SaveConfigurationProyectos()
    {
        SetInt("proyectos", (int)proyectos);
    }

    [TabGroup("Ramales")] [Button]
    public void LoadConfigurationProyectos()
    {
        proyectos = (Proyectos)GetInt("proyectos");
    }
    
    public void SetInt(string KeyName, int Value)
    {
        PlayerPrefs.SetInt(KeyName, Value);
    }

    public int GetInt(string KeyName)
    {
        return PlayerPrefs.GetInt(KeyName);
    }
    
    public void SetString(string KeyName, string Value)
    {
        PlayerPrefs.SetString(KeyName, Value);
    }

    public string GetString(string KeyName)
    {
        return PlayerPrefs.GetString(KeyName);
    }

    public void ValidaPasswordControlAcceso()
    {
        if (inputFieldPassword != null && panelPassword != null)
        {
            if (inputFieldPassword.text == password)
            {
                panelPassword.SetActive(false);
                panelAcceso.SetActive(true);
                inputFieldPassword.text = "";
            }
        }
    }
    
    [TabGroup("Ramales")] [Button]
    public void SetActiveProyectoTeoloyucan(bool Value)
    {
        if (singleton.proyectos.HasFlag(Proyectos.Teoloyucan) != Value)
        {
            proyectos ^= Proyectos.Teoloyucan;
            SaveConfigurationProyectos();
        }
    }
    
    [TabGroup("Ramales")] [Button]
    public void SetActiveProyectoAIFA(bool Value)
    {
        if (singleton.proyectos.HasFlag(Proyectos.PozosAIFA) != Value)
        {
            proyectos ^= Proyectos.PozosAIFA;
            SaveConfigurationProyectos();
        }
    }
    
    [TabGroup("Ramales")] [Button]
    public void SetActiveProyectoZumpango(bool Value)
    {
        if (singleton.proyectos.HasFlag(Proyectos.PozosZumpango) != Value)
        {
            proyectos ^= Proyectos.PozosZumpango;
            SaveConfigurationProyectos();
        }
    }

    public void SetActiveAplicationFocus(bool Value)
    {
        configuration.validaAplicationInFocus = Value;
        SaveConfiguration();
        
        if (AplicationControl._singletonExists)
            AplicationControl.singleton.validaAplicationInFocus = configuration.validaAplicationInFocus;
    }
    
    [TabGroup("Configuracion")] [Button]
    public void SaveConfiguration()
    {
        SetString("configuration", configuration.GetJsonString());
    }

    [TabGroup("Configuracion")] [Button]
    public void LoadConfiguration()
    {
        configuration.SetDataFromJson(GetString("configuration"));
    }
}

[Serializable]
public class Configuration
{
    public bool validaAplicationInFocus;
    
    public string GetJsonString()
    {
        return JsonUtility.ToJson(this);
    }
    
    public void SetDataFromJson(string json)
    {
        if (!string.IsNullOrEmpty(json))
            JsonUtility.FromJsonOverwrite(json, this);
    }
}
