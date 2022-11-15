using System.Collections.Generic;
using UnityEngine.Events;

namespace Pearl.Events
{
    public class TrackingUnityEvent<T0> : UnityEvent<T0>
    {
        private readonly List<UnityAction<T0>> unityActions = new();

        public List<UnityAction<T0>> UnityActions { get { return unityActions; } }

        public int GetNotPersistentEventCount()
        {
            return unityActions != null ? unityActions.Count : 0;
        }

        public bool IsThereListeners()
        {
            return GetPersistentEventCount() != 0 || GetNotPersistentEventCount() != 0;
        }

        public void AddNotPersistantListener(UnityAction<T0> call)
        {
            if (unityActions != null)
            {
                unityActions.Add(call);
                AddListener(call);
            }
        }

        public void AddInHeadNotPersistantListener(UnityAction<T0> call)
        {
            AddNotPersistantListener(0, call);
        }

        public void AddNotPersistantListener(int i, UnityAction<T0> call)
        {
            if (unityActions != null)
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

        public void RemoveNotPersistantListener(UnityAction<T0> call)
        {
            if (unityActions != null)
            {
                unityActions.Remove(call);
                RemoveListener(call);
            }
        }
    }
}