using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine.Events;
using System.Reflection;
using System.Collections;
using UnityEngine.Serialization;


namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "SE_IsUserOverUI_Name", menuName = "Raskulls/Scriptable System/Events/IsUserOverUI Event")]
    public class SE_IsUserOverUI : GameEventBase
    {
        public bool isUserOverUI;
        private readonly List<SE_IsUserOverUIListener> eventListeners = new List<SE_IsUserOverUIListener>();
        public void Raise(bool isUserOverUI, OnEventComplete onEventComplete = null)
        {
            this.isUserOverUI = isUserOverUI;
            if(GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(isUserOverUI, onEventComplete));
        }
        public void Raise(bool isUserOverUI)
        {
            this.isUserOverUI = isUserOverUI;
            if(GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(isUserOverUI));
        }
        private IEnumerator RaiseEvent(bool isUserOverUI, OnEventComplete onEventComplete = null)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPreEventRaised(isUserOverUI);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(isUserOverUI);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPostEventRaised(isUserOverUI);
            yield return null;
            if (onEventComplete != null) onEventComplete();
        }
        public override void RegisterListener(GameEventListenerBase listener)
        {
            if (!eventListeners.Contains((SE_IsUserOverUIListener)listener))
                eventListeners.Add((SE_IsUserOverUIListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase  listener)
        {
            if (eventListeners.Contains((SE_IsUserOverUIListener)listener))
                eventListeners.Remove((SE_IsUserOverUIListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<bool>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<bool>)unityEvent, (UnityAction<bool>)myMethod);
#endif
        }
    }
}
