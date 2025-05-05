using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "SV_Style_Name", menuName = "Raskulls/Scriptable System/Variables/SV_Style")]
    public class SV_Style : ScriptableVariableBase
    {
        public List<confStyle> styles;
        
        public confStyle GetStyle(confStyle.StyleType styleType)
        {
            return styles.Find(item => item.styleType == styleType);
        }
    }
    
    [Serializable]
    public class confStyle
    {
        public StyleType styleType;
        public TMP_FontAsset TMP_Font;
        public Font Legacy_Font;
        public int fontSize;
        public Color color;
    
        public enum StyleType
        {
            titulo,
            subtitulo,
            normal,
            encabezados,
            etiqueta_nombre,
            etiqueta_fecha,
            etiqueta_ID
        }
    }
}


