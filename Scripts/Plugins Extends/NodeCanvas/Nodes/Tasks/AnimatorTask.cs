#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class AnimatorTask : ActionTask
    {
        [RequiredField]
        public BBParameter<Animator> animator;
        [RequiredField]
        public BBParameter<string> nameVar;
        public BBParameter<bool> waitAnimation;

        public BBParameter<bool> useFloat;
        public BBParameter<bool> useBool;
        public BBParameter<bool> useInt;
        public BBParameter<bool> useTrigger;

        [Conditional("useFloat", 1)]
        public BBParameter<float> valueFloat;
        [Conditional("useBool", 1)]
        public BBParameter<bool> valueBool;
        [Conditional("useInt", 1)]
        public BBParameter<int> valueInt;

        private float _timeForAnim;
        private float _auxTime;

        protected override void OnExecute()
        {
            if (animator.IsExist(out var anim) && nameVar != null)
            {
                if (useFloat != null && useFloat.value && valueFloat != null)
                {
                    anim.SetFloat(nameVar.value, valueFloat.value);
                }
                else if (useBool != null && useBool.value && valueBool != null)
                {
                    anim.SetBool(nameVar.value, valueBool.value);
                }
                if (useInt != null && useInt.value && useInt != null)
                {
                    anim.SetInteger(nameVar.value, valueInt.value);
                }
                if (useTrigger != null && useTrigger.value)
                {
                    anim.SetTrigger(nameVar.value);
                }

                if (waitAnimation != null && waitAnimation.value)
                {

                    //TO DO
                    _auxTime = 0;
                    var currentState = anim.GetCurrentAnimatorStateInfo(0);
                    _timeForAnim = currentState.length * currentState.speed;
                }
                else
                {
                    EndAction();
                }
            }
        }

        protected override void OnUpdate()
        {
            if (waitAnimation != null && waitAnimation.value)
            {
                _auxTime += Time.deltaTime;
                if (_auxTime >= _timeForAnim)
                {
                    EndAction();
                }
            }
        }
    }
}

#endif