using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Raskulls.ScriptableSystem;
using Sirenix.OdinInspector;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class ControlMoveCameraMap : MonoBehaviour
{
    [TabGroup("Cameras")] public FlyCamera flyCamera;
    [TabGroup("Cameras")] public CameraZoomMapa cameraZoomMap;
    [TabGroup("Cameras")] public GameObject cameraRoot;
    [TabGroup("Cameras")] public GameObject zoomUpPivot;
    [TabGroup("Cameras")] public GameObject CameraGimbal;
    [TabGroup("Cameras")] public Vector3 OrigenPos;
    [TabGroup("Cameras")] public GameObject zoomDownPivot;
    
    [TabGroup("Zoom Tilt")] [PropertyRange(0, 1)] public float tiltValue;
    [TabGroup("Zoom Tilt")] [PropertyRange(0, 1)] [ShowInInspector] public float zoomValue => cameraZoomMap.zoomVal;
    [TabGroup("Zoom Tilt")] public float displacementSpeed = 0.02f;
    [TabGroup("Zoom Tilt")] public float displacementSpeedTouch = 0.002f;
    [TabGroup("Zoom Tilt")] public Vector3 offsetZoom;
    [TabGroup("Zoom Tilt")] public Vector3 offsetTilt;
    [TabGroup("Zoom Tilt")] public Vector3 OrigenRotCamera;
    [TabGroup("Zoom Tilt")] public Vector3 finalRotCamera;
    [TabGroup("Zoom Tilt")] private Vector3 rotationCamera;
    [TabGroup("Zoom Tilt")] public bool UseTiltMove;
    [TabGroup("Zoom Tilt")] public float zoomHome = 0.125f;
    
    [TabGroup("Touch")] public Vector3 inputTouch;
    [TabGroup("Touch")] public float minTouchSpeed = 1;
    [TabGroup("Touch")] public float maxTouchSpeed = 3;
    [TabGroup("Touch")] public float stopCoroutineMagnitude;
    [TabGroup("Touch")] public float valZoomSlectedSitio = 0.8f;
    [TabGroup("Touch")] public Vector3 SelectedSitioOffset;
    [TabGroup("Touch")] public bool isInputDrag;
    [TabGroup("Touch")] public float touchSpeed
    {
        get
        {
            _touchSpeed = Mathf.Lerp(maxTouchSpeed, minTouchSpeed, cameraZoomMap.zoomVal);
            return _touchSpeed;
        }
        set
        {
            _touchSpeed = value;
        }
    }
    
    [TabGroup("CameraInterpolated")] public bool InterpolatedCamera;
    [TabGroup("CameraInterpolated")] public bool coroutinePos;
    [TabGroup("CameraInterpolated")] public Vector3 FinalPosition;
    [TabGroup("CameraInterpolated")] public float MoveVelocity = 0.1f;
    [TabGroup("CameraInterpolated")] public float DistancePos = 100;
    
    [TabGroup("Scriptable Events")] public SE_Float SetTouchInputZoomEvent;
    [TabGroup("Scriptable Events")] public SE_Float SetTouchInputTiltEvent;
    [TabGroup("Scriptable Events")] public SE_Float SetTouchInputDragEvent;

    [TabGroup("GUI")] public GetClickOverUIElement MapTouchElement;
    [TabGroup("GUI")] public bool useElementUI;
    
    private float _touchSpeed;
    
    public CinemachineBrain cinemachineBrainMainCamera;
    public CinemachineCamera cameraHolder;
    //public CinemachineCamera cameraBase;
    
    public float AnimTime = 1f;
    public float waitAnimTime = 0.05f;

    public float ZoomCinemachineCamera;
    public float ZoomCinemachineCameraAnt;
    
    private void Start()
    {
        flyCamera.moveEvent.AddListener(() => SetTouchInputTiltEvent.Raise(tiltValue));
        
        SetTouchInputZoomEvent.Raise(tiltValue);
        SetTouchInputDragEvent.Raise(tiltValue);
        
        UpdateOrigen();
        
        if (ControlSelectedSitio._singletonExists)
        {
            ControlSelectedSitio.singleton.ChangeSitioSeleccionado.AddListener(SetSelectedSitio);
        }
    }

    [Button]
    public void UpdateOrigen()
    {
        OrigenPos = transform.localPosition;
    }
    
    void Update()
    {
        if (UseTiltMove)
        {
            if (FlyCamera.enableInputKeyboard)
            {
                if (Input.GetKey(KeyCode.Q))
                {
                    tiltValue -= displacementSpeed;
                    if (tiltValue < 0)
                        tiltValue = 0;
                    TiltMove();
                }

                if (Input.GetKey(KeyCode.E))
                {
                    tiltValue += displacementSpeed;
                    if (tiltValue > 1)
                        tiltValue = 1f;

                    TiltMove();
                }
            }
        }
        
        if (cameraZoomMap != null && FlyCamera.enableInputKeyboard)
        {
            if (Input.GetKey(KeyCode.R))
            {
                cameraZoomMap.zoomVal -= cameraZoomMap.zoomDelta;
                if (cameraZoomMap.zoomVal < 0)
                    cameraZoomMap.zoomVal = 0;
                cameraZoomMap.SetZoom(cameraZoomMap.zoomVal);
                
                tiltValue -= cameraZoomMap.zoomDelta;
                if (tiltValue < 0)
                    tiltValue = 0;
                
                TiltMove();
            }

            if (Input.GetKey(KeyCode.F))
            {
                cameraZoomMap.zoomVal += cameraZoomMap.zoomDelta;
                if (cameraZoomMap.zoomVal > 1)
                    cameraZoomMap.zoomVal = 1f;
                cameraZoomMap.SetZoom(cameraZoomMap.zoomVal);
            }
        }
        
        //---
        if (isInputDrag)
        {
            if (flyCamera != null)
                flyCamera.inputTouch = inputTouch;

            isInputDrag = false;
        }
        else if (LeanTouch.Fingers.Count == 1)
        {
            //isTouchDrag = false;
            if (flyCamera != null)
                flyCamera.inputTouch = inputTouch;
        }
        else
        {
            if (flyCamera != null)
                flyCamera.inputTouch = Vector3.zero;
        }
        
        ZoomCinemachineCamera = 1-((cinemachineBrainMainCamera.transform.position.y - cameraZoomMap.zoomDownPivot.transform.position.y)*
                                (1f/(transform.position.y - cameraZoomMap.zoomDownPivot.transform.position.y)));

        if (ZoomCinemachineCamera != ZoomCinemachineCameraAnt)
        {
            SetTouchInputZoomEvent.Raise(ZoomCinemachineCamera);
            ZoomCinemachineCameraAnt = ZoomCinemachineCamera;
        }
    }

    public void TiltMove()
    {
        // cameraRoot.transform.position = Vector3.Lerp(
        //     zoomUpPivot.transform.position,
        //     zoomDownPivot.transform.position,
        //     tiltValue);
        
        if (tiltValue < cameraZoomMap.zoomVal)
        {
            cameraZoomMap.zoomVal -= displacementSpeed;
            if (cameraZoomMap.zoomVal < 0)
                cameraZoomMap.zoomVal = 0;
            cameraZoomMap.SetZoom(cameraZoomMap.zoomVal);
        }

        if (tiltValue > cameraZoomMap.zoomVal)
        {
            cameraZoomMap.zoomVal += displacementSpeed;
            if (cameraZoomMap.zoomVal > 1)
                cameraZoomMap.zoomVal = 1f;
            cameraZoomMap.SetZoom(cameraZoomMap.zoomVal);
        }

        rotationCamera = CameraGimbal.transform.rotation.eulerAngles;

        rotationCamera.x = Mathf.Lerp(
            OrigenRotCamera.x,
            finalRotCamera.x,
            tiltValue);

        CameraGimbal.transform.rotation = Quaternion.Euler(rotationCamera);
        
        SetTouchInputTiltEvent.Raise(tiltValue);
    }

    public void ResetTilt()
    {
        tiltValue = 0;
        SetTouchInputTiltEvent.Raise(tiltValue);
    }

    public void ResetHomeXZ(Vector3 _origen)
    {
        OrigenPos.x = _origen.x;
        OrigenPos.z = _origen.z;
        MoveHome();
    }

    public void GoHome()
    {
        tiltValue = 0;
        cameraZoomMap.SetZoom(zoomHome);
        MoveHome();
    }
    
    public void MoveHome()
    {
        //flyCamera.SetPosition(OrigenPos);
        MoveCameraMapa(cinemachineBrainMainCamera.transform.position, OrigenPos);
    }
    
    public void SetTouchInputZoom(Vector2 _input)
    {
        if (LeanTouch.Fingers.Count  != 3)
            return;
        
        if (MapTouchElement != null)
            if (!MapTouchElement.IsClicOverElement)
            {
                return;
            }
        
        if (InteractionOverUI_List.GetIsInteractionOverUI_List())
        {
            return;
        }
        
        SetTouchInputTiltAction(_input);
    }

    private void SetTouchInputTiltAction(Vector2 _input)
    {
        tiltValue += displacementSpeedTouch * _input.y;
        if (tiltValue < 0)
            tiltValue = 0;

        if (tiltValue > 1)
            tiltValue = 1f;

        TiltMove();
        SetTouchInputTiltEvent.Raise(tiltValue);
    }

    public void SetTouchInputTiltFloat(float _input)
    {
        SetTouchInputTiltAction(new Vector2(0, _input));
    }
    
    public void SetTouchInputDrag(Vector2 _input)
    {
        if (LeanTouch.Fingers.Count > 1)
        {
            inputTouch.x = 0;
            inputTouch.z = 0;
            return;
        }
        
        if (MapTouchElement != null)
        {
            if (!MapTouchElement.IsClicOverElement)
            {
                inputTouch.x = 0;
                inputTouch.z = 0;
                return;
            }
        }
        
        if (InteractionOverUI_List.GetIsInteractionOverUI_List())
        {
            inputTouch.x = 0;
            inputTouch.z = 0;
            return;
        }
        
        inputTouch.x = _input.x;
        inputTouch.z = _input.y;
        
        inputTouch = inputTouch.normalized * (touchSpeed / (1 + tiltValue));
        
        SetTouchInputDragEvent.Raise(tiltValue);
    }

    public void SetTouchInputDragNoFinger(Vector2 _input, float _DragSpeed)
    {
        isInputDrag = true;
        
        inputTouch.x = _input.x;
        inputTouch.z = _input.y;

        inputTouch = inputTouch.normalized * (_DragSpeed / (1 + tiltValue));
        
        SetTouchInputDragEvent.Raise(tiltValue);
    }
    
    public void SetSelectedSitio(ControlSitio sitio)
    {
        if (sitio != null)
            SetSelectedSitioPosition(sitio.controlMarcadorMap.transform.position);
    }
    
    public void SetSelectedSitioPosition(Vector3 _position)
    {
        tiltValue = 0;
        SetTouchInputTiltEvent.Raise(tiltValue);
        
        cameraZoomMap.SetZoom(valZoomSlectedSitio);
        
        _position.y = 0;

        Vector3 oldPosition = _position;
        oldPosition.y = transform.position.y;
        
        FinalPosition = oldPosition + 
                        (offsetTilt * Mathf.Sqrt(tiltValue)) + 
                        (offsetZoom * Mathf.Sqrt(zoomValue)) + 
                        SelectedSitioOffset;
        
        if (!InterpolatedCamera)
            transform.position = FinalPosition;
        else
            MoveCameraMapa(cinemachineBrainMainCamera.transform.position, FinalPosition);
    }

    public IEnumerator MoveCameraToFinal()
    {
        coroutinePos = true;
        
        while (transform.position != FinalPosition)
        {
            transform.position = Vector3.Lerp(transform.position, FinalPosition, MoveVelocity);

            if (Vector3.Distance(transform.position, FinalPosition) < DistancePos)
                transform.position = FinalPosition;
            
            //SetTouchInputZoomEvent.Raise(zoomUpPivot.GetComponent<CameraZoomMapa>().zoomVal);
            SetTouchInputDragEvent.Raise(tiltValue);

            yield return null;
        }
        
        coroutinePos = false;
    }
    
    public void MoveCameraMapa(Vector3 origin, Vector3 destiny)
    {
        StartCoroutine(animCameraMapa(origin, destiny));
    }
    
    public IEnumerator animCameraMapa(Vector3 origin, Vector3 destiny)
    {
        cameraHolder.transform.position = origin;
        transform.position = new Vector3(
            origin.x, 
            transform.position.y, 
            origin.z);
        cinemachineBrainMainCamera.DefaultBlend.Time = 0;
        cameraHolder.Priority = 5;

        yield return new WaitForSeconds(waitAnimTime);
        
        cinemachineBrainMainCamera.DefaultBlend.Time = AnimTime;
        transform.position = destiny;
        cameraHolder.Priority = 1;
    }

    public void MoveCameraDisplacemment(Vector3 _displacement)
    {
        _displacement.y = 0;

        FinalPosition = transform.position + _displacement;

        transform.position = FinalPosition;
        
        //SetTouchInputZoomEvent.Raise(zoomUpPivot.GetComponent<CameraZoomMapa>().zoomVal);
    }

    public void SetPointZoom(float x, float z, float zoom)
    {
        FinalPosition = transform.localPosition;

        FinalPosition.x = x;
        FinalPosition.z = z;

        transform.position = FinalPosition;
        //SetSelectedSitioPosition(FinalPosition);
        
        cameraZoomMap.SetZoom(zoom);
    }
}
