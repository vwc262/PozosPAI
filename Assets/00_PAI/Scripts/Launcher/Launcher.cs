using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System;

public class Launcher : MonoBehaviour
{
    public string variableEntornoPAI = "pathVWC_PAI";
    public string variableEntornoCutzamala = "pathVWC_Cutzamala";
    public string variableEntornoHidrometricas = "pathVWC_Hidrometricas";
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
#if !UNITY_EDITOR
        //Environment.SetEnvironmentVariable(variableEntornoPAI, GetParentPath(Application.dataPath) + "/PozosPaiNorte.exe", EnvironmentVariableTarget.User);
        Environment.SetEnvironmentVariable(variableEntornoPAI,  Process.GetCurrentProcess().MainModule.FileName, EnvironmentVariableTarget.User);
#endif
    }

    public void LaunchCutzamala()
    {
        string file = Environment.GetEnvironmentVariable(variableEntornoCutzamala, EnvironmentVariableTarget.User);
        
        if (file != null)
        {
            LaunchAPP_Path(file);
            AplicactionQuit();
        }
    }
    
    public void LaunchPAI()
    {
        string file = Environment.GetEnvironmentVariable(variableEntornoPAI, EnvironmentVariableTarget.User);
        
        if (file != null)
        {
            LaunchAPP_Path(file);
            AplicactionQuit();
        }
    }
    
    public void LaunchHidrometricas()
    {
        string file = Environment.GetEnvironmentVariable(variableEntornoHidrometricas, EnvironmentVariableTarget.User);
        
        if (file != null)
        {
            LaunchAPP_Path(file);
            AplicactionQuit();
        }
    }
    
    public void AplicactionQuit()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LaunchAPP_RelativePath(string relativePath)
    {
        string file = GetParentPath(Application.dataPath) + relativePath;
        
        Debug.Log($"OpenAPP: {file}");
        
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = file,
            CreateNoWindow = true,
            UseShellExecute = false
        };
        
        Process.Start(startInfo);
    }
    
    public void LaunchAPP_Path(string path)
    {
        Debug.Log($"OpenAPP: {path}");

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = path,
            CreateNoWindow = true,
            UseShellExecute = false
        };

        Process.Start(startInfo);
    }
    
    public static string GetParentPath(string path) => HasParentPath(path) ? path.Substring(0, path.LastIndexOf('/')) : "";
    public static bool HasParentPath(string path) => path.LastIndexOf('/') > 0;
}
