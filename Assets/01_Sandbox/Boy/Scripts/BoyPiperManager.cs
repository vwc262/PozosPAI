using System;
using System.Collections;
using UnityEngine;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Sirenix.OdinInspector;
using UnityEngine.Events;
using UnityEngine.Networking;

public class BoyPiperManager : MonoBehaviour
{
    [ReadOnly]
    //public string piperExePath = @"B:\BoyDevelop\CIDETEC\ProyectoIA\Piper\piper_windows_amd64_2\piper\piper.exe";
    public string piperExePath = Application.dataPath + "/.BoyPiper/piper.exe";
    [ReadOnly]
    //public string modelPath = @"B:\BoyDevelop\CIDETEC\ProyectoIA\Piper\piper_windows_amd64_2\piper\piper-voices\es\es_MX\claude\high\es_MX-claude-high.onnx";
    public string modelPath = Application.dataPath + "/.BoyPiper/piper-voices/es/es_MX/claude/high/es_MX-claude-high.onnx";
    public string modelPath2 = Application.dataPath + "/.BoyPiper/BoyVoices/";
    public string modelName = "Laura";
    
    [ReadOnly]
    public string outputPath = Application.streamingAssetsPath;
    public string fileName = "output.wav";

    public string texto;
    
    //public AudioClip audioClip;
    public AudioSource audioSource;


    [ReadOnly]public bool speaking = false;
    [ReadOnly]public bool audioSourcePlating = false;
    public bool debug;
    
    public UnityEvent StartSpeacking = new UnityEvent();
    public UnityEvent StopSpeacking = new UnityEvent();


    [Button]
    private void FillePaths()
    {
        piperExePath = Application.dataPath + "/.BoyPiper/piper.exe";
        modelPath = Application.dataPath + "/.BoyPiper/BoyVoices/Laura";
        outputPath = Application.streamingAssetsPath;
    }

    private void Update()
    {
        audioSourcePlating = audioSource.isPlaying;
        if (speaking && !audioSource.isPlaying)
        {
            speaking = false;
            StopSpeacking.Invoke();
        }
    }


    [Button]
    public void Hablar(string texto)
    {
        //outputPath = "Assets/StreamingAssets/output.wav";
        outputPath = Application.streamingAssetsPath +"/"+ fileName;
        // Configuración del proceso para que sea invisible
        ProcessStartInfo psi = new ProcessStartInfo
        {
            FileName = piperExePath,
            //Arguments = $"--model \"{modelPath}\" --output_file \"{outputPath}\" --length_scale 0.8",
            Arguments = $"--model \"{modelPath2+modelName+".onnx"}\" --output_file \"{outputPath}\" --length_scale 0.8",
            UseShellExecute = false,
            RedirectStandardInput = true,
            CreateNoWindow = true,
            WorkingDirectory = Path.GetDirectoryName(piperExePath)
        };
        
        using (Process proceso = Process.Start(psi))
        {
            using (StreamWriter sw = proceso.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    texto = texto.Replace("ñ", "ni").Replace("Ñ", "Ni");
                    sw.WriteLine(texto);
                }
            }
            proceso.WaitForExit();
        }
        
        // Aquí llamarías a una función para cargar y reproducir el .wav
        if(debug)print("Audio generado en: " + outputPath);
        ReproducirAudio();
    }
    
    void ReproducirAudio()
    {
        StartCoroutine(CargarYReproducir(fileName));
    }
    
    
    private IEnumerator CargarYReproducir(string nombreArchivo)
    {
        // Usamos StreamingAssets para que sea multiplataforma
        string rutaFisica = Path.Combine(Application.streamingAssetsPath, nombreArchivo);
        
        if (!File.Exists(rutaFisica))
        {
            UnityEngine.Debug.LogError("El archivo no existe en: " + rutaFisica);
            yield break;
        }
        
        
        if(debug)print($"Conviertiendo a voz: {rutaFisica}");
        // Formateamos la URI correctamente según la plataforma
        System.Uri uri = new System.Uri(rutaFisica);

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(uri, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.LogError($"Error de Curl {www.responseCode}: {www.error}");
            }
            else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                if (clip != null)
                {
                    audioSource.clip = clip;
                    audioSource.Play();
                    StartSpeacking.Invoke();
                    speaking = true;
                }
            }
        }
    }
}