#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl.NodeCanvas.Tasks.Conditions
{
    [Category("Pearl")]
    public class GameManagerOnlineCheck : ConditionTask
    {
        protected override bool OnCheck()
        {
            return GameManager.ThereIsOnline;
        }
    }
}

#endif