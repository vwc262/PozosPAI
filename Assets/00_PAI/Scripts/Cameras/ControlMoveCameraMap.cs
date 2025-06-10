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
    [TabGroup("Touch")] public float valZoomSlectedSitio = 0.8f;
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
    public Coroutine coroutineMoveCamera;
    
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
                    if (tiltValue < cameraZoomMap.zoomVal)
                    {
                        SetZoom(-displacementSpeed);
                    }
                    
                    TiltMove(-displacementSpeed);
                }

                if (Input.GetKey(KeyCode.E))
                {
                    if (tiltValue > cameraZoomMap.zoomVal)
                    {
                        SetZoom(displacementSpeed);
                    }
                    
                    TiltMove(displacementSpeed);
                }
            }
        }
        
        if (cameraZoomMap != null && FlyCamera.enableInputKeyboard)
        {
            if (Input.GetKey(KeyCode.R))
            {
                if (cameraZoomMap.zoomVal < tiltValue)
                {
                    TiltMove(-cameraZoomMap.zoomDelta);
                }
                
                SetZoom(-cameraZoomMap.zoomDelta);
            }

            if (Input.GetKey(KeyCode.F))
            {
                SetZoom(cameraZoomMap.zoomDelta);
            }
        }
        
        ZoomCinemachineCamera = 1-((cinemachineBrainMainCamera.transform.position.y - cameraZoomMap.zoomDownPivot.transform.position.y)*
                                (1f/(transform.position.y - cameraZoomMap.zoomDownPivot.transform.position.y)));

        if (ZoomCinemachineCamera != ZoomCinemachineCameraAnt)
        {
            SetTouchInputZoomEvent.Raise(ZoomCinemachineCamera);
            ZoomCinemachineCameraAnt = ZoomCinemachineCamera;
        }
    }

    public void SetZoom(float incrementZoomValue)
    {
        cameraZoomMap.zoomVal += incrementZoomValue;
            
        if (cameraZoomMap.zoomVal > 1)
            cameraZoomMap.zoomVal = 1f;
        else if (cameraZoomMap.zoomVal < 0)
            cameraZoomMap.zoomVal = 0;
        
        cameraZoomMap.SetZoom(cameraZoomMap.zoomVal);
    }

    public void TiltMove(float incrementTiltValue)
    {
        tiltValue += incrementTiltValue;

        if (tiltValue < 0)
            tiltValue = 0;
        else if (tiltValue > 1)
            tiltValue = 1f;

        TiltMove();
    }
    
    public void TiltMove()
    {
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
        TiltMove();
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
        ResetTilt();
        MoveHome();
        cameraZoomMap.SetZoom(zoomHome);
    }
    
    public void MoveHome()
    {
        MoveCameraMapa(cinemachineBrainMainCamera.transform, OrigenPos);
    }
    
    public void SetTouchInputZoom(float _input)
    {
        if (cameraZoomMap != null)
        {
            if (_input < 0)
            {
                if (cameraZoomMap.zoomVal < tiltValue)
                {
                    TiltMove(_input);
                }
            }
                
            SetZoom(_input);
        }
    }
    
    public void SetTouchInputTilt(float _input)
    {
        if (_input < 0)
        {
            if (tiltValue < cameraZoomMap.zoomVal)
            {
                SetZoom(_input);
            }
                    
            TiltMove(_input);
        }
        else
        {
            if (tiltValue > cameraZoomMap.zoomVal)
            {
                SetZoom(_input);
            }
                    
            TiltMove(_input);
        }
    }
    
    public void SetTouchInputDrag(Vector2 _input)
    {
        flyCamera.inputTouch.x = -_input.x;
        flyCamera.inputTouch.z = -_input.y;
        
        SetTouchInputDragEvent.Raise(tiltValue);
    }

    public void SetTouchInputDragNoFinger(Vector2 _input, float _DragSpeed)
    {
        inputTouch.x = _input.x;
        inputTouch.z = _input.y;
    
        inputTouch = inputTouch.normalized * (_DragSpeed / (1 + tiltValue));

        SetTouchInputDrag(inputTouch);
    }
    
    public void SetSelectedSitio(ControlSitio sitio)
    {
        if (sitio != null)
            SetSelectedSitioPosition(sitio.controlMarcadorMap.GetMarcadorPosition(), sitio.controlMarcadorMap.SelectedSitioOffset);
    }
    
    public void SetSelectedSitioPosition(Vector3 _position, Vector3 SelectedSitioOffset)
    {
        ResetTilt();
        
        cameraZoomMap.SetZoom(valZoomSlectedSitio);
        
        _position.y = 0;

        Vector3 oldPosition = _position;
        oldPosition.y = transform.position.y;
        
        FinalPosition = oldPosition + SelectedSitioOffset;
        
        if (!InterpolatedCamera)
            transform.position = FinalPosition;
        else
            MoveCameraMapa(cinemachineBrainMainCamera.transform, FinalPosition);
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
    
    public void MoveCameraMapa(Transform origin, Vector3 destiny)
    {
        if (coroutineMoveCamera != null)
            StopCoroutine(coroutineMoveCamera);
        
        if (gameObject.activeInHierarchy)
            coroutineMoveCamera = StartCoroutine(animCameraMapa(origin, destiny));
    }
    
    public IEnumerator animCameraMapa(Transform origin, Vector3 destiny)
    {
        cameraHolder.transform.position = origin.position;
        cameraHolder.transform.rotation = origin.rotation;
        transform.position = new Vector3(
            origin.position.x, 
            transform.position.y, 
            origin.position.z);
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
    }

    public void MoveCameraAnimDisplacemment(Vector3 _displacement)
    {
        MoveCameraMapa(cinemachineBrainMainCamera.transform, transform.position + _displacement);
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
