using Lean.Touch;
using UnityEngine;

public class ControlCameraTouch : MonoBehaviour
{
    private ControlMoveCameraMap cameraMap;
    
    public float MultiplyInputZoomTouch;
    public float MultiplyInputTiltTouch;
    public float MultiplyInputMoveTouchMax;
    public float MultiplyInputMoveTouchMin;

    public float distance, distanceAnt;
    public bool isInitTouch2Fingers;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraMap = gameObject.GetComponent<ControlMoveCameraMap>();
    }

    // Update is called once per frame
    void Update()
    {
        InputTouch();
    }
    
    public float GetMultipliInputTouch()
    {
        return Mathf.Lerp(MultiplyInputMoveTouchMax, MultiplyInputMoveTouchMin, cameraMap.zoomValue);
    }
    
    public void InputTouch()
    {
            if (cameraMap != null)
            {
                if (LeanTouch.Fingers.Count == 1)
                {
                    if (!LeanTouch.GuiInUse && !InteractionOverUI_List.GetIsInteractionOverUI_List())
                    {
                        cameraMap.SetTouchInputDrag(new Vector2(
                            LeanTouch.Fingers[0].ScaledDelta.x * GetMultipliInputTouch(),
                            LeanTouch.Fingers[0].ScaledDelta.y * GetMultipliInputTouch()));
                    }
                }

                if (LeanTouch.Fingers.Count == 2)
                {
                    if (!LeanTouch.GuiInUse && !InteractionOverUI_List.GetIsInteractionOverUI_List())
                    {
                        distance = Vector2.Distance(LeanTouch.Fingers[0].ScreenPosition,
                            LeanTouch.Fingers[1].ScreenPosition);

                        if (isInitTouch2Fingers)
                        {
                            cameraMap.SetTouchInputZoom((distance - distanceAnt) * MultiplyInputZoomTouch);
                        }

                        distanceAnt = distance;
                        isInitTouch2Fingers = true;
                    }
                }
                else
                {
                    isInitTouch2Fingers = false;
                }

                if (LeanTouch.Fingers.Count == 3)
                {
                    if (!LeanTouch.GuiInUse && !InteractionOverUI_List.GetIsInteractionOverUI_List())
                    {
                        cameraMap.SetTouchInputTilt(LeanTouch.Fingers[0].ScaledDelta.y * MultiplyInputTiltTouch);
                    }
                }
            }
    }
}
