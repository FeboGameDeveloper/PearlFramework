#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Input;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ChangeInputMapTask : ActionTask
    {
        #region Inspector fields
        [RequiredField]
        public BBParameter<string> newMap;

        public BBParameter<bool> UIEnable = true;
        #endregion

        #region Unity CallBacks
        protected override void OnExecute()
        {
            var UIEnableValue = UIEnable != null ? UIEnable.value : true;
            if (newMap != null)
            {
                InputManager.Input?.SetSwitchMap(newMap.value, UIEnableValue);
            }
            EndAction();
        }
        #endregion
    }
}

#endif
