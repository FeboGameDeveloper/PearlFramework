using UnityEngine;

namespace Pearl.Events
{
    public class AddPearlEventsWithFloat : AddPearlEventsAbstract<float>
    {
        [SerializeField]
        private FloatEvent unityEvent = null;

        protected override void Invoke(float parameter)
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke(parameter);
            }
        }
    }

}