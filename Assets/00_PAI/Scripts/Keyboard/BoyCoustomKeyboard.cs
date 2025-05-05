using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class BoyCoustomKeyboard : Singleton<BoyCoustomKeyboard>
{
    // Hola Boy
    public bool sendEvents;
    public bool debugLog;
    
    [FoldoutGroup("Keys")]public UnityEvent Key1_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key2_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key3_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key4_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key5_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key6_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key7_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key8_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key9_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key10_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key11_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key12_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key13_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key14_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key15_Event;
    [FoldoutGroup("Keys")]public UnityEvent Key16_Event;

    [FoldoutGroup("Knobs")]public UnityEvent Knob1_L_Event;
    [FoldoutGroup("Knobs")]public UnityEvent Knob1_R_Event;
    [FoldoutGroup("Knobs")]public UnityEvent Knob1_C_Event;
    [FoldoutGroup("Knobs")]public UnityEvent Knob2_L_Event;
    [FoldoutGroup("Knobs")]public UnityEvent Knob2_R_Event;
    [FoldoutGroup("Knobs")]public UnityEvent Knob2_C_Event;
    [FoldoutGroup("Knobs")]public UnityEvent Knob3_L_Event;
    [FoldoutGroup("Knobs")]public UnityEvent Knob3_R_Event;
    [FoldoutGroup("Knobs")]public UnityEvent Knob3_C_Event;
    
    // Todas las teclas con Left Ctrl
    //  7   8   9   P
    //  4   5   6   S
    //  1   2   3   Alt/Tab (no ctrl)
    
    // Knobs
    //  -       .       +
    //  *       play    /
    //  NA      NA      NA
    
    private void Update()
    {
        if (!sendEvents)
            return;
        
        var leftControl = Input.GetKeyDown(KeyCode.LeftControl);

        if (leftControl) // Boy keyboard
        {
            #region Keys

            if (Input.GetKeyDown(KeyCode.Keypad7))
            {
                Key1_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K1");
            }
            if (Input.GetKeyDown(KeyCode.Keypad8))
            {
                Key2_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K2");
            }
            if (Input.GetKeyDown(KeyCode.Keypad9))
            {
                Key3_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K3");
            }
            if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                Key5_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K5");
            }
            if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                Key6_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K6");
            }
            if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                Key7_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K7");
            }
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                Key9_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K9");
            }
            if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Key10_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K10");
            }
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Key11_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K11");
            }
            // if (Input.GetKeyDown(KeyCode.P))
            //     Key4_Event.Invoke();
            // if (Input.GetKeyDown(KeyCode.))
            //     Key8_Event.Invoke();
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                Key12_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard K12");
            }

            #endregion

            #region Knobs
            if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                Knob1_L_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard Knob1_L");
            }
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                Knob1_R_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard Knob1_R");
            }
            if (Input.GetKeyDown(KeyCode.KeypadPeriod))
            {
                Knob1_C_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard Knob1_C");
            }
            if (Input.GetKeyDown(KeyCode.KeypadDivide))
            {
                Knob2_L_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard Knob2_L");
            }
            if (Input.GetKeyDown(KeyCode.KeypadMultiply))
            {
                Knob2_R_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard Knob2_R");
            }
            // if (Input.GetKeyDown(KeyCode.))
            // {
            //     Knob2_C_Event.Invoke();
            //     if(debugLog)
            //         Debug.Log("Boy Keyboard Knob2_C");
            // }
            if (Input.GetKeyDown(KeyCode.Home))
            {
                Knob3_L_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard Knob3_L");
            }
            if (Input.GetKeyDown(KeyCode.End))
            {
                Knob3_R_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard Knob3_R");
            }
            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                Knob3_C_Event.Invoke();
                if(debugLog)
                    Debug.Log("Boy Keyboard Knob3_C");
            }
            #endregion
            
        }
    }
}