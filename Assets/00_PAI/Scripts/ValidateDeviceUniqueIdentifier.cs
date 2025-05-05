using UnityEngine;

public class ValidateDeviceUniqueIdentifier : MonoBehaviour
{
    public string KeyName = "serialIdentifier";
    public string serialIdentifier;
    
    public TMPro.TMP_Text DeviceIdentifier;
    
    private string GetdeviceUniqueIdentifier()
    {
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
