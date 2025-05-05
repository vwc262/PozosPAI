using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyboardConfiuguration", menuName = "Scriptables/KeyboardConfiuguration", order = 0)]
public class KeyboardConfiguration : ScriptableObject
{
    public List<KeyboardAsign> keyAsignation;

    public KeyboardAsign GetKeyboardAsignByName(string _function)
    {
        return keyAsignation.Find(item => item.functionName == _function);
    }

    public bool getMods(List<KeyCode> _modKeyCodes)
    {
        bool mods = true;

        foreach (var key in _modKeyCodes)
        {
            if (!Input.GetKey(key)) mods = false;
        }

        return mods;
    }

    public bool GetKeyDownMod(string _function)
    {
        KeyboardAsign keyboardAsign = GetKeyboardAsignByName(_function);

        if (keyboardAsign == null)
            return false;

        return Input.GetKeyDown(keyboardAsign.keyCode) && getMods(keyboardAsign.modKeyCodes);
    }
}

[Serializable]
public class KeyboardAsign
{
    public string functionName;
    public KeyCode keyCode;
    public List<KeyCode> modKeyCodes;
}
