using Sirenix.OdinInspector;
using UnityEngine;

public class ValidateDeviceUniqueIdentifier : MonoBehaviour
{
    public string KeyName = "serialIdentifier";
    public string serialIdentifier;
    
    public TMPro.TMP_Text DeviceIdentifier;
    
    [Button]
    private string GetdeviceUniqueIdentifier()
    {
        serialIdentifier =  SystemInfo.deviceUniqueIdentifier;
        return SystemInfo.deviceUniqueIdentifier;
    }

    public bool ValidateDeviceIdentifier()
    {
        if (PlayerPrefs.HasKey(KeyName))
        {
            serialIdentifier = PlayerPrefs.GetString(KeyName);
            return SystemInfo.deviceUniqueIdentifier == PlayerPrefs.GetString(KeyName);
        }
        return false;
    }

    public void SetDeviceIdentifier()
    {
        PlayerPrefs.SetString(KeyName,SystemInfo.deviceUniqueIdentifier);
        
        if (DeviceIdentifier != null)
            DeviceIdentifier.text = SystemInfo.deviceUniqueIdentifier;
    }
}
