using UnityEngine;

namespace Pearl.Events
{
    public class AddPearlEventsWithGameobject : AddPearlEventsAbstract<GameObject>
    {
        [SerializeField]
        private GameObjectEvent unityEvent = null;

        protected override void Invoke(GameObject parameter)
        {
            if (unityEvent != null)
            {
                unityEvent.Invoke(parameter);
            }
        }
    }
}
