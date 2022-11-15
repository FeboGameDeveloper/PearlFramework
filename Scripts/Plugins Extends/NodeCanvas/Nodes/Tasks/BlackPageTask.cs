#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class BlackPageTask : ActionTask
    {
        public enum BlackPageEnum { Appear, Disappear }

        public BBParameter<BlackPageEnum> blackPageEnum;
        public BBParameter<float> timeForChange;
        public BBParameter<TypeSibilling> sibilling = TypeSibilling.Last;
        [Conditional("sibilling", (int)TypeSibilling.SpecificIndex)]
        public BBParameter<int> positionChild = 0;

        protected override void OnExecute()
        {
            if (blackPageEnum != null && timeForChange != null && sibilling != null && positionChild != null)
            {
                if (blackPageEnum.value == BlackPageEnum.Appear)
                {
                    BlackPageManager.AppearBlackPage(timeForChange.value, sibilling.value, positionChild.value);
                }
                else
                {
                    BlackPageManager.DisappearBlackPage(timeForChange.value, sibilling.value, positionChild.value);
                }
            }

            EndAction();
        }
    }
}
#endif
