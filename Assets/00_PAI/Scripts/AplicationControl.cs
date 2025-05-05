using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class AplicationControl : Singleton<AplicationControl>
{
    public int targetFrameRate = 30;
    
    public bool isAplicationInFocus;
    public bool validaAplicationInFocus;

    public float LastTimeInFocus;
    public float LastTimeOutFocus;
    public float TimeInFocus;
    public float TimeOutFocus;
    public float TimeOutFocusLimit;

    public TMPro.TMP_Text textTimeOut;
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
    }

    [Button]
    public void RestartPC()
    {
        System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
    }

    private void Update()
    {
        isAplicationInFocus = Application.isFocused;
        
        if (isAplicationInFocus)
        {
            LastTimeInFocus = Time.time;
            TimeInFocus = Time.time - LastTimeOutFocus;
            TimeOutFocus = 0;

            if (textTimeOut != null)
                textTimeOut.text = "";
        }
        else
        {
            LastTimeOutFocus = Time.time;
            TimeOutFocus = Time.time - LastTimeInFocus;
            TimeInFocus = 0;
            
            if (textTimeOut != null)
                textTimeOut.text = $"{TimeOutFocus:F2}";
        }
        
#if !UNITY_EDITOR
        if (validaAplicationInFocus && TimeOutFocus > TimeOutFocusLimit)
            RestartPC();
#endif
    }

    // void OnApplicationFocus(bool hasFocus)
    // {
    //     isAplicationInFocus = hasFocus;
    // }
}
