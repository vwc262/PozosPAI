using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlyCamera : Singleton<FlyCamera>
{
    public bool enableInputKeyboard;
    
    public float mainSpeed = 1.0f; //regular speed
    public float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    public float maxShift = 1000.0f; //Maximum speed when holdin gshift
    public float camSens = 0.25f; //How sensitive it with mouse
    public bool invertY = true;

    public float scrollWheelSens = 1f;

    private Vector3
        lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)

    private float totalRun = 1.0f;
    
    public bool UseMouse;
    public bool UseZoom;

    public Vector3 inputTouch;

    public UnityEvent moveEvent;

    public Vector3 positionAux;
    public Vector3 positionMax;
    public Vector3 positionMin;

    public bool SetPositionAux;
    public bool ClampX;
    public bool ClampZ;
    
    void Update()
    {
        if (SetPositionAux)
        {
            transform.position = positionAux;
            SetPositionAux = false;
            return;
        }
        
        if (UseMouse)
        {
            if (Input.GetMouseButton(1))
            {
                var mouseMoveY = invertY ? -1 * Input.GetAxis("Mouse Y") : Input.GetAxis("Mouse Y");
                var mouseMoveX = Input.GetAxis("Mouse X");

                var mouseMove = new Vector3(mouseMoveY, mouseMoveX, 0) * camSens;
                transform.eulerAngles = transform.eulerAngles + mouseMove;
            }

            if (Input.GetMouseButtonDown(1))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

            if (Input.GetMouseButtonUp(1))
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        //Mouse  camera angle done.  

        //Keyboard commands
        float f = 0.0f;
        
        Vector3 p = GetBaseInput() + inputTouch;
        
        if (p.sqrMagnitude > 0)
        {
            // only move while a direction key is pressed
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            if (Input.GetKey(KeyCode.Space))
            {
                //If player wants to move on X and Z axis only
                transform.Translate(p);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else
            {
                transform.Translate(p);
            }
        }

        var scroll = Input.GetAxis("Mouse ScrollWheel");
        mainSpeed += scroll * scrollWheelSens;

        transform.position = ClampMove(transform.position);
    }

    public void SetPosition(Vector3 position)
    {
        SetPositionAux = true;
        positionAux = position;
    }

    public Vector3 ClampMove(Vector3 positionAux)
    {
        if (ClampX)
        {
            if (positionAux.x > positionMax.x)
                positionAux.x = positionMax.x;
            
            if (positionAux.x < positionMin.x)
                positionAux.x = positionMin.x;
        }
        
        if (ClampZ)
        {
            if (positionAux.z > positionMax.z)
                positionAux.z = positionMax.z;

            if (positionAux.z < positionMin.z)
                positionAux.z = positionMin.z;
        }

        return positionAux;
    }

    private Vector3 GetBaseInput()
    {
        if (!enableInputKeyboard)
            return Vector3.zero;
        
        //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        
        if (UseZoom)
        {
            if (Input.GetKey(KeyCode.Q))
            {
                p_Velocity += new Vector3(0, -1, 0);
            }

            if (Input.GetKey(KeyCode.E))
            {
                p_Velocity += new Vector3(0, 1, 0);
            }
        }

        if(p_Velocity.magnitude > 0)
            moveEvent.Invoke();

        return p_Velocity;
    }
}