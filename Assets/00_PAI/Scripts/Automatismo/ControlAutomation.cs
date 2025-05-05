using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ControlAutomation : Singleton<ControlAutomation>
{
    public bool datosValidos = false; 
    public GameObject panelAutomation;

    [TabGroup("Automatismo")] public ControlAutomationSubestacion Sub_Teoloyucan;
    [TabGroup("Automatismo")] public ControlAutomationSubestacion Sub_FFCC;
    [TabGroup("Automatismo")] public ControlAutomationSubestacion Sub_Zumpango1;
    [TabGroup("Automatismo")] public ControlAutomationSubestacion Sub_Zumpango2;
    
    [TabGroup("Automatismo")] public GameObject prefabControlAutomatismo;
    [TabGroup("Automatismo")] public GameObject contentTeoloyucan;
    [TabGroup("Automatismo")] public GameObject contentFFCC;
    [TabGroup("Automatismo")] public GameObject contentZumpango1;
    [TabGroup("Automatismo")] public GameObject contentZumpango2;

    [TabGroup("UI")] public PlayMakerFSM Fsm_ControlAutomatizacion;
    [TabGroup("UI")] public PlayMakerFSM Fsm_ControlAnim;
    [TabGroup("UI")] public Button ramalTeoloyucanButton;
    [TabGroup("UI")] public Button ramalFFCCButton;
    [TabGroup("UI")] public Button ramalZumpangoButton;
    [TabGroup("UI")] public Button buttonToggleControlAutomatismo;
    [TabGroup("UI")] public TMPro.TMP_Text textMessage;
    [TabGroup("UI")] public Image imageMessage;
    
    public bool isActiveTeoloyucan;
    public bool isActiveZumpango;
    public bool isActiveFFCC;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (panelAutomation != null)
            panelAutomation.SetActive(false);

        if (buttonToggleControlAutomatismo != null)
        {
            buttonToggleControlAutomatismo.gameObject.SetActive(false);
            buttonToggleControlAutomatismo.interactable = false;
        }
    }

    public void enableControlAutomatismo()
    {
        datosValidos = true;
        
        if (buttonToggleControlAutomatismo != null)
        {
            if (ControlAccesoPozosPAI._singletonExists)
            {
                if (ControlAccesoPozosPAI.singleton.proyectos != 0)
                {
                    buttonToggleControlAutomatismo.gameObject.SetActive(true);
                    buttonToggleControlAutomatismo.interactable = true;
                }
                else
                {
                    buttonToggleControlAutomatismo.gameObject.SetActive(false);
                }
            }
            else
            {
                buttonToggleControlAutomatismo.gameObject.SetActive(true);
                buttonToggleControlAutomatismo.interactable = true;
            }
        }
    }
    
    public void ActualizarListas()
    {
        if (ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                ControlAccesoPozosPAI.Proyectos.Teoloyucan))
        {
            isActiveTeoloyucan = true;
            
            if (Sub_Teoloyucan != null)
                Sub_Teoloyucan.SetSubestacionByID(1);

            Sub_Teoloyucan.ClearLists();

            foreach (var sitioT in ControlDatosAux.singleton.listSitios.Where(
                         item => item.Estructura == (int)RequestAPI.Proyectos.Teoloyucan))
            {
                if (contentTeoloyucan != null && prefabControlAutomatismo != null)
                {
                    GameObject obj = Instantiate(prefabControlAutomatismo, contentTeoloyucan.transform);

                    obj.name = "Automation sitio: " + sitioT.nombre;
                    ControlAutomationSitio ASitio = obj.GetComponent<ControlAutomationSitio>();

                    if (ASitio != null)
                    {
                        ASitio.SetDataSitio(sitioT, Sub_Teoloyucan);
                        Sub_Teoloyucan.controlAutomatismoList.Add(ASitio);
                    }
                }
            }

            Sub_Teoloyucan.InitSubestacion();
            
            if (ramalTeoloyucanButton != null)
                ramalTeoloyucanButton.gameObject.SetActive(true);
        }
        else
        {
            if (ramalTeoloyucanButton != null)
                ramalTeoloyucanButton.gameObject.SetActive(false);
            
            isActiveTeoloyucan = false;
        }
        
        if (ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
            ControlAccesoPozosPAI.Proyectos.PozosAIFA))
        {
            isActiveFFCC = true;
            
            if (Sub_FFCC != null)
                Sub_FFCC.SetSubestacionByID(2);

            Sub_FFCC.ClearLists();

            foreach (var sitioT in ControlDatosAux.singleton.listSitios.Where(
                         item => item.Estructura == (int)RequestAPI.Proyectos.PozosAIFA))
            {
                if (contentFFCC != null && prefabControlAutomatismo != null)
                {
                    GameObject obj = Instantiate(prefabControlAutomatismo, contentFFCC.transform);

                    obj.name = "Automation sitio: " + sitioT.nombre;
                    ControlAutomationSitio ASitio = obj.GetComponent<ControlAutomationSitio>();

                    if (ASitio != null)
                    {
                        ASitio.SetDataSitio(sitioT, Sub_FFCC);
                        Sub_FFCC.controlAutomatismoList.Add(ASitio);
                    }
                }
            }

            Sub_FFCC.InitSubestacion();
            
            if (ramalFFCCButton != null)
                ramalFFCCButton.gameObject.SetActive(true);
        }
        else
        {
            if (ramalFFCCButton != null)
                ramalFFCCButton.gameObject.SetActive(false);
            
            isActiveFFCC = false;
        }
        
        if (ControlAccesoPozosPAI.singleton.proyectos.HasFlag(
                ControlAccesoPozosPAI.Proyectos.PozosZumpango))
        {
            isActiveZumpango = true;
            
            if (Sub_Zumpango1 != null)
                Sub_Zumpango1.SetSubestacionByID(3);

            Sub_Zumpango1.ClearLists();

            foreach (var sitioT in ControlDatosAux.singleton.listSitios.Where(
                         item =>
                         {
                             return item.Estructura == (int)RequestAPI.Proyectos.PozosZumpango &&
                                    item.automationData.idSubestacion == 3;
                         }))
            {
                if (contentZumpango1 != null && prefabControlAutomatismo != null)
                {
                    GameObject obj = Instantiate(prefabControlAutomatismo, contentZumpango1.transform);

                    obj.name = "Automation sitio: " + sitioT.nombre;
                    ControlAutomationSitio ASitio = obj.GetComponent<ControlAutomationSitio>();

                    if (ASitio != null)
                    {
                        ASitio.SetDataSitio(sitioT, Sub_Zumpango1);
                        Sub_Zumpango1.controlAutomatismoList.Add(ASitio);
                    }
                }
            }

            Sub_Zumpango1.InitSubestacion();

            if (Sub_Zumpango2 != null)
                Sub_Zumpango2.SetSubestacionByID(4);

            Sub_Zumpango2.ClearLists();

            foreach (var sitioT in ControlDatosAux.singleton.listSitios.Where(
                         item =>
                         {
                             return (item.Estructura == (int)RequestAPI.Proyectos.PozosZumpango) &&
                                    item.automationData.idSubestacion == 4;
                         }))
            {
                if (contentZumpango2 != null && prefabControlAutomatismo != null)
                {
                    GameObject obj = Instantiate(prefabControlAutomatismo, contentZumpango2.transform);

                    obj.name = "Automation sitio: " + sitioT.nombre;
                    ControlAutomationSitio ASitio = obj.GetComponent<ControlAutomationSitio>();

                    if (ASitio != null)
                    {
                        ASitio.SetDataSitio(sitioT, Sub_Zumpango2);
                        Sub_Zumpango2.controlAutomatismoList.Add(ASitio);
                    }
                }
            }

            Sub_Zumpango2.InitSubestacion();
            
            if (ramalZumpangoButton != null)
                ramalZumpangoButton.gameObject.SetActive(true);
        }
        else
        {
            if (ramalZumpangoButton != null)
                ramalZumpangoButton.gameObject.SetActive(false);
            
            isActiveZumpango = false;
        }
        
        if (isActiveTeoloyucan)
            SendEventFSM("teoloyucan");
        else if (isActiveFFCC)
            SendEventFSM("ffcc");
        else
            SendEventFSM("zumpango");
    }

    public void SimulaDatos()
    {
        //Sub_Teoloyucan.nominalVoltage = 220;
        Sub_Teoloyucan.toleranceVoltage = 10;
        Sub_Teoloyucan.startupTime = Random.Range(30,100);
        Sub_Teoloyucan.stabilitationTime = Random.Range(30,100);
        Sub_Teoloyucan.windowTime = Random.Range(30,100);
        
        //Sub_FFCC.nominalVoltage = 220;
        Sub_FFCC.toleranceVoltage = 10;
        Sub_FFCC.startupTime = Random.Range(30,100);
        Sub_FFCC.stabilitationTime = Random.Range(30,100);
        Sub_FFCC.windowTime = Random.Range(30,100);
        
        //Sub_Zumpango1.nominalVoltage = 220;
        Sub_Zumpango1.toleranceVoltage = 10;
        Sub_Zumpango1.startupTime = Random.Range(30,100);
        Sub_Zumpango1.stabilitationTime = Random.Range(30,100);
        Sub_Zumpango1.windowTime = Random.Range(30,100);
        
        //Sub_Zumpango2.nominalVoltage = 220;
        Sub_Zumpango2.toleranceVoltage = 10;
        Sub_Zumpango2.startupTime = Random.Range(30,100);
        Sub_Zumpango2.stabilitationTime = Random.Range(30,100);
        Sub_Zumpango2.windowTime = Random.Range(30,100);
        
        int cont = 0;
        foreach (var sitioT in ControlDatosAux.singleton.listSitios.Where(
                     item=>item.Estructura == (int)RequestAPI.Proyectos.Teoloyucan))
        {
            cont++;
            sitioT.automationData.index = cont;
            sitioT.automationData.isActiveAutomation = Random.value < 0.5f;
            //sitioT.automationData.nominalVoltage = Sub_Teoloyucan.nominalVoltage;
            sitioT.automationData.toleranceVoltage = Sub_Teoloyucan.toleranceVoltage;
            sitioT.automationData.starupTime = Sub_Teoloyucan.startupTime;
            sitioT.automationData.stabilitationTime = Sub_Teoloyucan.stabilitationTime;
            sitioT.automationData.windowTime = Sub_Teoloyucan.windowTime;
        }
        
        cont = 0;
        foreach (var sitioT in ControlDatosAux.singleton.listSitios.Where(
                     item=>item.Estructura == (int)RequestAPI.Proyectos.PozosAIFA))
        {
            cont++;
            sitioT.automationData.index = cont;
            sitioT.automationData.isActiveAutomation = Random.value < 0.5f;
            //sitioT.automationData.nominalVoltage = Sub_FFCC.nominalVoltage;
            sitioT.automationData.toleranceVoltage = Sub_FFCC.toleranceVoltage;
            sitioT.automationData.starupTime = Sub_FFCC.startupTime;
            sitioT.automationData.stabilitationTime = Sub_FFCC.stabilitationTime;
            sitioT.automationData.windowTime = Sub_FFCC.windowTime;
        }
        
        cont = 0;
        foreach (var sitioT in ControlDatosAux.singleton.listSitios.Where(
                     item=>
                     {
                         return item.Estructura == (int)RequestAPI.Proyectos.PozosZumpango &&
                                item.idSitio <= 1813;
                     }))
        {
            cont++;
            sitioT.automationData.index = cont;
            sitioT.automationData.isActiveAutomation = Random.value < 0.5f;
            //sitioT.automationData.nominalVoltage = Sub_Zumpango1.nominalVoltage;
            sitioT.automationData.toleranceVoltage = Sub_Zumpango1.toleranceVoltage;
            sitioT.automationData.starupTime = Sub_Zumpango1.startupTime;
            sitioT.automationData.stabilitationTime = Sub_Zumpango1.stabilitationTime;
            sitioT.automationData.windowTime = Sub_Zumpango1.windowTime;
        }
        
        cont = 0;
        foreach (var sitioT in ControlDatosAux.singleton.listSitios.Where(
                     item=>
                     {
                         return (item.Estructura == (int)RequestAPI.Proyectos.PozosZumpango) &&
                                item.idSitio > 1813;
                     }))
        {
            cont++;
            sitioT.automationData.index = cont;
            sitioT.automationData.isActiveAutomation = Random.value < 0.5f;
            //sitioT.automationData.nominalVoltage = Sub_Zumpango2.nominalVoltage;
            sitioT.automationData.toleranceVoltage = Sub_Zumpango2.toleranceVoltage;
            sitioT.automationData.starupTime = Sub_Zumpango2.startupTime;
            sitioT.automationData.stabilitationTime = Sub_Zumpango2.stabilitationTime;
            sitioT.automationData.windowTime = Sub_Zumpango2.windowTime;
        }
    }

    public void TogglePanelAutomation()
    {
        if (datosValidos)
        {
            if (panelAutomation != null)
                panelAutomation.SetActive(!panelAutomation.activeSelf);

            if (panelAutomation.activeSelf)
                ActualizarListas();
        }
    }

    public void ActiveAnimation()
    {
        if (Fsm_ControlAnim != null)
            Fsm_ControlAnim.SendEvent("send");
    }

    public static bool isValidInputPositiveInt(string input, ref int num)
    {
        try
        {
            num = int.Parse(input);

            if (num < 0)
            {
                num = 0;
                return false;
            }
            
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            num = 0;
            return false;
        }
    }

    public static int ValidaInputPositiveInt(string input)
    {
        try
        {
            int num = int.Parse(input);
            
            if (num < 0) 
                return 0;
            
            return num;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
    }
    
    public void SendEventFSM(string eventName)
    {
        if (Fsm_ControlAutomatizacion != null)
            Fsm_ControlAutomatizacion.SendEvent(eventName);
    }

    public void SetMessage(string message, Color colorMessage)
    {
        if (textMessage != null)
            textMessage.text = message;
        
        if (imageMessage != null)
            imageMessage.color = colorMessage;
    }
}
