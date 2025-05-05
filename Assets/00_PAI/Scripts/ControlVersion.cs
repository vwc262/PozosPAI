using UnityEngine;

public class ControlVersion : MonoBehaviour
{
    public TMPro.TMP_Text TMP_Text_Version;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (TMP_Text_Version != null)
            TMP_Text_Version.text = Application.version;
    }
}
