#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace Pearl
{
    [Category("Pearl")]
    public class ChangeSpriteTask : ActionTask
    {
        public BBParameter<Sprite> newSprite = null;
        [RequiredField]
        public BBParameter<GameObject> spriteContainer;

        protected override void OnExecute()
        {
            if (spriteContainer != null && newSprite != null)
            {
                SpriteManager spriteManager = new SpriteManager(spriteContainer.value);
                if (spriteManager != null)
                {
                    spriteManager.SetSprite(newSprite.value);
                }
            }
            EndAction();
        }
    }
}

#endif
