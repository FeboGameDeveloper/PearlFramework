using UnityEngine;

namespace Pearl.Tweens
{
    public class TranslateTweenComponent : TweenComponent<Vector3, Transform>
    {
        [SerializeField]
        private SpaceTranslation space = SpaceTranslation.WorldSpace;
        [SerializeField]
        private TypeTranslation typeTranslation = TypeTranslation.Transform;
        [SerializeField]
        private bool useTransformDestination = false;
        [SerializeField]
        [ConditionalField("@useTransformDestination")]
        private Transform destination = null;

        protected override void CreateTween()
        {
            Vector3[] newValues = useTransformDestination && destination != null ? ArrayExtend.CreateArray(destination.position) : values;
            _tween = TweensExtend.TranslateTween(container.Component, _valueForTween, isAutoKill, functionEnum, space, mode, curveFactor, typeTranslation, newValues);
            curveFactor = _tween.CurveFactor;
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
                Vector3[] newValues = useTransformDestination && destination != null ? ArrayExtend.CreateArray(destination.position) : values;
                _tween.FinalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(newValues);
            }

            base.Play(setInit);
        }

        public override void SetValue(Vector3 type)
        {
            if (transform != null)
            {
                transform.position = type;
            }
        }
    }
}