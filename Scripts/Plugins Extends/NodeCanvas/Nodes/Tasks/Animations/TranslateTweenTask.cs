#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Tweens;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [ParadoxNotion.Design.Category("Pearl/Tweens")]
    public class TranslateTweenTask : TweenTask<Vector3, Transform>
    {
        public BBParameter<SpaceTranslation> spaceTanslation = SpaceTranslation.WorldSpace;
        public BBParameter<TypeTranslation> typeTranslation = TypeTranslation.Transform;

        public BBParameter<bool> useTransformDestination = false;
        [Conditional("useTransformDestination", 1)]
        public BBParameter<Transform> destination = null;

        protected override TweenContainer CreateTween(in Transform container, in float timeOrVelocity)
        {
            SpaceTranslation space = spaceTanslation != null ? spaceTanslation.value : SpaceTranslation.WorldSpace;
            TypeTranslation type = typeTranslation != null ? typeTranslation.value : TypeTranslation.Transform;
            bool useTarget = useTransformDestination != null && useTransformDestination.value && destination != null && destination.value != null && curveFactor != null;

            if (container != null && (useTarget || newValue != null))
            {
                Vector3[] aux = useTarget ? ArrayExtend.CreateArray(destination.value.position) : newValue.value;
                var auxTween = container.TranslateTween(timeOrVelocity, true, AxisCombined.XYZ, space, functionEnum.value, changeMode.value, curveFactor.value, type, aux);
                curveFactor = auxTween.CurveFactor;
                return auxTween;
            }
            return null;
        }

        protected override void UseDefaultValue(in TweenContainer tween)
        {
            if (useDefaultValue != null && useDefaultValue.value && defaultValue != null && functionEnum != null)
            {
                tween.SetInitValue(defaultValue.value);
            }
        }
    }
}
#endif