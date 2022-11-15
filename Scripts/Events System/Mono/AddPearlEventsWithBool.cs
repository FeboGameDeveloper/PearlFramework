using UnityEngine;

namespace Pearl.Events
{
    public class AddPearlEventsWithBool : AddPearlEventsAbstract<bool>
    {
        [SerializeField]
        private BoolEvent unityEvent = null;

        protected override void Invoke(bool parameter)
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke(parameter);
            }
        }
    }
}