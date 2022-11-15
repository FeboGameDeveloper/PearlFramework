#if STOMPYROBOT

using SRDebugger;
using UnityEngine;

namespace Pearl.UI
{
    public class ConsoleButtonWidget : MonoBehaviour
    {
        [SerializeField]
        private DefaultTabs t = DefaultTabs.Console;

        public void Open()
        {
            SRDebug.Instance.ShowDebugPanel(t);
        }
    }
}
#endif
