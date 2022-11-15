using UnityEngine;

namespace Pearl.Events
{
    public class AddPearlEventsWithString : AddPearlEventsAbstract<string>
    {
        [SerializeField]
        private StringEvent unityEvent = null;

        protected override void Invoke(string parameter)
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke(parameter);
            }
        }
    }
}
