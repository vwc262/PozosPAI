using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ConnectionData : ControlJSON
{
    public bool useLocalHost;
    public string local = "http://localhost",//@"http://192.168.15.111",
        external = @"http://w1.doomdns.com:11002";
    
    [Button]
    public void ReadConnectionData()
    {
        ReadJSON_DataFile();
    }
}
