using UnityEngine;

public class ControlLetrero : MonoBehaviour
{
    public int regionID;
    
    public void CenterRegion()
    {
        if (ControlMoveCamera._singletonExists)
            ControlMoveCamera.singleton.SetMoveCameraByRegionID(regionID);
    }
}
