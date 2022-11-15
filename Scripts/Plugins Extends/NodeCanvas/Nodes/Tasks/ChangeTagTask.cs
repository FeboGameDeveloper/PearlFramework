#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Multitags;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class ChangeTagTask : ActionTask
    {
        public BBParameter<string[]> tagsToAdd;
        public BBParameter<string[]> tagsToRemove;

        [RequiredField]
        public BBParameter<MultitagsComponent> pearlObject;

        protected override void OnExecute()
        {
            if (pearlObject != null && pearlObject.value.IsNotNull(out MultitagsComponent obj))
            {
                string[] tags;
                if (tagsToAdd != null && tagsToAdd.value.IsNotNull(out tags))
                {
                    foreach (string tag in tags)
                    {
                        obj.AddTags(tag);
                    }
                }

                if (tagsToRemove != null && tagsToRemove.value.IsNotNull(out tags))
                {
                    obj.RemoveTags(tags);
                }
            }

            EndAction();
        }
    }
}

#endif
