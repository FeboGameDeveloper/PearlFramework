#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl
{
    [Category("Pearl")]
    public class TeleporterTask : ActionTask
    {
        public BBParameter<Transform> owner;
        public BBParameter<Transform> destination;

        protected override void OnExecute()
        {
            if (owner != null && destination != null)
            {
                var ownerTr = owner.value;
                var destinationTr = destination.value;
                if (ownerTr && destinationTr)
                {
                    ownerTr.position = destinationTr.position;
                }
            }
            EndAction();
        }
    }
}

#endif
