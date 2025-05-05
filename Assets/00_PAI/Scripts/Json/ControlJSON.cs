using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

public class ControlJSON
{
    [ShowInInspector]
    private string jsonString;
    public string path;
    public string fileName = "DataFile.vwc";
    
    [Button]
    public void SaveJSON_DataFile()
    {
        jsonString = GetJsonString();
        SaveDataFile(jsonString);
    }
    
    [Button]
    public void ReadJSON_DataFile()
    {
        jsonString = ReadDataFile();
        
        if (jsonString != "")
            SetDataFromJson(jsonString);
    }
    
    public string GetJsonString()
    {
        return JsonUtility.ToJson(this);
    }
    
    public void SetDataFromJson(string json)
    {
        JsonUtility.FromJsonOverwrite(json, this);
    }
    
    public void SaveDataFile(string data)
    {
        path = Application.dataPath + "/" + fileName;
        
        File.WriteAllText(path, data);
    }
    
    public string ReadDataFile()
    {
        path = Application.dataPath + "/" + fileName;

        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }
        else
        {
            return "";
        }
    }
}
