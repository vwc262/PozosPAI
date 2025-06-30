using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BalanceGastoGUI : Singleton<BalanceGastoGUI>
{
    public PlayMakerFSM analiticsFSM;
    
    private void Update()
    {
        UpdateUI();
    }

    public virtual void UpdateUI()
    {

    }
    
    public void SendEventFSMPanelAnalitics(string eventName)
    {
        if (analiticsFSM != null)
            analiticsFSM.SendEvent(eventName);
    }
}
