using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class BoyCarrusel : MonoBehaviour
{
    // Hola Boy
    public BoyCarruselElement[] carruselElements;
    public int hidenElementIndex;

    public List<int> elementIndexes = new List<int>();


    public UnityEvent updateToRight;
    public UnityEvent updateToLeft;
    
    public float deltaSwipe = 800f;
    
    public List<InteractionOverUI_List> ListPanelsUI;
	private void OnValidate()
    {
        
    }
	   
	private void Awake()
    {
        
    }
	
    void Start()
    {
	    elementIndexes.Clear();
	    foreach (var element in carruselElements)
	    {
		    elementIndexes.Add(element.carruselElementIndex);
	    }
	    
	    ListPanelsUI.AddRange(gameObject.GetComponentsInChildren<InteractionOverUI_List>());
    }

    
    void Update()
    {
        
    }

    public void UpdateCarruselBySign(int val)
    {
	    if (val > 0)
		    UpdateCarruselToRight();
	    if (val < 0)
		    UpdateCarruselToLeft();
    }
    
    
    [Button]
    public void UpdateCarruselToLeft()
    {
	    elementIndexes[^1] = elementIndexes[0];
	    for (int i = 0; i < elementIndexes.Count; i++)
	    {
		    var next_i = (i + 1) % elementIndexes.Count;
		    elementIndexes[i] = elementIndexes[next_i];
	    }
	    
	    UpdateCarruselElements();
    }
    
    [Button]
    public void UpdateCarruselToRight()
    {
	   elementIndexes[^1] = elementIndexes[elementIndexes.Count - 2];
	    for (int i = elementIndexes.Count -1; i >= 0 ; i--)
	    {
		    var next_i = i - 1;
		    if (next_i < 0) next_i += elementIndexes.Count;
		    elementIndexes[i] = elementIndexes[next_i];
	    }

	    UpdateCarruselElements();
    }
    
    public void UpdateCarruselElements()
    {
	    for (int i = 0; i < carruselElements.Length; i++)
	    {
		    if(i < carruselElements.Length - 1)
				carruselElements[i].carruselElementIndex = elementIndexes[i];
		    else
			    carruselElements[i].carruselElementIndex = hidenElementIndex;

		    carruselElements[i].UpdateCarruselElement();
	    }

    }
    
    public void SwipeDelta(Vector2 _delta)
    {
	    //if (InteractionOverUILerma.GetInteractionOverUI())
	    if (GetInteractionOverUI())
	    {
		    Debug.Log("delta: " + _delta);

		    if (_delta.x > 0)
			    updateToRight.Invoke();
		    else
			    updateToLeft.Invoke();
	    }
    }
    
    public bool GetInteractionOverUI()
    {
	    foreach(InteractionOverUI_List item in ListPanelsUI)
	    {
		    if (item.GetIsInteractionOverUI())
			    return true;
	    }

	    return false;
    }
}
 