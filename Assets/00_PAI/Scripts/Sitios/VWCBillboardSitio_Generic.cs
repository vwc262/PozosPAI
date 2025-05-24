using UnityEngine;

public class VWCBillboardSitio_Generic : VWCBillboardSitio
{
    public GameObject rootBillboard;
    
    public override void RecalculateHeight()
    {
        if (_cameraMapMoveControl == null && ControlMoveCamera._singletonExists)
            _cameraMapMoveControl = ControlMoveCamera.singleton.moveCamera;
        if (cameraGimbal == null && _cameraMapMoveControl != null)
            cameraGimbal = _cameraMapMoveControl.cinemachineBrainMainCamera.gameObject;
        
        interpolationValuePos = Mathf.Max(_cameraMapMoveControl.ZoomCinemachineCamera, _cameraMapMoveControl.tiltValue);
        
        if (useChangeAngle)
            rootBillboard.transform.localEulerAngles = Vector3.Lerp(minAngle, maxAngle, _cameraMapMoveControl.tiltValue);
        
        if (useChangeScale)
            transform.localScale = Vector3.Lerp(minScale, maxScale, interpolationValuePos);
        
        if (useMoveGUISelected)
            MoveGUISelectedSitio();
        
        if (useChangeGUIHeight)
        {
            distance = Vector3.Distance(cameraGimbal.gameObject.transform.position, transform.position);
            interpolationValueHeight = curve.Evaluate(distance / maxHeightDistance) *
                                       _cameraMapMoveControl.tiltValue;

            var heightOffset =
                Vector3.Lerp(new Vector3(0, 0, minHeight), new Vector3(0, 0, maxHeight), interpolationValueHeight);
            
            guiHeightDistance.transform.localPosition = heightOffset;
        }

        if (useGPSDisplacement)
        {
            float posInterpolation = interpolationValuePos > interpolationValuePosMax
                ? interpolationValuePosMax
                : interpolationValuePos;
            
            var newPos = Vector3.Lerp(positionFinalMarcador, positionGPSOriginal, posInterpolation);

            transform.localPosition = newPos;
        }
    }
}
