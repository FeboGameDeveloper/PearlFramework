#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine.UI;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class NavigationUITask : ActionTask
    {
        [RequiredField]
        public BBParameter<Selectable> target = null;

        public BBParameter<bool> useUp;
        public BBParameter<bool> useDown;
        public BBParameter<bool> useLeft;
        public BBParameter<bool> useRight;

        [Conditional("useUp", 1)]
        public BBParameter<Selectable> selectableUp;
        [Conditional("useDown", 1)]
        public BBParameter<Selectable> selectableDown;
        [Conditional("useLeft", 1)]
        public BBParameter<Selectable> selectableLeft;
        [Conditional("useRight", 1)]
        public BBParameter<Selectable> selectableRight;

        protected override void OnExecute()
        {
            if (target != null && target.value.IsNotNull(out Selectable auxTarget))
            {
                Navigation navigation = auxTarget.navigation;

                if (useUp != null && useUp.value)
                {
                    navigation.selectOnUp = selectableUp != null ? selectableUp.value : null;
                }
                if (useDown != null && useDown.value)
                {
                    navigation.selectOnDown = selectableDown != null ? selectableDown.value : null;
                }
                if (useLeft != null && useLeft.value)
                {
                    navigation.selectOnLeft = selectableLeft != null ? selectableLeft.value : null;
                }
                if (useRight != null && useRight.value)
                {
                    navigation.selectOnRight = selectableRight != null ? selectableRight.value : null;
                }

                auxTarget.navigation = navigation;
            }

            EndAction();
        }
    }
}

#endif
