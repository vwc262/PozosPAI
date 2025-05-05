using Lean.Touch;
//using PipelineToolkit;
using UnityEngine;

public class DroneManager : Singleton<DroneManager>
{
    private CharacterController _controller;
    //public LeanMultiUpdate leanMultiUpdate;
    //public LeanMultiPinch leanMultiPinch;
    public Vector3 velocity;
    public Vector3 targetLocalEulerAngles;
    public float rotationFactor;
    public float rotationCCWSpeed;
    public float rotSpeed;
    public float maxAngle;
    public GameObject drone;
    
    public float droneSpeedIncrement;
    public Vector3 droneMaxSpeed;
    public float droneDrag;
    public float insideVectorFactor;
    public KeyCode key_forward;
    public KeyCode key_backward;
    public KeyCode key_left;
    public KeyCode key_right;
    public KeyCode key_up;
    public KeyCode key_down;
    public KeyCode key_rot_clockwise;
    public KeyCode key_rot_counterclockwise;
    
    
    public Vector3 insidePos;
    public Vector3 insidePos_1;
    public bool insideTrigger;

    public Vector2 deltaTouch;
    public Vector3 touchScaleFactor;
    public Vector3 touchScaleFactorJoystick;
    public float pinchTouch;
    
    public float distance, distanceAnt;
    public float distanceX, distanceXAnt;
    
    public bool isInitTouch1Fingers = false;
    public bool isInitTouch2Fingers = false;
    public float magnitudeMax;
    public Vector2 screenResolutionFactor;
    public Vector2 screenDespl;
    public Vector2 stickPosition;
    public GameObject Joystick;
    public GameObject Joystick_stick;
    public RectTransform joystick_Rect;
    public RectTransform joystick_stick_Rect;
    
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        
        //leanMultiUpdate.OnDelta.AddListener((val) => deltaTouch = val);
        //leanMultiPinch.OnPinch.AddListener((val) => pinchTouch = val);
        
        screenResolutionFactor = new Vector2(1920 / (float)Screen.width, 1080 / (float)Screen.height);
        
        if (Joystick != null)
            joystick_Rect = Joystick.GetComponent<RectTransform>();
        
