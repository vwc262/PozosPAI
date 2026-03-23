using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Launcher : MonoBehaviour
{
    public string relativePathCutzamala;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchCutzamala()
    {
        LaunchAPP(relativePathCutzamala);

        AplicactionQuit();
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

    public void LaunchAPP(string relativePath)
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
    
    public static string GetParentPath(string path) => HasParentPath(path) ? path.Substring(0, path.LastIndexOf('/')) : "";
    public static bool HasParentPath(string path) => path.LastIndexOf('/') > 0;
}
