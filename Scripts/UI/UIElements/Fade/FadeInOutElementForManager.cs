using UnityEngine;

namespace Pearl.UI
{
    public class FadeInOutElementForManager : FadeInOutImages
    {
        [SerializeField]
        private PhaseFadeUIManager phaseFadeUIManager = null;

        protected override void Start()
        {
        }

        protected override void ChangeSprite()
        {
            if (phaseFadeUIManager && imageComponent && sprites != null)
            {
                imageComponent.sprite = phaseFadeUIManager.GetRandomSprite(imageComponent.sprite);
            }
        }

    }
}

