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

    public GameObject zoomDownPivot;
    public Vector2 screenPos;
    public Vector3 relativePoint;

    public float DesplacementZoomIn = 80000;
    public float DesplacementZoomOut = 34000;

    public VWC_MoveCamera moveCamera;

    public bool ZoomToPosition = true;
    
    public double ZoomDeatZone = 0.03f;
    
    public float zoomDelta;
    
    public float zoomValMultiply = 0.5f;

    private void Update()
    {
        if (FlyCamera._singletonExists && FlyCamera.singleton.enableInputKeyboard)
        {
            if (Input.GetKey(KeyCode.R))
            {
                zoomVal -= zoomDelta;
                if (zoomVal < 0)
                    zoomVal = 0;
                SetZoom(zoomVal);
            }

            if (Input.GetKey(KeyCode.F))
            {
                zoomVal += zoomDelta;
                if (zoomVal > 1)
                    zoomVal = 1f;
                SetZoom(zoomVal);
            }
        }
    }

    [Button]
    public void SetZoom(float val)
    {
        zoomVal = val;
        
        var newPos = transform.localPosition;
        newPos.y = zoomDownPivot.transform.localPosition.y * zoomVal;
        transform.localPosition = newPos;

        if (ZoomToPosition)
        {
            GetScreenPosition();
            MoveCameraPosition();
        }
    }

    public void ResetZoom()
    {
        SetZoom(resetZoomVal);
    }
    
    public void AddToZoomInverted(float val)
    {
        if(LeanTouch.Fingers.Count !=2)
            return;
        
        if (InteractionOverUI_List.GetIsInteractionOverUI_List())
        {
            return;
        }
        
        if (moveCamera.MapTouchElement != null)
            if (!moveCamera.MapTouchElement.IsClicOverElement)
            {
                return;
            }
        
        //Debug.Log("zoomval: " + (val-1));
        if (Mathf.Abs(val-1) > ZoomDeatZone)
            AddToZoom((val-1) * zoomValMultiply);
    }
    

    public void AddToZoom(float val)
    {
        zoomIncrement = val;
        zoomVal = Mathf.Clamp01(zoomVal + val);
        SetZoom(zoomVal);
    }

    public void GetScreenPosition()
    {
        if (LeanTouch.Fingers.Count == 2)
        {
            //normalizado -1 a 1
            if (moveCamera.useElementUI)
            {
                screenPos = moveCamera.MapTouchElement.GetPoitInUIElement(
                    LeanGesture.GetScreenCenter(LeanTouch.Fingers));
                screenPos.x = (screenPos.x) * 2 - 1;
                screenPos.y = (screenPos.y) * 2 - 1;
            }
            else
            {
                screenPos = LeanGesture.GetScreenCenter(LeanTouch.Fingers);
                screenPos.x = (screenPos.x / Screen.width) * 2 - 1;
                screenPos.y = (screenPos.y / Screen.height) * 2 - 1;
            }
        }
    }

    public virtual void MoveCameraPosition()
    {
        if (moveCamera != null)
        {
            if (zoomIncrement > 0)
            {
                moveCamera.MoveCameraDisplacemment(new Vector3(screenPos.x, 0, screenPos.y) *
                                                       DesplacementZoomIn * (1 - zoomVal) * zoomIncrement);
            }
            else
            {
                if (zoomVal > 0.1f)
                {
                    Vector3 mov = moveCamera.transform.InverseTransformPoint(moveCamera.OrigenPos).normalized;

                    relativePoint.x = mov.x;
                    relativePoint.z = mov.z;
                    
                    moveCamera.MoveCameraDisplacemment(relativePoint *
                                                       DesplacementZoomOut * (1f - zoomVal) * 
                                                       Mathf.Abs(zoomIncrement));
                }
                else
                {
                    //moveCamera.MoveCameraDisplacemment(Vector3.zero);
                    moveCamera.MoveCameraDisplacemment(moveCamera.OrigenPos - moveCamera.transform.position);
                }
            }
        }
    }
}
