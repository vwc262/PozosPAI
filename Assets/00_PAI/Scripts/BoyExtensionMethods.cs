using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public static class BoyExtensionMethods
{
    public static Vector3 with(this Vector3 original, float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(x?? original.x, y?? original.y, z?? original.z);
    }

    public static void CopyAllTo<T>(this T source, T target)
    {
        var type = typeof(T);
        foreach (var sourceProperty in type.GetProperties())
        {
            var targetProperty = type.GetProperty(sourceProperty.Name);
            targetProperty.SetValue(target, sourceProperty.GetValue(source, null), null);
        }
        foreach (var sourceField in type.GetFields())
        {
            var targetField = type.GetField(sourceField.Name);
            targetField.SetValue(target, sourceField.GetValue(source));
        }
    }

    public static int ClampToArraySize(this int original, Array  array)
    {
        if (original <= 0)
            return 0;

        if (original >= array.Length)
            return array.Length - 1;

        return original;
    }
    public static int ClampToListSize(this int original, IList  list)
    {
        if (original <= 0)
            return 0;

        if (original >= list.Count)
            return list.Count - 1;

        return original;
    }
    
    public static Texture2D toTexture2D(this RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGBA32, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }
    
    private static System.Random random = new System.Random();
 
    public static T GetRandomElement<T>(this IEnumerable<T> list)
    {
        // If there are no elements in the collection, return the default value of T
        if (list.Count() == 0)
            return default(T);

        return list.ElementAt(random.Next(list.Count()));
    }
    
    public static void AddEventTrigger(this EventTrigger eventTrigger, UnityAction<BaseEventData> action,
        EventTriggerType triggerType)
    {
        EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent();
        trigger.AddListener(action);

        EventTrigger.Entry entry = new EventTrigger.Entry { callback = trigger, eventID = triggerType };
        eventTrigger.triggers.Add(entry);
    }
    
    // public static Vector2 GetSnapToPositionToBringChildIntoView(this ScrollRect scrollRect, RectTransform child)
    // {
    //     Canvas.ForceUpdateCanvases();
    //     Vector2 viewportLocalPosition = scrollRect.viewport.localPosition;
    //     Vector2 childLocalPosition   = child.localPosition;
    //     Vector2 result = new Vector2(
    //         0 - (viewportLocalPosition.x + childLocalPosition.x),
    //         0 - (viewportLocalPosition.y + childLocalPosition.y)
    //     );
    //     return result;
    // }
    //
    // public static void SnapToChild(this ScrollRect scrollRect, RectTransform child)
    // {
    //     Vector2 pos = scrollRect.GetSnapToPositionToBringChildIntoView(child);
    //     scrollRect.content.localPosition = pos;
    // }
}