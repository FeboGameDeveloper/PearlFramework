#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Storage;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class StorageOptionsTask : ActionTask
    {
        protected override void OnExecute()
        {
            StoragePlayerPrefs.SaveAll();
            EndAction();
        }
    }
}

#endif


