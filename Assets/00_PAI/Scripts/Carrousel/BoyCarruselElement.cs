using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class BoyCarruselElement : MonoBehaviour
{
    // Hola Boy
    public int carruselElementIndex;
    public int lastElementIndex;
    [PropertyRange(0,1f)]
    public float scaleFactor;
    public BoyCarruselElementData[] elementObjects;

    public float alpha;
    public bool mainWindow;
    
    
    [SerializeField]
    private CanvasGroup _canvasGroup;
    
    private void OnValidate()
    {
	    //UpdateCarruselElement();
    }

    private void Start()
    {
	    UpdateCarruselElement();
    }

    void Update()
    {
	    if (lastElementIndex != carruselElementIndex)
	    {
		    UpdateCarruselElement();
	    }

	    _canvasGroup.alpha = alpha;
    }

    public void UpdateCarruselElement()
    {
	    for (int i = 0; i < elementObjects.Length; i++)
	    {
		    elementObjects[i].UpdateElement(i);
	    }
	    
	    lastElementIndex = carruselElementIndex;
	    SetScaleFactor(scaleFactor);
    }
    
    public void SetScaleFactor(float val)
    {
	    scaleFactor = val;
	    transform.localScale = elementObjects[carruselElementIndex].scale * val;
    }

    public void SetTransparency(float val)
    {
	    _canvasGroup.alpha = val;
    }
}