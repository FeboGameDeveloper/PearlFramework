using UnityEngine;

namespace Pearl.Tweens
{
    public class ScaleTweenComponent : TweenComponent<Vector3, Transform>
    {
        protected override void CreateTween()
        {
            _tween = TweensExtend.ScaleTween(container.Component, _valueForTween, isAutoKill, functionEnum, mode, values);
        }

        protected void Reset()
        {
            if (container != null)
            {
                container.Component = transform;
            }
        }

        public override void Play(bool setInit = false)
        {
            if (_tween != null)
            {
                _tween.FinalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(values);
            }

            base.Play(setInit);
        }

        public override void SetValue(Vector3 type)
        {
            if (transform != null)
            {
                transform.localScale = type;
            }
        }
    }
}