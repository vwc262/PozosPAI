using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class VWCBillboardSitio : MonoBehaviour
{
    // Hola Boy
    public GameObject cameraGimbal;
    public ControlMoveCameraMap _cameraMapMoveControl;
    public ControlMarcadorSitio controlSitio;
    
    [TabGroup("Angle")] public Vector3 minAngle;
    [TabGroup("Angle")] public Vector3 maxAngle;
    
    [TabGroup("Height")] public float distance;
    [TabGroup("Height")] public float interpolationValueHeight;
    [TabGroup("Height")] public AnimationCurve curve;
    [TabGroup("Height")] public float maxHeightDistance;
    [TabGroup("Height")] public float minHeight;
    [TabGroup("Height")] public float maxHeight;
    
    [TabGroup("Scale")] public Vector3 minScale = new Vector3(3,3,3);
    [TabGroup("Scale")] public Vector3 maxScale = new Vector3(1,1,1);

    [TabGroup("Position")] public float interpolationValuePos;
    [TabGroup("Position")] public float interpolationValuePosMax = 0.8f;
    [TabGroup("Position")] public Vector3 positionFinalMarcador;
    [TabGroup("Position")] public Vector3 positionGPSOriginal;
    
    [TabGroup("GUI")] public GameObject guiHeightDistance;
    [TabGroup("GUI")] public GameObject guiDespSelection;
    [TabGroup("GUI")] public Vector3 despGuiSelected;
    [TabGroup("GUI")] public Vector3 posGuiOriginal;
    
    [TabGroup("Deformation")] public GameObject[] DeformationObjects;
    [TabGroup("Deformation")] public float deformationFactor;
    [TabGroup("Deformation")] public float deformationDistanceFactor; 
    [TabGroup("Deformation")] public float deformationDistanceFactorOffset;
    [TabGroup("Deformation")] public bool useDistanceDeformationZ;
    
    public bool useChangeAngle;
    public bool useChangeScale;
    public bool useChangeGUIHeight;
    public bool useGPSDisplacement;
    public bool useMoveGUISelected;
    
    public void Start()
    {
        cameraGimbal = FindObjectOfType<ControlMoveCameraMap>().CameraGimbal;
        RecalculatePerspectiveDeformation();
        RecalculateHeight();
        posGuiOriginal = guiDespSelection.transform.localPosition;
    }

    public void RecalculateTilt(float _interpolationValueAngle)
    {
        RecalculateHeight();
    }
    
    public void RecalculateZoom(float _interpolationValuePos)
    {
        RecalculateHeight();
    }

    public void RecalculateDrag(float _interpolationValueHeight)
    {
        RecalculateHeight();
    }
    
    public virtual void RecalculateHeight()
    {
        if (_cameraMapMoveControl == null && ControlMoveCamera._singletonExists)
            _cameraMapMoveControl = ControlMoveCamera.singleton.moveCamera;
        if (cameraGimbal == null && _cameraMapMoveControl != null)
            cameraGimbal = _cameraMapMoveControl.cinemachineBrainMainCamera.gameObject;
        
        interpolationValuePos = Mathf.Max(_cameraMapMoveControl.ZoomCinemachineCamera, _cameraMapMoveControl.tiltValue);
        
        if (useChangeAngle)
            transform.localEulerAngles = Vector3.Lerp(minAngle, maxAngle, _cameraMapMoveControl.tiltValue);
        
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
                Vector3.Lerp(new Vector3(0, minHeight, 0), new Vector3(0, maxHeight, 0), interpolationValueHeight);
            
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
        
        //RecalculatePerspectiveDeformation();
    }

    public void MoveGUISelectedSitio()
    {
        if (controlSitio != null)
        {
            if (controlSitio.sitio.isSelected)
                guiDespSelection.transform.localPosition = posGuiOriginal + despGuiSelected;
            else
                guiDespSelection.transform.localPosition = posGuiOriginal;
        }
    }

    public void RecalculatePerspectiveDeformation()
    {
        foreach (var obj in DeformationObjects)
        {
            if(obj == null) 
                continue;
            
            var scaleX = obj.transform.localScale.x;

            var distanceForDeformation = cameraGimbal.gameObject.transform.position - transform.position;

            var distanceDeform = useDistanceDeformationZ
                ? distanceForDeformation.z
                : distanceForDeformation.x;
            
            deformationFactor = 1 + Mathf.Abs(distanceDeform) *
                deformationDistanceFactor * (interpolationValuePos + deformationDistanceFactorOffset);
            obj.transform.localScale = obj.transform.localScale.with(y: scaleX * deformationFactor);
        }
    }

    public void SetSelectedSitio()
    {
        if (useMoveGUISelected)
            MoveGUISelectedSitio();
    }
    
    public void SetDeselectedSitio()
    {
        if (useMoveGUISelected)
            MoveGUISelectedSitio();
    }
}