        if (Joystick_stick != null)
            joystick_stick_Rect = Joystick_stick.GetComponent<RectTransform>();
    }

    
    void Update()
    {
        if(insideTrigger)
            DroneInputHandle();
        else
            InsideTriggerHandle();
        UpdateDroneRotation();
    }

    private void InsideTriggerHandle()
    {
        _controller.Move(insideVectorFactor*( insidePos_1-insidePos));
        velocity = Vector3.Lerp(velocity,Vector3.zero, Time.deltaTime * droneDrag);
        //transform.position = Vector3.Lerp(transform.position ,insidePos +insideVectorFactor*( insidePos_1-insidePos), Time.deltaTime * droneSpeedIncrement);
    }

    private void DroneInputHandle()
    {
        DroneKeyInputHandle();
        DroneTouchInputHandleJoystick();
    }

    private void DroneTouchInputHandle()
    {
        // if(LeanTouch.Fingers.Count <= 0)
        //     return;
        
        if (LeanTouch.Fingers.Count == 1)
        {
            velocity.x = touchScaleFactor.x * deltaTouch.x;
            velocity.z = touchScaleFactor.z * deltaTouch.y;
        }
        
        if (LeanTouch.Fingers.Count == 2)
            velocity.y = touchScaleFactor.y * (pinchTouch - 1);
    }
    
    private void DroneTouchInputHandleJoystick()
    {
        if (LeanTouch.Fingers.Count == 1)
        {
            if (isInitTouch1Fingers)
            {
                velocity.x = touchScaleFactorJoystick.x *
                             (LeanTouch.Fingers[0].ScreenPosition.x - LeanTouch.Fingers[0].StartScreenPosition.x);
                velocity.z = touchScaleFactorJoystick.z *
                             (LeanTouch.Fingers[0].ScreenPosition.y - LeanTouch.Fingers[0].StartScreenPosition.y);

                if (joystick_Rect != null && joystick_stick_Rect != null)
                {
                    stickPosition = (LeanTouch.Fingers[0].ScreenPosition * screenResolutionFactor + screenDespl) - joystick_Rect.anchoredPosition;
                    
                    if (stickPosition.magnitude > magnitudeMax)
                        stickPosition = magnitudeMax * stickPosition.normalized;
                        
                    joystick_stick_Rect.anchoredPosition = stickPosition;
                }
            }
            else
            {
                if (joystick_Rect != null)
                {
                    joystick_Rect.gameObject.SetActive(true);
                    joystick_Rect.anchoredPosition = LeanTouch.Fingers[0].ScreenPosition * screenResolutionFactor + screenDespl;
                }
                
                isInitTouch1Fingers = true;
            }
        }
        else
        {
            if (joystick_Rect != null)
            {
                joystick_Rect.gameObject.SetActive(false);
            }

            isInitTouch1Fingers = false;
        }

        if (LeanTouch.Fingers.Count == 2)
        {
            distance = (Vector2.Distance(LeanTouch.Fingers[0].ScreenPosition, LeanTouch.Fingers[1].ScreenPosition)) * screenResolutionFactor.y;
            if (isInitTouch2Fingers) velocity.y = touchScaleFactorJoystick.y * (distance - distanceAnt);
            distanceAnt = distance;
            
            //distanceX = (LeanTouch.Fingers[0].ScreenPosition.x - LeanTouch.Fingers[1].ScreenPosition.x) * screenResolutionFactor.x;
            distanceX = Vector2.SignedAngle(LeanTouch.Fingers[0].ScreenPosition - LeanTouch.Fingers[1].StartScreenPosition, 
                                            LeanTouch.Fingers[0].ScreenPosition - LeanTouch.Fingers[1].ScreenPosition);
            
            if (isInitTouch2Fingers) transform.Rotate(Vector3.up, (distanceX - distanceXAnt));
            distanceXAnt = distanceX;
            
            isInitTouch2Fingers = true;
        }
        else
        {
            isInitTouch2Fingers = false;
        }
    }
    
    private void DroneKeyInputHandle()
    {
        if(Input.GetKey(key_forward))
            velocity.z += droneSpeedIncrement;
        if(Input.GetKey(key_backward))
            velocity.z -= droneSpeedIncrement;
        if(Input.GetKey(key_left))
            velocity.x -= droneSpeedIncrement;
        if(Input.GetKey(key_right))
            velocity.x += droneSpeedIncrement;
        if(Input.GetKey(key_up))
            velocity.y += droneSpeedIncrement;
        if(Input.GetKey(key_down))
            velocity.y -= droneSpeedIncrement;
        if(Input.GetKey(key_rot_clockwise))
            transform.Rotate(Vector3.up, rotationCCWSpeed*Time.deltaTime);
        if(Input.GetKey(key_rot_counterclockwise))
            transform.Rotate(Vector3.up, -rotationCCWSpeed*Time.deltaTime);

        if(!Input.GetKey(key_forward) && !Input.GetKey(key_backward))
           velocity.z = Mathf.Lerp(velocity.z, 0, Time.deltaTime * droneDrag);
        if(!Input.GetKey(key_left) && !Input.GetKey(key_right))
           velocity.x = Mathf.Lerp(velocity.x, 0, Time.deltaTime * droneDrag);
        if(!Input.GetKey(key_up) && !Input.GetKey(key_down))
           velocity.y = Mathf.Lerp(velocity.y, 0, Time.deltaTime * droneDrag);
    }

    private void UpdateDroneRotation()
    {
        velocity.x = Mathf.Clamp(velocity.x, -droneMaxSpeed.x, droneMaxSpeed.x);
        velocity.y = Mathf.Clamp(velocity.y, -droneMaxSpeed.y, droneMaxSpeed.y);
        velocity.z = Mathf.Clamp(velocity.z, -droneMaxSpeed.z, droneMaxSpeed.z);
        
        if(_controller.enabled)
            _controller.Move(transform.TransformDirection(velocity) * Time.deltaTime);
        
        targetLocalEulerAngles.x = drone.transform.localEulerAngles.x;
        targetLocalEulerAngles.z = drone.transform.localEulerAngles.z;
        if (targetLocalEulerAngles.x > 180) targetLocalEulerAngles.x -= 360;
        if (targetLocalEulerAngles.z > 180) targetLocalEulerAngles.z -= 360;
        targetLocalEulerAngles.x = Mathf.Lerp(targetLocalEulerAngles.x, rotationFactor * velocity.z,
            rotSpeed * Time.deltaTime);
        targetLocalEulerAngles.z = Mathf.Lerp(targetLocalEulerAngles.z, -rotationFactor * velocity.x,
            rotSpeed * Time.deltaTime);
        if (targetLocalEulerAngles.x > 180) targetLocalEulerAngles.x -= 360;
        if (targetLocalEulerAngles.z > 180) targetLocalEulerAngles.z -= 360;
        targetLocalEulerAngles.x = Mathf.Clamp(targetLocalEulerAngles.x, -maxAngle, maxAngle);
        targetLocalEulerAngles.z = Mathf.Clamp(targetLocalEulerAngles.z, -maxAngle, maxAngle);
        drone.transform.localEulerAngles = targetLocalEulerAngles;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
    }
    
    private void OnTriggerStay(Collider other)
    {
        insideTrigger = true;
        insidePos_1 = insidePos;
        insidePos = transform.position;
    }
    
    private void OnTriggerExit(Collider other)
    {
        insideTrigger = false;
    }
    
    
}
