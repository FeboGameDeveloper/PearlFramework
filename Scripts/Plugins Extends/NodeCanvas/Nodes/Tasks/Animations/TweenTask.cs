#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Tweens;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    public abstract class TweenTask<Type, Container> : ActionTask<PearlFSMOwner> where Container : Component
    {
        public BBParameter<ComponentReference<Container>> container;
        public BBParameter<Type[]> newValue;
        public BBParameter<ChangeModes> changeMode = ChangeModes.Time;
        [Conditional("changeMode", (int)ChangeModes.Time)]
        public BBParameter<float> time;
        [Conditional("changeMode", (int)ChangeModes.Time)]
        public BBParameter<TimeType> timeType = TimeType.Scaled;
        [Conditional("changeMode", (int)ChangeModes.Velocity)]
        public BBParameter<float> velocity;
        public BBParameter<bool> loop;
        [Conditional("loop", 1)]
        public BBParameter<float> waitAtEndPath = 0f;
        public BBParameter<bool> inverse;
        public BBParameter<bool> pingPong;
        public BBParameter<float> curveFactor = 0;
        public BBParameter<TypeReferenceEnum> typeReference = TypeReferenceEnum.Absolute;
        public BBParameter<bool> useDefaultValue;
        [Conditional("useDefaultValue", 1)]
        public BBParameter<Type> defaultValue;
        public BBParameter<FunctionEnum> functionEnum;

        public BBParameter<bool> wait;
        public BBParameter<bool> inheritance;
        [Conditional("inheritance", 1)]
        public BBParameter<bool> onlyChildren = true;
        public BBParameter<bool> KillOnStop = false;
        public BBParameter<bool> saveTween;
        [Conditional("saveTween", 1)]
        public BBParameter<string> nameVarTween;


        private readonly List<TweenContainer> _tweens = new();
        private int _tweensFinishedCount;

        protected override void OnExecute()
        {
            if (changeMode == null || !container.IsExist(out var containerReference) || _tweens == null ||
                containerReference.Component == null)
            {
                EndAction();
                return;
            }


            _tweens.Clear();

            float timeOrVelocity = changeMode != null ? (changeMode.value == ChangeModes.Time && time != null ? time.value : (changeMode.value == ChangeModes.Velocity && velocity != null ? velocity.value : 0)) : 0;
            var onlyChildrnBool = onlyChildren == null || onlyChildren.value;
            List<Container> aux = inheritance != null && inheritance.value ? containerReference.Component.gameObject.GetChildrenInHierarchy<Container>(onlyChildrnBool) : new List<Container> { container.value.Component };


            foreach (Container element in aux)
            {
                TweenContainer tween = null;
                if (time != null && changeMode != null && functionEnum != null)
                {
                    tween = CreateTween(element, timeOrVelocity);
                }
                if (tween != null)
                {
                    _tweens.Add(tween);
                }
            }

            if (!_tweens.IsAlmostSpecificCount())
            {
                EndAction();
                return;
            }

            DecorateTweens();


            if (blackboard != null && nameVarTween != null && nameVarTween.value != null && saveTween != null && saveTween.value)
            {
                var tweensVar = blackboard.GetVariable<Dictionary<string, TweenContainer>>("tweens");
                if (tweensVar == null)
                {
                    blackboard.AddVariable("tweens", new Dictionary<string, TweenContainer>());
                    tweensVar = blackboard.GetVariable<Dictionary<string, TweenContainer>>("tweens");
                }

                if (tweensVar.value.IsNotNull(out var dict))
                {
                    for (int i = 0; i < _tweens.Count; i++)
                    {
                        dict.Update(nameVarTween.value + "" + i, _tweens[i]);
                    }
                }
            }

            if (wait != null && wait.value)
            {
                foreach (var tween in _tweens)
                {
                    tween.OnComplete += OnComplete;
                }
            }

            var type = timeType != null ? timeType.value : TimeType.Scaled;
            foreach (var tween in _tweens)
            {
                tween.Play(false, timeType.value);
            }


            if (wait == null || !wait.value)
            {
                Complete();
            }
        }

        private void DecorateTweens()
        {
            foreach (var tween in _tweens)
            {
                UseDefaultValue(tween);
            }

            if (inverse != null)
            {
                foreach (var tween in _tweens)
                {
                    tween.IsInverse = inverse.value;
                }
            }

            if (typeReference != null)
            {
                foreach (var tween in _tweens)
                {
                    tween.PathReference = typeReference.value;
                }
            }

            if (loop != null)
            {
                foreach (var tween in _tweens)
                {
                    tween.IsLoop = loop.value;
                    if (waitAtEndPath != null)
                    {
                        tween.WaitAtEndPath = waitAtEndPath.value;
                    }
                }
            }

            if (pingPong != null)
            {
                foreach (var tween in _tweens)
                {
                    tween.PingPong = pingPong.value;
                }
            }

            if (curveFactor != null)
            {
                foreach (var tween in _tweens)
                {
                    tween.CurveFactor = curveFactor.value;
                }
            }
        }
        private void Complete()
        {
            _tweensFinishedCount++;

            if (_tweensFinishedCount < _tweens.Count)
            {
                return;
            }

            _tweensFinishedCount = 0;
            if (blackboard != null && nameVarTween != null && nameVarTween.value != null && saveTween != null && saveTween.value)
            {
                var tweensVar = blackboard.GetVariable<Dictionary<string, TweenContainer>>("tweens");
                if (tweensVar != null && tweensVar.value.IsNotNull(out var dict))
                {
                    for (int i = 0; i < _tweens.Count; i++)
                    {
                        dict.Remove(nameVarTween.value + "" + i);
                    }
                }
            }

            EndAction();
        }

        private void OnComplete(TweenContainer tweenContainer, float error)
        {
            Complete();
        }

        protected override void OnStop()
        {
            if (KillOnStop != null && KillOnStop.value)
            {
                Kill();
            }
        }

        private void Kill()
        {
            if (_tweens != null)
            {
                foreach (var tween in _tweens)
                {
                    tween.ForceFinish();
                }
            }
        }

        protected abstract TweenContainer CreateTween(in Container container, in float timeOrVelocity);

        protected abstract void UseDefaultValue(in TweenContainer tween);
    }
}
#endif