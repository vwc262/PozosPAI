using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class VWCBillboardSitio : MonoBehaviour
{
    // Hola Boy
    private GameObject cameraMove;
    private VWC_MoveCamera cameraMoveVWC;
    [FormerlySerializedAs("sitioGPS")] public ControlMarcadorSitio controlMarcadorSitio;

    public bool useDistance_X_Z;
    
    public SpriteRenderer frameDark;
    public SpriteRenderer circleID;
    
    [TabGroup("Angle")] public float interpolationValueAngle;
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
    [TabGroup("Position")] public float interpolationValuePosAux;
    [TabGroup("Position")] public float interpolationValuePosMax = 0.8f;
    [TabGroup("Position")] public Vector3 positionFinalMarcador;
    [TabGroup("Position")] public Vector3 positionGPSOriginal;
    [TabGroup("Position")] public Vector3 pos0GUIAtPlay;
    [TabGroup("Position")] public Vector3 posOriginalBillboard;
    [TabGroup("Position")] public Vector3 posOverlay;
    
    [TabGroup("GUI")]public GameObject guiObject;
    [TabGroup("GUI")]public GameObject guiObject2;
    [TabGroup("GUI")]public Vector3 guiObjPosTilt;
    [TabGroup("GUI")]Vector3 posGuiOriginal;
    
    [TabGroup("LineRender")] public Vector3 lineRenderOffsetStart;
    [TabGroup("LineRender")] public Vector3 lineRenderOffsetFinish;
    [TabGroup("LineRender")] public LineRenderer lineRenderer;
    
    [TabGroup("Deformation")]public GameObject[] DeformationObjects;
    [TabGroup("Deformation")]public float deformationFactor;
    [TabGroup("Deformation")]public float deformationDistanceFactor; 
    [TabGroup("Deformation")]public float deformationDistanceFactorOffset;
    
    public bool useChangeAngle;
    public bool useChangeScale;
    public bool useChangeGUIHeight;
    public bool useGPSDisplacement;
    public bool useMoveGUISelected;
    
    void Start()
    {
        //_originalPos = positionGPSOriginal;
        cameraMove = FindObjectOfType<VWC_MoveCamera>().CameraGimbal;

        pos0GUIAtPlay = transform.localPosition;
        RecalculatePerspectiveDeformation();
        RecalculateHeight();
        posGuiOriginal = guiObject2.transform.localPosition;
    }

    public void RecalculateTilt(float _interpolationValueAngle)
    {
        interpolationValueAngle = _interpolationValueAngle;
        
        RecalculateHeight();
    }
    
    public void RecalculateZoom(float _interpolationValuePos)
    {
        interpolationValuePosAux =_interpolationValuePos;
        
        RecalculateHeight();
    }

    public void RecalculateDrag(float _interpolationValueHeight)
    {
        RecalculateHeight();
    }
    
    private void RecalculateHeight()
    {
        if (cameraMove == null)
            cameraMove = FindObjectOfType<VWC_MoveCamera>().CameraGimbal;
        if (cameraMoveVWC == null)
            cameraMoveVWC = cameraMove.GetComponentInParent<VWC_MoveCamera>();
        
        interpolationValuePos = Mathf.Max(interpolationValuePosAux, interpolationValueAngle);
        
        if (useChangeAngle)
            transform.localEulerAngles = Vector3.Lerp(minAngle, maxAngle, interpolationValueAngle);
        
        if (useChangeScale)
            transform.localScale = Vector3.Lerp(minScale, maxScale, interpolationValuePos);
        
        if (useMoveGUISelected)
            MoveGUISelectedSitio();
        
        if (useChangeGUIHeight)
        {
            distance = Vector3.Distance(cameraMove.gameObject.transform.position, transform.position);
            interpolationValueHeight = curve.Evaluate(distance / maxHeightDistance) *
                                       cameraMoveVWC.tiltValue;

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
        
        frameDark.material.color = new Color(0, 0, 0, interpolationValuePos * 2);
        
        RecalculatePerspectiveDeformation();
    }

    private void MoveGUISelectedSitio()
    {
        var pos1 = posGuiOriginal + guiObjPosTilt;
        
        if (controlMarcadorSitio.selectedSitio) 
            //guiObject2.transform.localPosition = Vector3.Lerp(posGuiOriginal, pos1, interpolationValueAngle);
            guiObject2.transform.localPosition = Vector3.Lerp(posGuiOriginal, pos1, 1);
        else
            guiObject2.transform.localPosition = posGuiOriginal;
    }

    public void RecalculateLineRenderer()
    {
        lineRenderer.SetPosition(0, transform.parent.position+lineRenderOffsetStart);
        lineRenderer.SetPosition(1, transform.position+lineRenderOffsetFinish);
    }

    public void RecalculatePerspectiveDeformation()
    {
        foreach (var obj in DeformationObjects)
        {
            if(obj == null) 
                continue;
            
            var scaleX = obj.transform.localScale.x;

            var distanceForDeformation = cameraMove.gameObject.transform.position - transform.position;

            var distanceDeform = useDistance_X_Z
                ? distanceForDeformation.z
                : distanceForDeformation.x;
            
            deformationFactor = 1 + Mathf.Abs(distanceDeform) *
                deformationDistanceFactor * (interpolationValuePos + deformationDistanceFactorOffset);
            obj.transform.localScale = obj.transform.localScale.with(y: scaleX * deformationFactor);
        }
    }
    
    // public void Recalculate2(float _interpolationValue)
    // {
    //     if (cameraMove == null)
    //         cameraMove = FindObjectOfType<VWC_Lerma_MoveCamera>().CameraGimbal;
    //     
    //     interpolationValueAngle = _interpolationValue;
    //     transform.localEulerAngles = Vector3.Lerp(minAngle, maxAngle, interpolationValueAngle);
    //     
    //     transform.localScale = Vector3.Lerp(minScale, maxScale, interpolationValuePos);
    //
    //     distance = Vector3.Distance(cameraMove.gameObject.transform.position, transform.position);
    //     interpolationValueHeight = curve.Evaluate(distance / maxHeightDistance);
    //
    //     var heightOffset = Vector3.Lerp(new Vector3(0,minHeight,0),new Vector3(0,maxHeight,0), interpolationValueHeight);
    //     
    //     var newPos = Vector3.Lerp(pos0GUIAtPlay, originalPos, interpolationValuePos);
    //
    //     transform.localPosition = newPos + heightOffset;
    //     frameDark.material.color = new Color(0, 0, 0, interpolationValuePos*2);
    //
    //     RecalculateLineRenderer();
    // }
}
