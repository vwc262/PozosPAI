using Lean.Touch;
using UnityEngine;

public class ControlCameraTouch : MonoBehaviour
{
    private ControlMoveCameraMap cameraMap;
        
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
    
    public void InputTouch()
    {
        if (cameraMap != null)
        {
            if (LeanTouch.Fingers.Count == 1)
            {
                Debug.Log("Touch" + LeanTouch.Fingers[0].ToString());
                
                // cameraMap.transform.position = ClampMove(cameraBase.transform.position +
                //                                          new Vector3(
                //                                              LeanTouch.Fingers[0].ScaledDelta.x *
                //                                              GetMultipliInputTouch(),
                //                                              0,
                //                                              LeanTouch.Fingers[0].ScaledDelta.y *
                //                                              GetMultipliInputTouch()));
            }

            // if (LeanTouch.Fingers.Count == 2)
            // {
            //     //Debug.Log("Touch" + LeanTouch.Fingers[0].ToString());
            //
            //     distance = Vector2.Distance(LeanTouch.Fingers[0].ScreenPosition, LeanTouch.Fingers[1].ScreenPosition);
            //
            //     if (isInitTouch2Fingers)
            //     {
            //         cameraBase.transform.position = ClampMove(cameraBase.transform.position +
            //                                                   new Vector3(
            //                                                       0,
            //                                                       (distance - distanceAnt) * MultiplyInputZoomTouch,
            //                                                       0));
            //     }
            //
            //     distanceAnt = distance;
            //     isInitTouch2Fingers = true;
            //
            //     CalculaZoom(cameraBase.transform.position);
            // }
            // else
            // {
            //     isInitTouch2Fingers = false;
            // }
        }
    }
}
