using Raskulls.ScriptableSystem;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InteractionOverUI_List : MonoBehaviour
{
    [ShowInInspector]
    public static List<InteractionOverUI_List> ListUI = new List<InteractionOverUI_List>();

    public static bool isDragingOverUI;
    public bool isInteractionOverUI;

    private void Start()
    {
        if (!ListUI.Exists(item => item == this))
        {
            ListUI.Add(this);
        }
    }

    private void OnDestroy()
    {
        ListUI.Remove(this);
    }

    public static bool GetIsInteractionOverUI_List()
    {
        foreach(InteractionOverUI_List item in ListUI)
        {
            if (item.GetIsInteractionOverUI())
                return true;
        }

        return false;
    }

    // Update is called once per frame
    public bool GetIsInteractionOverUI()
    {
        isInteractionOverUI = IsPointerOverUIElement(GetEventSystemRaycastResults());

        return isInteractionOverUI;
    }

    public bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];

            //if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("IgnoreScrollView"))
            if (curRaysastResult.gameObject == this.gameObject)
                return true;
        }

        return false;
    }

    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        return raysastResults;
    }
}
