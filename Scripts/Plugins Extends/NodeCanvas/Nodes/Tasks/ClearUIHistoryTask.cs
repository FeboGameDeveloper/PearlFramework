#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.UI;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ClearUIHistoryTask : ActionTask
    {
        protected override void OnExecute()
        {
            FocusManager.ClearAll();
            EndAction();
        }
    }
}

#endif