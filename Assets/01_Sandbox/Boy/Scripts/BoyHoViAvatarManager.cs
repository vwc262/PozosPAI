using System;
using UnityEngine;

public class BoyHoViAvatarManager : MonoBehaviour
{
    public SO_HoVi_Data HoViData;
    
    public GameObject avatarGameObject;
    
    
    private void Start()
    {
        if(avatarGameObject == null) SetupAvatar();
    }

    public void SetupAvatar()
    {
        if(avatarGameObject != null) DestroyImmediate(avatarGameObject);
        
        if(HoViData.prefab == null)
            return;
        
        avatarGameObject = Instantiate(HoViData.prefab);
        
        avatarGameObject.transform.position = HoViData.startPosition;
        avatarGameObject.transform.localEulerAngles = HoViData.startRotation;
        
    }
}
