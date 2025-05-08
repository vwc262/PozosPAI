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
        public void OnPreEventRaised(ControlMarcadorSitio controlMarcadorSitio)
        {
            PreResponse.Invoke(controlMarcadorSitio);
        }
        public void OnEventRaised(ControlMarcadorSitio controlMarcadorSitio)
        {
            Response.Invoke(controlMarcadorSitio);
        }
        public void OnPostEventRaised(ControlMarcadorSitio controlMarcadorSitio)
        {
            PostResponse.Invoke(controlMarcadorSitio);
        }
        [System.Serializable] public class UnityEventReborn : UnityEvent<ControlMarcadorSitio> { }
    }
}
