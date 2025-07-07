using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
    
    // Importar funciones de user32.dll
    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    
    [DllImport("user32.dll")]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, IntPtr processId);
    
    [DllImport("user32.dll")]
    private static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, bool fAttach);
    
    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);
    
    private const int SW_RESTORE = 9; // Para restaurar la ventana si está minimizada
    private IntPtr unityWindowHandle;
    
    public float updateRate = 5;
    private float countdown;
    
    void Start()
    {
        Application.targetFrameRate = targetFrameRate;
        
        // Obtener el handle de la ventana de Unity
        unityWindowHandle = GetActiveWindow();
        Application.runInBackground = true; // Opcional: evitar pausa al perder foco
        
#if !UNITY_EDITOR
        if (validaAplicationInFocus)
        {
            string file = GetParentPath(Application.dataPath) + "/ControlAplication/ControlAplication.exe";
            
            Debug.Log(file);
            
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = file,
                CreateNoWindow = true,
                UseShellExecute = false
            };
            
            Process.Start(startInfo);
        }
#endif
    }
    
    public static string GetParentPath(string path) => HasParentPath(path) ? path.Substring(0, path.LastIndexOf('/')) : "";
    public static bool HasParentPath(string path) => path.LastIndexOf('/') > 0;
    
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

    [Button]
    public void RestartPC()
    {
        System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
    }

    private void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0)
        {
            Updatefocus();
            countdown = updateRate;
        }
    }
    
    public void Updatefocus()
    {
#if !UNITY_EDITOR
        if (validaAplicationInFocus)
        {
            // Verificar si la ventana de Unity ya no está activa
            if (GetActiveWindow() != unityWindowHandle)
            {
                Debug.Log("La ventana perdió el foco. Recuperando...");
                ForceFocusKeyboardSim();
            }
        }
#endif
    }
    
    private void RestoreWindowFocus()
    {
        // Restaurar si está minimizada y traer al frente
        ShowWindow(unityWindowHandle, SW_RESTORE);
        SetForegroundWindow(unityWindowHandle);
    }
    
    private void ForceFocus()
    {
        // Obtener el ID del hilo de la ventana activa actual
        uint currentThreadId = GetWindowThreadProcessId(GetActiveWindow(), IntPtr.Zero);
        uint unityThreadId = GetWindowThreadProcessId(unityWindowHandle, IntPtr.Zero);

        // "Enganchar" el input de Unity al hilo actual
        AttachThreadInput(currentThreadId, unityThreadId, true);

        // Restaurar y forzar el foco
        ShowWindow(unityWindowHandle, SW_RESTORE);
        SetForegroundWindow(unityWindowHandle);

        // Desenganchar el input
        AttachThreadInput(currentThreadId, unityThreadId, false);
    }
    
    private void ForceFocusKeyboardSim()
    {
        // Simular una tecla Alt (para "desbloquear" SetForegroundWindow)
        keybd_event(0x12, 0, 0, IntPtr.Zero); // 0x12 = Tecla Alt
        keybd_event(0x12, 0, 2, IntPtr.Zero); // Liberar Alt

        // Ahora llamar a SetForegroundWindow
        ShowWindow(unityWindowHandle, SW_RESTORE);
        SetForegroundWindow(unityWindowHandle);
    }
    
    private void UpdateFocus()
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
}
