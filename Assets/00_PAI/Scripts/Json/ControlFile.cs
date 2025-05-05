using System.IO;
using UnityEngine;
using SFB;

public class ControlFile
{
    public string fileName = "DataFile.vwc";
    public string path;

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
    
    public void SaveDataFile(string filePath, string data)
    {
        File.WriteAllText(filePath, data);
    }
    
    public string GetFilePath()
    {
        string paths = StandaloneFileBrowser.SaveFilePanel("Save file", "", "HistoricData.csv", "csv");
        
        return paths;
    }
}
