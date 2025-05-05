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
        RequestAPI.Instance.UpdateVersionEvent.AddListener(UpdateVersion);
        RequestAPI.Instance.InitializeVersionEvent.AddListener(UpdateVersionText);
    }

    private void UpdateVersionText()
    {
        versionText.text = "" + RequestAPI.Instance.ServiceVersion;
    }

    private void UpdateVersion()
    {
        StartCountdownEvent.Invoke();
    }

}
