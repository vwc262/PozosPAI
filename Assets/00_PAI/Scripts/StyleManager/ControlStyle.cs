using System;
using System.Collections.Generic;
using NUnit.Framework;
using Raskulls.ScriptableSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class ControlStyle : MonoBehaviour
{
    public SV_Style sv_Style;
    
    [TabGroup("UI")] public confStyle.StyleType styleType;
    [TabGroup("UI")] public List<TMPro.TMP_Text> textos;
    [TabGroup("UI")] public List<Text> textosLegacy;

    private void Start()
    {
        SetFontText();
    }

    public void SetFontText()
    {
        confStyle style = sv_Style.GetStyle(styleType);
        
        if (style != null)
        {
            foreach (var text in textos)
            {
                text.font = style.TMP_Font;
                text.fontSize = style.fontSize;
                text.fontSizeMax = style.fontSize;
                text.color = style.color;
            }

            foreach (var textLegacy in textosLegacy)
            {
                textLegacy.font = style.Legacy_Font;
                textLegacy.fontSize = style.fontSize;
                textLegacy.resizeTextMaxSize = style.fontSize;
                textLegacy.color = style.color;
            }
            
        }
    }
}
