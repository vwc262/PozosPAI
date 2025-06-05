using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Raskulls.ScriptableSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraZoomMapa : MonoBehaviour
{
    // Hola Boy
    public float zoomVal;
    public float resetZoomVal;
    public float zoomIncrement;

    public float DesplacementZoomIn = 80000;
    public float DesplacementZoomOut = 34000;
    
    public double ZoomDeatZone = 0.03f;
    public float zoomDelta;
    public float zoomValMultiply = 0.5f;
    
    public Vector2 screenPos;
    public Vector3 relativePoint;
    
    public ControlMoveCameraMap moveCamera;
    public GameObject zoomDownPivot;
    
    [Button]
    public void SetZoom(float val)
    {
        zoomVal = val;
        
        var newPos = transform.localPosition;
        newPos.y = zoomDownPivot.transform.localPosition.y * zoomVal;
        transform.localPosition = newPos;
    }

    public void ResetZoom()
    {
        SetZoom(resetZoomVal);
    }
    
    // public void AddToZoomInverted(float val)
    // {
    //     if(LeanTouch.Fingers.Count !=2)
    //         return;
    //     
    //     if (InteractionOverUI_List.GetIsInteractionOverUI_List())
    //     {
    //         return;
    //     }
    //     
    //     if (moveCamera.MapTouchElement != null)
    //         if (!moveCamera.MapTouchElement.IsClicOverElement)
    //         {
    //             return;
    //         }
    //     
    //     //Debug.Log("zoomval: " + (val-1));
    //     if (Mathf.Abs(val-1) > ZoomDeatZone)
    //         AddToZoom((val-1) * zoomValMultiply);
    // }
    
    public void AddToZoom(float val)
    {
        zoomIncrement = val;
        SetZoom(Mathf.Clamp01(zoomVal + val));
    }
}
