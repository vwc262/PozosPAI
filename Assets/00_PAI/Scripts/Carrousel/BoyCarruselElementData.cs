using System;
using UnityEngine;

[Serializable]
public class BoyCarruselElementData : MonoBehaviour
{
    public string elementName;
    public GameObject elementTypeRootObject;
    public GameObject mainWindowRootObject;
    public GameObject secondaryWindowRootObject;
    public Vector3 scale = Vector3.one;
    public int elementIndex;

    public BoyCarruselElement manager;

    public void UpdateElement(int index)
    {
        elementIndex = index;
		
        if (elementIndex == manager.carruselElementIndex)
        {
            if(mainWindowRootObject != null)
                mainWindowRootObject.SetActive(manager.mainWindow);
            if(secondaryWindowRootObject != null)
                secondaryWindowRootObject.SetActive(!manager.mainWindow);
        }
        else
        {
            if(mainWindowRootObject != null)
                mainWindowRootObject.SetActive(false);
            if(secondaryWindowRootObject != null)
                secondaryWindowRootObject.SetActive(false);
        }
		
        if(elementTypeRootObject != null)
        {
            elementTypeRootObject.SetActive(elementIndex == manager.carruselElementIndex);
        }
    }
    
    
}