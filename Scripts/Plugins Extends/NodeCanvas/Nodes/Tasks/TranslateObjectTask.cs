#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace Pearl
{
    [Category("Pearl")]
    public class TranslatObjectTask : ActionTask
    {
        [RequiredField]
        public BBParameter<TranslateObject> translateObjectScript;

        protected override void OnExecute()
        {
            if (translateObjectScript != null && translateObjectScript.value)
            {
                translateObjectScript.value.Play();
            }
            EndAction();
        }
    }
}

#endif
