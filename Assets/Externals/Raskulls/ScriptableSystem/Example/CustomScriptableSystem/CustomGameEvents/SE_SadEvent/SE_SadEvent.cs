using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine.Events;
using System.Reflection;
using System.Collections;


namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "SE_SadEvent_Name", menuName = "Raskulls/Scriptable System/Events/SadEvent Event")]
    public class SE_SadEvent : GameEventBase
    {
        private readonly List<SE_SadEventListener> eventListeners = new List<SE_SadEventListener>();
        public void Raise(OnEventComplete onEventComplete = null)
        {
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(onEventComplete));
        }
        public void Raise()
        {
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent());
        }
        private IEnumerator RaiseEvent(OnEventComplete onEventComplete = null)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPreEventRaised();
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised();
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPostEventRaised();
            yield return null;
            if (onEventComplete != null) onEventComplete();
        }
        public override void RegisterListener(GameEventListenerBase listener)
        {
            if (!eventListeners.Contains((SE_SadEventListener)listener))
                eventListeners.Add((SE_SadEventListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase  listener)
        {
            if (eventListeners.Contains((SE_SadEventListener)listener))
                eventListeners.Remove((SE_SadEventListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent)unityEvent, (UnityAction)myMethod);
#endif
        }
    }
}
