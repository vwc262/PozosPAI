using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_IsUserOverUIListener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_IsUserOverUI Event ;
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
        public void OnPreEventRaised(bool isUserOverUI)
        {
            PreResponse.Invoke(isUserOverUI);
        }
        public void OnEventRaised(bool isUserOverUI)
        {
            Response.Invoke(isUserOverUI);
        }
        public void OnPostEventRaised(bool isUserOverUI)
        {
            PostResponse.Invoke(isUserOverUI);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<bool> { }
    }
}
