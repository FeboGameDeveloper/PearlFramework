#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl
{
    [Category("Pearl")]
    public class QuitTask : ActionTask
    {
        protected override void OnExecute()
        {
            GameManager.Quit();
            EndAction();
        }
    }
}

#endif