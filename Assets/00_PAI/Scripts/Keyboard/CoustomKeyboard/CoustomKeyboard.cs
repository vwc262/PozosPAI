using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoustomKeyboard : MonoBehaviour
{
    public bool enableBoyKeyboard;
    
    public virtual void SetEnable(bool val)
    {
        enableBoyKeyboard = val;
    }
}
