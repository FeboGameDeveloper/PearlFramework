using UnityEngine;

namespace Pearl
{
    public class InteractEvents : MonoBehaviour
    {
        [SerializeField]
        private StringEventDictionary eventsDictionry = null;

        public void Interact(string comand)
        {
            if (eventsDictionry.IsNotNullAndTryGetValue(comand, out var events))
            {
                events.ev?.Invoke();
            }
        }
    }
}
