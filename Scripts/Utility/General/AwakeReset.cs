using UnityEngine;
using UnityEngine.Events;

namespace Pearl
{
    public class AwakeReset : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent initEvent = null;

        private void Awake()
        {
            initEvent?.Invoke();
        }

    }
}
