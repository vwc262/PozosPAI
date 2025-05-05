using UnityEngine;
using UnityEngine.Events;


namespace Raskulls.ScriptableSystem
{
    public class SE_SelectSitioListener: GameEventListenerBase
    {
        [Tooltip("Event to register with.")]
        public SE_SelectSitio Event ;
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
        public void OnPreEventRaised(SitioGPS sitioGPS)
        {
            PreResponse.Invoke(sitioGPS);
        }
        public void OnEventRaised(SitioGPS sitioGPS)
        {
            Response.Invoke(sitioGPS);
        }
        public void OnPostEventRaised(SitioGPS sitioGPS)
        {
            PostResponse.Invoke(sitioGPS);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<SitioGPS> { }
    }
}
