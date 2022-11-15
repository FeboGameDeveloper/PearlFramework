#if NODE_CANVAS

using Pearl.Tweens;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [ParadoxNotion.Design.Category("Pearl/Tweens")]
    public class FadeTweenTask : TweenTask<float, Transform>
    {
        protected override TweenContainer CreateTween(in Transform container, in float timeOrVelocity)
        {
            SpriteManager spriteManager = new SpriteManager(container);
            if (spriteManager.IsActive && newValue != null)
            {
                return spriteManager.FadeTween(timeOrVelocity, true, functionEnum.value, changeMode.value, newValue.value);
            }
            return null;
        }

        protected override void UseDefaultValue(in TweenContainer tween)
        {
            if (useDefaultValue != null && useDefaultValue.value && defaultValue != null && functionEnum != null)
            {

                Vector4 value = new Vector4(defaultValue.value, 0, 0, 0);
                tween.SetInitValue(value);
            }
        }
    }
}

#endif