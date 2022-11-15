#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.UI;
using UnityEngine;

namespace Pearl
{
    [Category("Pearl")]
    public class SetFocusTask : ActionTask
    {
        public BBParameter<GameObject> focusableObj;

        protected override void OnExecute()
        {
            if (focusableObj != null)
            {
                FocusManager.SetFocus(focusableObj.value, true);
            }
            EndAction();
        }
    }
}

#endif
