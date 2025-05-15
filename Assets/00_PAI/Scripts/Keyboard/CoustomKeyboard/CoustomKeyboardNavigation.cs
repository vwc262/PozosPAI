using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Touch;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class CoustomKeyboardNavigation : CoustomKeyboard
{
    // Hola Boy
    public VWC_MoveCamera cameraControl;
    
    
    [BoxGroup("Zoom")]
    public float zoomIncrement;
    [BoxGroup("Pan")]
    public float panAction;
    [BoxGroup("Pan")]
    public float panIncrement;
    [BoxGroup("Pan")]
    public float minPanAction;
    [BoxGroup("Pan")]
    public float panAttenuation;
    
    [BoxGroup("Drag")]
    public float dragSpeed;
    [BoxGroup("Drag")]
    public float dragAttenuation;
    [BoxGroup("Drag")]
    public Vector2 dragAction;
    [BoxGroup("Drag")]
    public float dragIncrement;
    [BoxGroup("Drag")]
    public float minDragInput;

    public bool zoomPan = false;
    
    private void Start()
    {
	    if (BoyCoustomKeyboard._singletonExists)
	    {
		    BoyCoustomKeyboard.singleton.Key1_Event.AddListener(GoHome);
		    
		    BoyCoustomKeyboard.singleton.Knob1_L_Event.AddListener(DecrementDragX);
		    BoyCoustomKeyboard.singleton.Knob1_R_Event.AddListener(IncrementDragX);
		    BoyCoustomKeyboard.singleton.Knob2_L_Event.AddListener(DecrementDragY);
		    BoyCoustomKeyboard.singleton.Knob2_R_Event.AddListener(IncrementDragY);
		    BoyCoustomKeyboard.singleton.Knob3_C_Event.AddListener(ToogleZoomPan);
		    BoyCoustomKeyboard.singleton.Knob3_L_Event.AddListener(DecrementZoomPan);
		    BoyCoustomKeyboard.singleton.Knob3_R_Event.AddListener(IncrementZoomPan);
	    }
    }

    private void SetPan()
    {
	    cameraControl.SetTouchInputTiltFloat(panAction);
	    panAction *= panAttenuation;
    }
	
    private void SetDrag()
    {
		cameraControl.SetTouchInputDragNoFinger(dragAction, dragSpeed * dragAction.magnitude);
	    dragAction *= dragAttenuation;
    }

    public void IncrementZoom()
    {
	    if(enableBoyKeyboard)
			cameraControl.cameraZoomMap.AddToZoom(zoomIncrement);
    }
    
    public void DecrementZoom()
    {
	    if(enableBoyKeyboard)
			cameraControl.cameraZoomMap.AddToZoom(-zoomIncrement);
    }
    
	public void IncrementPan()
	{
		if(enableBoyKeyboard)
			panAction += panIncrement;
	}
    
	public void DecrementPan()
	{
		if(enableBoyKeyboard)
			panAction -= panIncrement;
	}
	
	public void IncrementDragX()
	{
		if(enableBoyKeyboard)
			dragAction.x += dragIncrement;
	}
    
	public void DecrementDragX()
	{
		if(enableBoyKeyboard)
			dragAction.x -= dragIncrement;
	}
	
	public void IncrementDragY()
	{
		if(enableBoyKeyboard)
			dragAction.y += dragIncrement;
	}
    
	public void DecrementDragY()
	{
		if(enableBoyKeyboard)
			dragAction.y -= dragIncrement;
	}

	public void IncrementZoomPan()
	{
		if (zoomPan)
			IncrementZoom();
		else
			IncrementPan();
	}
	
	public void DecrementZoomPan()
	{
		if (zoomPan)
			DecrementZoom();
		else
			DecrementPan();
	}

	public void ToogleZoomPan()
	{
		if(enableBoyKeyboard)
			zoomPan = !zoomPan;
	}

	public override void SetEnable(bool val)
	{
		enableBoyKeyboard = val;
		if (val)
			ResetActions();
	}

	private void ResetActions()
	{
		dragAction = Vector2.zero;
	}

	public void GoHome()
	{
		if (ControlSelectedSitio._singletonExists)
			ControlSelectedSitio.singleton.DeseleccionarSitio();
		cameraControl.GoHome();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F12))
		{
			enableBoyKeyboard = !enableBoyKeyboard;
		}
		if(!enableBoyKeyboard)
			return;
		
		if (dragAction.magnitude > minDragInput)
			if (!cameraControl.coroutinePos)
				SetDrag();
		if (Mathf.Abs(panAction) > minPanAction)
			SetPan();
	}
}
