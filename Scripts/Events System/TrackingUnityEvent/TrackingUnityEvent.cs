using System.Collections.Generic;
using UnityEngine.Events;

namespace Pearl.Events
{
    public class TrackingUnityEvent : UnityEvent
    {
        private readonly List<UnityAction> unityActions = new List<UnityAction>();

        public List<UnityAction> UnityActions { get { return unityActions; } }

        public int GetNotPersistentEventCount()
        {
            return unityActions != null ? unityActions.Count : 0;
        }

        public bool IsThereListeners()
        {
            return GetPersistentEventCount() != 0 || GetNotPersistentEventCount() != 0;
        }

        public void AddNotPersistantListener(UnityAction call)
        {
            if (unityActions != null)
            {
                unityActions.Add(call);
                AddListener(call);
            }
        }

        public void AddInHeadNotPersistantListener(UnityAction call)
        {
            AddNotPersistantListener(0, call);
        }

        public void AddNotPersistantListener(int i, UnityAction call)
        {
            if (unityActions != null && call != null)
            {
                unityActions.ComplexInsert(i, call);
                RemoveAllListeners();

                foreach (var action in unityActions)
                {
                    AddListener(action);
                }
            }
        }

        public void RemoveAllNotPersistantListener()
        {
            if (unityActions != null)
            {
                unityActions.Clear();
                RemoveAllListeners();
            }
        }

        public void RemoveNotPersistantListener(UnityAction call)
        {
            if (unityActions != null)
            {
                unityActions.Remove(call);
                RemoveListener(call);
            }
        }
    }
}
