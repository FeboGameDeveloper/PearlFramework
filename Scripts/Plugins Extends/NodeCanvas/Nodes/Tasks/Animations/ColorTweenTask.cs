#if NODE_CANVAS

using Pearl.Tweens;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [ParadoxNotion.Design.Category("Pearl/Tweens")]
    public class ColorTweenTask : TweenTask<Color, Transform>
    {
        protected override TweenContainer CreateTween(in Transform container, in float timeOrVelocity)
        {
            SpriteManager spriteManager = new(container);
            if (spriteManager.IsActive && newValue != null)
            {
                return spriteManager.ColorTween(timeOrVelocity, true, functionEnum.value, changeMode.value, newValue.value);
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