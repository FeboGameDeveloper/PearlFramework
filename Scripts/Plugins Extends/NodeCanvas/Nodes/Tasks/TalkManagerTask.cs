#if NODE_CANVAS && INK

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Ink;
using System;
using System.Reflection;
using UnityEngine;

namespace Pearl
{
    [Category("Pearl")]
    public class TalkManagerTask : ActionTask<TalkManager>
    {
        public enum TalkAction { Talk, SetVar, ChangeDialog}

        public BBParameter<TalkAction> talkActionBB;

        [Conditional("talkActionBB", 1)]
        public BBParameter<string> varName;
        [Conditional("talkActionBB", 1)]
        public BBParameter<string> varValue;

        [Conditional("talkActionBB", 2)]
        public BBParameter<string> varDialog;

        protected override void OnExecute()
        {
            if (talkActionBB == null)
            {
                return;
            }

            if (talkActionBB.value == TalkAction.Talk)
            {
                agent.Talk();
            }
            else if(talkActionBB.value == TalkAction.SetVar && varName != null && varValue != null)
            {
                agent.SetVar(varName.value, varValue.value);
            }
            else if (talkActionBB.value == TalkAction.ChangeDialog && varDialog != null)
            {
                agent.ChangePath(varDialog.value);
            }

            EndAction();
        }
    }
}

#endif