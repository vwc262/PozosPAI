using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_SreenShoot_Configuration", menuName = "Mike/SO_SreenShoot_Configuration")]
public class SO_SreenShoot_Configuration : ScriptableObject
{
    public enum TipoColor
    {
        Gris,
        Verde,
        Rojo,
        Azul
    }
    
    public Color[] colors;

    public List<DataPozoScreenShoot> DataPozos;
    
    public void TakeScreenshot( string fileName)
    {
        ScreenCapture.CaptureScreenshot(fileName + ".png");
    }
}

[Serializable]
public class DataPozoScreenShoot
{
    public Vector3 positions;
    
    public Vector3 localEulerAngles;

    public string names;

    public DataPozoScreenShoot()
    {
        
    }
    
    public DataPozoScreenShoot(string n, Vector3 pos, Vector3 rot)
    {
        names = n;
        positions = pos;
        localEulerAngles = rot;
    }
}