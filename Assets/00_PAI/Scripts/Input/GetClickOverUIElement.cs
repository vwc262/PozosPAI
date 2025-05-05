using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class GetClickOverUIElement : MonoBehaviour, IPointerClickHandler
{
    public bool IsClicOverElement;

    public Canvas MyCanvas;
    
    public BoyRaycastSitio RaycastSitio;

    public bool isCircularElement;


    public float DeltaTimeTap = 0.5f;

    private void Start()
    {
        MyCanvas = GetComponentInParent<Canvas>();
    }

    private void Update()
    {
        IsClicOverElement = IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    public void OnPointerClick(PointerEventData ED)
    {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponent<RectTransform>(), ED.position, ED.pressEventCamera, out localCursor))
            return;
        
        if (Lean.Touch.LeanTouch.Fingers.Count() > 1)
            return;

        if (Lean.Touch.LeanTouch.Fingers.Count() == 1)
            if (Lean.Touch.LeanTouch.Fingers[0].Age > DeltaTimeTap)
                return;

        if (isCircularElement)
        {
            Vector2 localCursorCircular = localCursor;
            
            // -0.5 a 0.5
            // localCursor.x /= GetComponent<RectTransform>().rect.width;
            // localCursor.y /= GetComponent<RectTransform>().rect.height;
            
            if (GetComponent<RectTransform>().rect.width < GetComponent<RectTransform>().rect.height)
            {
                localCursorCircular.x /= GetComponent<RectTransform>().rect.width;
                localCursorCircular.y /= GetComponent<RectTransform>().rect.width;
            }
            else
            {
                localCursorCircular.x /= GetComponent<RectTransform>().rect.height;
                localCursorCircular.y /= GetComponent<RectTransform>().rect.height;
            }
            
            
            if (localCursorCircular.magnitude <= 0.5f)
            {
                //Normalizar 0 a 1
                localCursorCircular.x += 0.5f;
                localCursorCircular.y += 0.5f;

                if (RaycastSitio != null)
                    RaycastSitio.DoClickOverMap(localCursorCircular);
            }
        }
        else
        {
            if (!InteractionOverUI_List.GetIsInteractionOverUI_List())
            {
                // -0.5 a 0.5
                localCursor.x /= GetComponent<RectTransform>().rect.width;
                localCursor.y /= GetComponent<RectTransform>().rect.height;

                //Normalizar 0 a 1
                localCursor.x += 0.5f;
                localCursor.y += 0.5f;

                if (RaycastSitio != null)
                    RaycastSitio.DoClickOverMap(localCursor);
            }
        }
    }
    
    public bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults )
    {
        for(int index = 0;  index < eventSystemRaysastResults.Count; index ++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults [index];

            if (curRaysastResult.gameObject == this.gameObject)
                return true;
        }

        return false;
    }

    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {   
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position =  Input.mousePosition;

        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll( eventData, raysastResults );

        return raysastResults;
    }
    
    public Vector2 GetPoitInUIElement(Vector2 _position)
    {
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                GetComponent<RectTransform>(), _position, MyCanvas.worldCamera,
                out localCursor))
        {
            return Vector2.zero;
        }

        //Normalizar 0 a 1
        localCursor.x /= GetComponent<RectTransform>().rect.width;
        localCursor.y /= GetComponent<RectTransform>().rect.height;

        localCursor.x += 0.5f;
        localCursor.y += 0.5f;

        return localCursor;
    }
}
