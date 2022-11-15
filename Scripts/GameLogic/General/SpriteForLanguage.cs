#if LOCALIZATION

using Pearl.Events;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public class SpriteForLanguage : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<SystemLanguage, Sprite> dictionary = null;
        [SerializeField]
        private GameObject spriteContainer = null;

        private SpriteManager _spriteManager;

        protected void Awake()
        {
            _spriteManager = new SpriteManager(spriteContainer);
        }

        protected void Start()
        {
            PearlEventsManager.AddAction(ConstantStrings.SetNewLanguageEvent, SetSprite);
            SetSprite();
        }

        protected void OnDestroy()
        {
            PearlEventsManager.RemoveAction(ConstantStrings.SetNewLanguageEvent, SetSprite);
        }

        private void SetSprite()
        {
            if (dictionary != null && _spriteManager != null)
            {
                dictionary.TryGetValue(LocalizationManager.Language, out Sprite correctSprite);
                _spriteManager.SetSprite(correctSprite);
            }
        }
    }
}

#endif
