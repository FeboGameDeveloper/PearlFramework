using Pearl.Events;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pearl
{
    [Serializable]
    public struct TriggerEventInfo
    {
        public string ev;
        public UnityEvent unityEvent;
    }


    public class TriggerFromEvents : MonoBehaviour
    {
        [SerializeField]
        private TriggerEventInfo[] triggerInfos = null;

        private Dictionary<string, UnityEvent> _dictionaryEvent;
        private Dictionary<string, Action> _dictionaryAction;

        private void Awake()
        {
            _dictionaryEvent = new Dictionary<string, UnityEvent>();
            _dictionaryAction = new Dictionary<string, Action>();
        }

        private void Start()
        {
            foreach (var info in triggerInfos)
            {
                Action action = () =>
                {
                    if (_dictionaryEvent.TryGetValue(info.ev, out UnityEvent unityEvent))
                    {
                        unityEvent?.Invoke();
                    }
                };
                _dictionaryAction.Add(info.ev, action);
                _dictionaryEvent.Add(info.ev, info.unityEvent);
                PearlEventsManager.AddAction(info.ev, action);
            }

        }

        private void OnDestroy()
        {
            foreach (var info in triggerInfos)
            {
                if (_dictionaryAction.TryGetValue(info.ev, out Action action))
                {
                    PearlEventsManager.RemoveAction(info.ev, action);
                }
            }
        }
    }
}
