#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Storage;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class StorageTask : ActionTask
    {
        public enum StorageEnumTask { SaveAll = 0, LoadAll, RemoveAll, ChangeSlot }

        public BBParameter<StorageEnumTask> storageEnumBB = default;
        [Conditional("storageEnumBB", 2)]
        public BBParameter<StorageTypeEnum> storageTypeEnumBB = default;
        [Conditional("storageEnumBB", 3)]
        public BBParameter<int> intSlotBB = default;

        protected override void OnExecute()
        {
            if (storageEnumBB.IsExist(out var storageEnum))
            {
                if (storageEnum == StorageEnumTask.LoadAll)
                {
                    StorageManager.LoadAll();
                }
                else if (storageEnum == StorageEnumTask.SaveAll)
                {
                    StorageManager.SaveAll();
                }
                else if (storageEnum == StorageEnumTask.ChangeSlot)
                {
                    if (intSlotBB != null)
                    {
                        StorageManager.ChangeSlot(intSlotBB.value);
                    }
                }
                else
                {
                    if (storageTypeEnumBB != null)
                    {
                        StorageManager.RemoveAll(storageTypeEnumBB.value);
                    }
                }
            }

            EndAction();
        }
    }
}

#endif