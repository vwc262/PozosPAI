using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlMoveCamera : Singleton<ControlMoveCamera>
{
    public ControlMoveCameraMap moveCamera;

    public Vector3 positionCenterEstructura, MaxPos, MinPos;
    
    public Vector3 OffsetMov;

    public List<ControlSitio> listSitiosEstructura;
    
    public void SetMoveCameraByRegionID(int estructura)
    {
        if (ControlDatos._singletonExists)
        {
            listSitiosEstructura = ControlDatos.singleton.listSitios.Where(
                x => x.dataSitio.Estructura == estructura).ToList();

            MaxPos.x = listSitiosEstructura.Max(x => x.controlMarcadorMap.transform.position.x);
            MaxPos.z = listSitiosEstructura.Max(x => x.controlMarcadorMap.transform.position.z);
            
            MinPos.x = listSitiosEstructura.Min(x => x.controlMarcadorMap.transform.position.x);
            MinPos.z = listSitiosEstructura.Min(x => x.controlMarcadorMap.transform.position.z);
            
            positionCenterEstructura = new Vector3(
                MinPos.x + ((MaxPos.x - MinPos.x) / 2f),
                moveCamera.flyCamera.transform.position.y,
                MinPos.z + ((MaxPos.z - MinPos.z) / 2f));
            
            moveCamera.flyCamera.SetPosition(positionCenterEstructura);
        }
    }

    public void SetLimitsCameraMovement()
    {
        if (ControlDatos._singletonExists && moveCamera != null)
        {
            moveCamera.flyCamera.positionMax = Gps2UnityConverter.GPS2Unity(
                ControlDatos.singleton.maxLatitud,
                ControlDatos.singleton.maxLongitud);
            moveCamera.flyCamera.positionMin  = Gps2UnityConverter.GPS2Unity(
                ControlDatos.singleton.minLatitud,
                ControlDatos.singleton.minLongitud);

            moveCamera.flyCamera.positionMax += OffsetMov;
            moveCamera.flyCamera.positionMin -= OffsetMov;
        }
    }

    public void ResetCenterCamera()
    {
        if (ControlMap._singletonExists)
        {
            moveCamera.ResetHomeXZ(Gps2UnityConverter.GPS2Unity(
                ControlMap.singleton.latitudCenterPozos + ControlMap.singleton.latitudOffset, 
                ControlMap.singleton.longitudCenterPozos + ControlMap.singleton.longitudOffset));
        }
    }

    public void ResetCamera()
    {
        if (ControlPrefabs._singletonExists)
            moveCamera.ResetHomeXZ(new Vector3(0, 0, 0) + ControlPrefabs.singleton.GetOffsetPrefab());
        else
            moveCamera.ResetHomeXZ(new Vector3(0, 0, 0));
    }
}
