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
    [CreateAssetMenu(fileName = "SE_SelectSitio_Name", menuName = "Raskulls/Scriptable System/Events/SelectSitio Event")]
    public class SE_SelectSitio : GameEventBase
    {
        [FormerlySerializedAs("sitioGPS")] public ControlMarcadorSitio controlMarcadorSitio;
        private readonly List<SE_SelectSitioListener> eventListeners = new List<SE_SelectSitioListener>();
        public void Raise(ControlMarcadorSitio controlMarcadorSitio, OnEventComplete onEventComplete = null)
        {
            this.controlMarcadorSitio = controlMarcadorSitio;
            if(GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(controlMarcadorSitio, onEventComplete));
        }
        public void Raise(ControlMarcadorSitio controlMarcadorSitio)
        {
            this.controlMarcadorSitio = controlMarcadorSitio;
            if(GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(controlMarcadorSitio));
        }
        private IEnumerator RaiseEvent(ControlMarcadorSitio controlMarcadorSitio, OnEventComplete onEventComplete = null)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPreEventRaised(controlMarcadorSitio);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(controlMarcadorSitio);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPostEventRaised(controlMarcadorSitio);
            yield return null;
            if (onEventComplete != null) onEventComplete();
        }
        public override void RegisterListener(GameEventListenerBase listener)
        {
            if (!eventListeners.Contains((SE_SelectSitioListener)listener))
                eventListeners.Add((SE_SelectSitioListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase  listener)
        {
            if (eventListeners.Contains((SE_SelectSitioListener)listener))
                eventListeners.Remove((SE_SelectSitioListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<DataSitio>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<DataSitio>)unityEvent, (UnityAction<DataSitio>)myMethod);
#endif
        }
    }
}
