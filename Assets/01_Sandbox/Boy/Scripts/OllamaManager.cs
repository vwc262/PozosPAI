using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

public class OllamaManager : MonoBehaviour
{
    private const string BaseUrl = "http://localhost:11434";

    public BoyHoViAvatarManager boyHoViAvatarManager;
    
    public bool updateOllamaOnStart = true;
    public bool deleteHoviFile = true;
    [ReadOnly]public bool ollamaUpdated = true;
    [ReadOnly]public bool thinking = false;
    //public string batPath = Application.dataPath + "/.BoyOllama/OllamaGeneration.bat";
    public string ModelFileName = "Ollama_ModelFile.intent";
    
    private string cmdInstructions = "ollama create ";
    
    public string logMessage;
    [Multiline(7)]
    public string resultMessage;
    
    public bool debug;
    public UnityEvent<string> Thinking = new UnityEvent<string>( );
    public UnityEvent<string> ResultReceived = new UnityEvent<string>( );

    private string modelText = "";
    

    private void Start()
    {
        if (updateOllamaOnStart)
        {
            PrepareModel();
        }
    }
    
    [Button]
    public void PrepareModel()
    {
        CreateModelText();
        ollamaUpdated = false;
            
        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = "cmd.exe";
        //psi.Arguments = $"/c \"{cmdInstructions}\"";   // /c ejecuta y cierra
        //psi.Arguments = $"/c \"{cmdInstructions+TrainedModelFileName+" -f "+ModelFileName}\"";   // /c ejecuta y cierra
        psi.Arguments = $"/c \"{cmdInstructions+boyHoViAvatarManager.HoViData.model+" -f Ollama_Model.HoVi"}\"";   // /c ejecuta y cierra
        psi.WorkingDirectory = Application.dataPath + "/.BoyOllama";
        psi.CreateNoWindow = true;
        psi.UseShellExecute = false;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;
        psi.RedirectStandardInput = true;

        Process process = new Process();
        process.StartInfo = psi;

        process.OutputDataReceived += (s, e) => { if (e.Data != null && debug) UnityEngine.Debug.Log("<color=yellow>"+e.Data+"</color>"); };
        process.ErrorDataReceived += (s, e) => { if (e.Data != null  && debug) UnityEngine.Debug.Log("<color=orange>"+e.Data+"</color>"); };

        process.Start();
        
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        ollamaUpdated = true;
        
        if(deleteHoviFile)
            System.IO.File.Delete(Application.dataPath + "/.BoyOllama/Ollama_Model.HoVi");
    }

    private void CreateModelText()
    {
        modelText = "";
        string[] lines1 = System.IO.File.ReadAllLines(Application.dataPath + "/.BoyOllama/Ollama_ModelFileBase.HoVi");
        foreach (var line in lines1)
            modelText += line+"\n";
        modelText += "SYSTEM \"\"\"\n";
        string[] lines2 = System.IO.File.ReadAllLines(Application.dataPath + "/.BoyOllama/"+boyHoViAvatarManager.HoViData.HoViCatConfigFileName);
        foreach (var line in lines2)
            modelText += line+"\n";
        string[] lines3 = System.IO.File.ReadAllLines(Application.dataPath + "/.BoyOllama/"+boyHoViAvatarManager.HoViData.HoViModelFileName);
        foreach (var line in lines3)
            modelText += line+"\n";

        if(!File.Exists(Application.dataPath + "/.BoyOllama/Ollama_Model_Memory.HoVi"))
            System.IO.File.WriteAllText(Application.dataPath + "/.BoyOllama/Ollama_Model_Memory.HoVi", "");
        modelText += "Cosas que debes recordar:\nLa aplicacion fue creada por Boy en Febrero de 2026\n";
        modelText += $"El dia de hoy es {DateTime.Today.ToString("dd/MM/yyyy")}\n";
        string[] lines4 =
            System.IO.File.ReadAllLines(Application.dataPath + "/.BoyOllama/Ollama_Model_Memory.HoVi");
        foreach (var line in lines4)
            modelText += line + "\n";
        
        modelText += "\"\"\"";
        
        System.IO.File.WriteAllText(Application.dataPath + "/.BoyOllama/Ollama_Model.HoVi", modelText);
    }

    [System.Serializable] 
    class ChatRequest
    {
        public string model;
        public Message[] messages;
        public bool stream = false;
    }

    [System.Serializable] 
    class Message
    {
        public string role;
        public string content;
        public Message(string r, string c){ role = r; content = c; }
    }

    [System.Serializable] 
    class ChatResponse
    {
        public Message message;
        public bool done;
    }

    [Button]
    public void Ask(string prompt)
    {
        resultMessage = "";
        thinking = true;
        StartCoroutine(ChatCoroutine(prompt));
    }

    IEnumerator ChatCoroutine(string prompt)
    {
        var reqObj = new ChatRequest
        {
            model = boyHoViAvatarManager.HoViData.model,
            messages = new[] { new Message("user", prompt) },
            stream = false
        };

        string json = JsonUtility.ToJson(reqObj);

        UnityWebRequest req = new UnityWebRequest($"{BaseUrl}/api/chat", "POST");
        
        req.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");

        logMessage = prompt;
        if(debug)
            Debug.Log("Ollama Pregunta:\n" + "<color=green>" + prompt + "</color>");
        Thinking?.Invoke("...");
        yield return req.SendWebRequest();

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ollama error: " + req.error);
            yield break;
        }

        var response = JsonUtility.FromJson<ChatResponse>(req.downloadHandler.text);
        resultMessage = response.message.content;
        ResultReceived?.Invoke(resultMessage);
        thinking = false;
        if(debug)
            Debug.Log("Ollama Respuesta:\n" + "<color=orange>" + response.message.content + "</color>");
        
    }
}