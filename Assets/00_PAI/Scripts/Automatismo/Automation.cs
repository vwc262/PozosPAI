using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class Automation
{
    public int idSubestacion;
    public bool isActiveAutomation;
    public bool AutomationError;
    public int index;
    
    public int starupTime;
    public int stabilitationTime;
    public int windowTime;
    
    public int nominalVoltage;
    public float toleranceVoltage;

    public bool ConfIsActiveAutomation;
    public int ConfIndex;
    public int ConfStarupTime;
    public int ConfWindowTime;
    public int ConfNominalVoltage;
    public float ConfToleranceVoltage;

    public void SetDataAutomation(Automation automation)
    {
        isActiveAutomation = automation. isActiveAutomation;
        AutomationError = automation. AutomationError;
        index = automation.index;
        starupTime = automation.starupTime;
        stabilitationTime = automation.stabilitationTime;
        windowTime = automation.windowTime;
        nominalVoltage = automation.nominalVoltage;
        toleranceVoltage = automation.toleranceVoltage;
        
        ConfIsActiveAutomation = automation.ConfIsActiveAutomation;
        ConfIndex = automation.ConfIndex;
        ConfStarupTime = automation.ConfStarupTime;
        ConfWindowTime = automation.ConfWindowTime;
        ConfNominalVoltage = automation.ConfNominalVoltage;
        ConfToleranceVoltage = automation.ConfToleranceVoltage;
    }
}
