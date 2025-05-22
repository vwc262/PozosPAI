using Sirenix.OdinInspector;
using UnityEngine;

public class VWCBillboardSitio : MonoBehaviour
{
    // Hola Boy
    public GameObject cameraGimbal;
    public ControlMoveCameraMap _cameraMapMoveControl;
    public ControlSitio sitio;

    public bool useDistanceDeformationZ;
    
    public SpriteRenderer frameDark;
    public SpriteRenderer circleID;
    
    [TabGroup("Angle")] public Vector3 minAngle;
    [TabGroup("Angle")] public Vector3 maxAngle;
    
    [TabGroup("Height")] public float distance;
    [TabGroup("Height")] public float interpolationValueHeight;
    [TabGroup("Height")] public AnimationCurve curve;
    [TabGroup("Height")] public float maxHeightDistance;
    [TabGroup("Height")] public float minHeight;
    [TabGroup("Height")] public float maxHeight;
    
    [TabGroup("Scale")][ShowInInspector] public static Vector3 minScale = new Vector3(3,3,3);
    [TabGroup("Scale")][ShowInInspector] public static Vector3 maxScale = new Vector3(1,1,1);

    [TabGroup("Position")] public float interpolationValuePos;
    [TabGroup("Position")] public float interpolationValuePosMax = 0.8f;
    [TabGroup("Position")] public Vector3 positionFinalMarcador;
    [TabGroup("Position")] public Vector3 positionGPSOriginal;
    
    [TabGroup("GUI")]public GameObject guiObject;
    [TabGroup("GUI")]public GameObject guiObject2;
    [TabGroup("GUI")]public Vector3 guiObjPosTilt;
    [TabGroup("GUI")]public Vector3 posGuiOriginal;
    
    [TabGroup("Deformation")]public GameObject[] DeformationObjects;
    [TabGroup("Deformation")]public float deformationFactor;
    [TabGroup("Deformation")]public float deformationDistanceFactor; 
    [TabGroup("Deformation")]public float deformationDistanceFactorOffset;
    
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
        posGuiOriginal = guiObject2.transform.localPosition;
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
    
    public void RecalculateHeight()
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
            
            guiObject.transform.localPosition = heightOffset;
        }

        if (useGPSDisplacement)
        {
            float posInterpolation = interpolationValuePos > interpolationValuePosMax
                ? interpolationValuePosMax
                : interpolationValuePos;
            var newPos = Vector3.Lerp(positionFinalMarcador, positionGPSOriginal, posInterpolation);

            transform.localPosition = newPos;
        }
        
        if (frameDark != null)
            frameDark.material.color = new Color(0, 0, 0, interpolationValuePos * 2);
        
        RecalculatePerspectiveDeformation();
    }

    public void MoveGUISelectedSitio()
    {
        var pos1 = posGuiOriginal + guiObjPosTilt;
        
        if (sitio.isSelected) 
            //guiObject2.transform.localPosition = Vector3.Lerp(posGuiOriginal, pos1, interpolationValueAngle);
            guiObject2.transform.localPosition = Vector3.Lerp(posGuiOriginal, pos1, 1);
        else
            guiObject2.transform.localPosition = posGuiOriginal;
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
}
