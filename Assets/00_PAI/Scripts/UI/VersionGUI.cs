using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class VersionGUI : MonoBehaviour
{
    // Hola Boy
    public Text versionText;

    public UnityEvent StartCountdownEvent;

    void Start()
    {
        RequestAPI.singleton.UpdateVersionEvent.AddListener(UpdateVersion);
        RequestAPI.singleton.InitializeVersionEvent.AddListener(UpdateVersionText);
    }

    private void UpdateVersionText()
    {
        versionText.text = "" + RequestAPI.singleton.ServiceVersion;
    }

    private void UpdateVersion()
    {
        StartCountdownEvent.Invoke();
    }

}
