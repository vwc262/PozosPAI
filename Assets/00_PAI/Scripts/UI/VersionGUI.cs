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
        ControlRequest.singleton.UpdateVersionEvent.AddListener(UpdateVersion);
        ControlRequest.singleton.InitializeVersionEvent.AddListener(UpdateVersionText);
    }

    private void UpdateVersionText()
    {
        versionText.text = "" + ControlRequest.singleton.listRequestAPI[0].ServiceVersion;
    }

    private void UpdateVersion()
    {
        StartCountdownEvent.Invoke();
    }

}
