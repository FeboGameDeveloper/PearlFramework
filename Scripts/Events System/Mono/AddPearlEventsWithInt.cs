using UnityEngine;

namespace Pearl.Events
{
    public class AddPearlEventsWithInt : AddPearlEventsAbstract<int>
    {
        [SerializeField]
        private IntEvent unityEvent = null;

        protected override void Invoke(int parameter)
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke(parameter);
            }
        }
    }
}
