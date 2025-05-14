using System.Collections.Generic;
using UnityEngine;

public class VWC_MoveCamera_PAI : Singleton<VWC_MoveCamera_PAI>
{
    public VWC_MoveCamera moveCamera;

    public List<Vector3> positions;
    
    public Vector3 OffsetMov;
    
    public void SetMoveCameraByRigionID(int zone)
    {
        switch (zone)
        {
            case 14:
                moveCamera.SetPointZoom(positions[0].x, positions[0].y, positions[0].z);
                break;
            case 17:
                moveCamera.SetPointZoom(positions[1].x, positions[1].y, positions[1].z);
                break;
            case 18:
                moveCamera.SetPointZoom(positions[2].x, positions[2].y, positions[2].z);
                break;
        }
    }

    public void SetLimitsCameraMovement()
    {
        if (ControlDatos._singletonExists && moveCamera != null)
        {
            moveCamera.flyCamera.positionMax = Gps2UnityConverter.GPS2Unity(ControlDatos.singleton.maxLatitud,
                ControlDatos.singleton.maxLongitud);
            moveCamera.flyCamera.positionMin  = Gps2UnityConverter.GPS2Unity(ControlDatos.singleton.minLatitud,
                ControlDatos.singleton.minLongitud);

            moveCamera.flyCamera.positionMax += OffsetMov;
            moveCamera.flyCamera.positionMin -= OffsetMov;
        }
    }
}
