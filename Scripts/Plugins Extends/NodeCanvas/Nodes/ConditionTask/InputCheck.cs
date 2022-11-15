#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Input;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class InputCheck : ConditionTask
    {
        [RequiredField]
        public BBParameter<string> inputString;
        public BBParameter<string> mapString = null;
        public BBParameter<int> player = 0;

        protected override string info { get { return "[" + inputString.ToString() + "]"; } }

        protected override void OnEnable()
        {
            int playerNum = player != null ? player.value : 0;
            if (inputString != null)
            {
                mapString = mapString != null && mapString.value == string.Empty ? null : mapString;
                InputManager.Get(playerNum)?.PerformedHandle(inputString.value, OnFinish, Pearl.ActionEvent.Add, StateButton.Down, mapString.value);
            }
        }

        protected override void OnDisable()
        {
            int playerNum = player != null ? player.value : 0;
            if (inputString != null)
            {
                InputManager.Get(playerNum)?.PerformedHandle(inputString.value, OnFinish, Pearl.ActionEvent.Remove);
            }
        }

        protected override bool OnCheck() { return false; }

        private void OnFinish()
        {
            YieldReturn(true);
        }
    }
}

#endif