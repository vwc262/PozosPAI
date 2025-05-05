using UnityEngine;
using UnityEngine.Events;
using Raskulls.ScriptableSystem;


namespace Raskulls.ScriptableSystem
{
    public class SE_PlayerHitListener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_PlayerHit Event ;
        public UnityEventReborn PreResponse;
        public UnityEventReborn Response;
        public UnityEventReborn PostResponse;
        private void OnEnable()
        {
            Event.RegisterListener(this);
        }
        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }
        public void OnPreEventRaised(Vector3 HitPosition, SV_PlayerDamage Damage)
        {
            PreResponse.Invoke(HitPosition, Damage);
        }
        public void OnEventRaised(Vector3 HitPosition, SV_PlayerDamage Damage)
        {
            Response.Invoke(HitPosition, Damage);
        }
        public void OnPostEventRaised(Vector3 HitPosition, SV_PlayerDamage Damage)
        {
            PostResponse.Invoke(HitPosition, Damage);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<Vector3, SV_PlayerDamage> { }
    }
}
