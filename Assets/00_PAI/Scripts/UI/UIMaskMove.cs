using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaskMove : MonoBehaviour
{
    // Hola Boy

    public GameObject child;

    [Range(0,1)]
    public float posYLerp;

    public float minPosY;
    public float maxPosY;
    
	private void OnValidate()
    {
        UpdatePositions();
    }

    private void Update()
    {
        UpdatePositions();
    }

    private void UpdatePositions()
    {
        var pos = transform.localPosition;
        var yPos = Mathf.Lerp(minPosY, maxPosY, posYLerp);
        transform.localPosition = pos.with(y: yPos);
        child.transform.localPosition = pos.with(y: -yPos);
    }
}
