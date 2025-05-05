using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoustomKeyboardParticular : CoustomKeyboard
{
    public VWC_ControlCameraParticular controlCamera;

    public Vector2 moveMultiplayer;
    
    private void Start()
    {
        if (BoyCoustomKeyboard._singletonExists)
        {
            BoyCoustomKeyboard.singleton.Knob1_L_Event.AddListener(DecrementDragX);
            BoyCoustomKeyboard.singleton.Knob1_R_Event.AddListener(IncrementDragX);
            
            BoyCoustomKeyboard.singleton.Knob2_L_Event.AddListener(DecrementDragY);
            BoyCoustomKeyboard.singleton.Knob2_R_Event.AddListener(IncrementDragY);
        }
    }

    public void DecrementDragX()
    {
        if(enableBoyKeyboard)
            controlCamera.MoveCamera(new Vector2(-moveMultiplayer.x,0));
    }
    
    public void IncrementDragX()
    {
        if(enableBoyKeyboard)
            controlCamera.MoveCamera(new Vector2(moveMultiplayer.x,0));
    }
    
    public void DecrementDragY()
    {
        if(enableBoyKeyboard)
            controlCamera.MoveCamera(new Vector2(0, -moveMultiplayer.y));
    }
    
    public void IncrementDragY()
    {
        if(enableBoyKeyboard)
            controlCamera.MoveCamera(new Vector2(0, moveMultiplayer.y));
    }
}
