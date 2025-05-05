using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class BoyLoginPassword : MonoBehaviour
{
    // Hola Boy

    // [TabGroup("Login")] public string user;
    // [TabGroup("Login")] public string password; 
    [TabGroup("Login")] public DataLogin dataLogin;
    [TabGroup("Login")] public bool accepted;
    [TabGroup("Login")] public bool useHardcodeCredencials;
    [TabGroup("Login")] public bool useOnlineLogin;
    [TabGroup("Login")] public string addressLogin = $"https://virtualwavecontrol.com.mx/Core24/crud/Login";
    [TabGroup("Login")] public FCredentials Credencials;
    [TabGroup("Login")] public FLogin Login;
    [TabGroup("Login")] public PlayMakerFSM LoginFSM;

    [TabGroup("GeneraPassword")] public int offset;
    [TabGroup("GeneraPassword")] public List<int> primeNumbers = new List<int>();
    [TabGroup("GeneraPassword")] public int _char = 0;
    [TabGroup("GeneraPassword")] public int _char1 = 0;
    [TabGroup("GeneraPassword")] public int _char2 = 1;
    
    private void OnValidate()
    {
        if (primeNumbers.Count <= 0)
        {
            primeNumbers.Add(2);

            for (int i = 3; i <= 100; i++)
            {
                bool addNumber = true;
                for (int j = 0; j < primeNumbers.Count; j++)
                {
                    if (i % primeNumbers[j] == 0) addNumber = false;
                }
                if(addNumber) primeNumbers.Add(i);
            }
        }
    }

    [Button] [TabGroup("GeneraPassword")] 
    public void GeneratePassword()
    {
        dataLogin.password = "";
        int cont = 0;
        int lastChar = 0;
        _char = 0;
        _char1 = 0;
        _char2 = 1;
        
        foreach (var _c in dataLogin.user)
        {
            _char1 += _c;
            _char2 *= _c;
        }

        _char = (int)_char2 / _char1;
        
        for (int i = 0; i < 10; i++)
        {
            
            int newChar = (_char + offset + lastChar * primeNumbers[cont]);
            
            while (newChar < 48 || newChar > 122)
            {
                if (newChar > 122) newChar -= (122 - 48);
                if (newChar < 48) newChar += (122 - 48);
            }

            lastChar = newChar;
            dataLogin.password += (char)newChar;
            cont++;
        }
    }
    
    [Button] [TabGroup("Login")]
    public bool ValidateUser(string _user, string _password)
    {
        // ASCII 48 = '0'
        // ASCCI 122 = 'z'

        return _user == dataLogin.user && _password == dataLogin.password;
    }
    
    public void ValidateUserAsynchronous(string _user, string _password)
    {
        StartCoroutine(checkLogin(_user, _password));
    }

    public IEnumerator checkLogin(string _user, string _password)
    {
        accepted = false;

        if (useHardcodeCredencials)
        {
            accepted = _user == dataLogin.user && _password == dataLogin.password;
        }
        else if (useOnlineLogin)
        {
            UnityWebRequest unityWebRequest = null;

            Credencials.usuario = _user;
            Credencials.contrasena = _password;
           // dataLogin.idProyect = (int)RequestAPI.Instance.sistema;
           
           if (RequestAPI.Instance.sistema != RequestAPI.Proyectos.PozosPAI)
               dataLogin.idProyect = (int)RequestAPI.Instance.sistema;
           else
               dataLogin.idProyect = (int)RequestAPI.Proyectos.Teoloyucan;
           
            Credencials.idProyecto = dataLogin.idProyect;

            unityWebRequest = UnityWebRequest.Post(addressLogin, JsonUtility.ToJson(Credencials), "application/json");

            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || 
                unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(unityWebRequest.error);
            else if (unityWebRequest.isDone)
            {
                Login = JsonUtility.FromJson<FLogin>(unityWebRequest.downloadHandler.text);
                accepted = Login.response;
                
                if (!accepted)
                    accepted = _user == dataLogin.user && _password == dataLogin.password;
            }

            //yield return new WaitForSeconds(5);
        }

        if (LoginFSM != null)
        {
            if (accepted)
                LoginFSM.SendEvent("correct");
            else
                LoginFSM.SendEvent("wrong");
        }
        
        yield return null;
    }
    
    public void SendEventFSM(string eventName)
    {
        if (LoginFSM != null)
            LoginFSM.SendEvent(eventName);
    }
}
