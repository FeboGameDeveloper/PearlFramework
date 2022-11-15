using UnityEngine;

namespace Pearl.Tweens
{
    public class FadeTweenComponent : TweenComponent<float, Transform>
    {
        protected override void CreateTween()
        {
            SpriteManager spriteManager = new(container.Component);
            _tween = TweensExtend.FadeTween(spriteManager, _valueForTween, isAutoKill, functionEnum, mode, values);
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

        public override void SetValue(float type)
        {
            SpriteManager spriteManager = new(container.Component);
            if (spriteManager != null)
            {
                spriteManager.SetAlpha(type);
            }
        }
    }
}