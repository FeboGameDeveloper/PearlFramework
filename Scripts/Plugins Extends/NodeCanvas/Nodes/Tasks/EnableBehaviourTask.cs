#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using System;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Serializable]
    public struct ComponentInfo
    {
        public Component behaviour;
    }


    [Category("Pearl")]
    public class EnableBehaviourTask<T> : ActionTask<Transform> where T : Behaviour
    {
        public BBParameter<ComponentReference<T>> container;


        public BBParameter<bool> enable;

        protected override void OnExecute()
        {
            if (enable == null || container == null)
            {
                EndAction();
            }


            T component = container.value.Component;

            if (component != null)
            {
                component.enabled = enable.value;
            }

            //ReflectionExtend.SetProperty(component.value, "enabled", enable.value);
            EndAction();
        }
    }
}

#endif