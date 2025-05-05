using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using UnityEngine;

public class VWC_ControlCameraParticular : MonoBehaviour
{
    public InteractionOverUI_List PanelUI_Particular;

    public Vector3 rotation;
    public Vector3 rotationMax;
    public Vector3 rotationMin;
    public Vector2 moveMultiplayer;

    public bool clampX;
    public bool clampY;
    public bool clampZ;
    
    // public bool isTouchOneFinger;

    public void SetTouchInputDrag(Vector2 _input)
    {
        if (LeanTouch.Fingers.Count > 1)
            return;

        if (PanelUI_Particular.GetIsInteractionOverUI())
            MoveCamera(_input * moveMultiplayer);
    }

    public void MoveCamera(Vector2 _input)
    {
        rotation = transform.localEulerAngles + new Vector3(_input.y, _input.x, 0);

        if (rotation.x > 180) rotation.x -= 360;
        if (rotation.y > 180) rotation.y -= 360;
        if (rotation.z > 180) rotation.z -= 360;

        if (clampX)
        {
            if (rotation.x > rotationMax.x) rotation.x = rotationMax.x;
            if (rotation.x < rotationMin.x) rotation.x = rotationMin.x;
        } 
        
        if (clampY)
        {
            if (rotation.y > rotationMax.y) rotation.y = rotationMax.y;
            if (rotation.y < rotationMin.y) rotation.y = rotationMin.y;
        }
        
        if (clampZ)
        {
            if (rotation.z > rotationMax.z) rotation.z = rotationMax.z;
            if (rotation.z < rotationMin.z) rotation.z = rotationMin.z;
        }
        
        transform.localRotation = Quaternion.Euler(rotation);
        
        //transform.Rotate(new Vector3(_input.y, _input.x, 0));
    }
    
    // public void SetTouch(List<LeanFinger> listFingers)
    // {
    //     if (listFingers.Count == 1)
    //     {
    //         isTouchOneFinger = true;
    //     }
    //     else
    //     {
    //         isTouchOneFinger = false;
    //     }
    // }
}
