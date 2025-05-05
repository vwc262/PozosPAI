using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine.Events;
using System.Reflection;
using System.Collections;

using Raskulls.ScriptableSystem;

namespace Raskulls.ScriptableSystem
{
    [CreateAssetMenu(fileName = "SE_PlayerHit_Name", menuName = "Raskulls/Scriptable System/Events/PlayerHit Event")]
    public class SE_PlayerHit : GameEventBase
    {
        public Vector3 HitPosition;
        public SV_PlayerDamage Damage;
        private readonly List<SE_PlayerHitListener> eventListeners = new List<SE_PlayerHitListener>();
        public void Raise(Vector3 HitPosition, SV_PlayerDamage Damage, OnEventComplete onEventComplete = null)
        {
            this.HitPosition = HitPosition;
            this.Damage = Damage;
            if(GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(HitPosition, Damage, onEventComplete));
        }
        public void Raise(Vector3 HitPosition, SV_PlayerDamage Damage)
        {
            this.HitPosition = HitPosition;
            this.Damage = Damage;
            if (GameEventCoroutineStarter.instance) GameEventCoroutineStarter.instance.StartCoroutine(RaiseEvent(HitPosition, Damage));
        }
        private IEnumerator RaiseEvent(Vector3 HitPosition, SV_PlayerDamage Damage, OnEventComplete onEventComplete = null)
        {
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPreEventRaised(HitPosition,Damage);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnEventRaised(HitPosition, Damage);
            yield return null;
            for (int i = eventListeners.Count - 1; i >= 0; i--)
                eventListeners[i].OnPostEventRaised(HitPosition, Damage);
            yield return null;
            if (onEventComplete != null) onEventComplete();
        }
        public override void RegisterListener(GameEventListenerBase listener)
        {
            if (!eventListeners.Contains((SE_PlayerHitListener)listener))
                eventListeners.Add((SE_PlayerHitListener)listener);
        }
        public override void UnregisterListener(GameEventListenerBase listener)
        {
            if (eventListeners.Contains((SE_PlayerHitListener)listener))
                eventListeners.Remove((SE_PlayerHitListener)listener);
        }
        public override void CreatePersistentListener(System.Type scriptType, MethodInfo methodInfo, Component myScript, object unityEvent)
        {
#if UNITY_EDITOR
            var myMethod = methodInfo.CreateDelegate(typeof(UnityAction<Vector3, SV_PlayerDamage>), myScript);
            UnityEventTools.AddPersistentListener((UnityEvent<Vector3, SV_PlayerDamage>)unityEvent, (UnityAction<Vector3, SV_PlayerDamage>)myMethod);
#endif
        }
    }
}
