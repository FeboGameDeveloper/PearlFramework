#if LOCALIZATION

using Pearl.Events;
using UnityEngine;

namespace Pearl
{
    public class ImageManager : MonoBehaviour
    {
        [SerializeField]
        private bool isLocalize = true;
        [SerializeField]
        private bool preservedAspect = true;
        [SerializeField]
        private string idSprite = string.Empty;

        [SerializeField]
        private ImagesDictionary dictionary;

        private SpriteManager _spriteManager;
        private Sprite _currentSprite;

        private void Awake()
        {
            _spriteManager = new SpriteManager(gameObject);
            if (_spriteManager != null)
            {
                _spriteManager.SetPreserveAspect(preservedAspect);
            }
        }

        private void Start()
        {
            if (isLocalize)
            {
                PearlEventsManager.AddAction(ConstantStrings.SetNewLanguageEvent, SetSprite);
                if (idSprite != string.Empty)
                {
                    SetDictionary();
                    SetSprite();
                }
            }
        }

        private void OnDestroy()
        {
            if (isLocalize)
            {
                PearlEventsManager.RemoveAction(ConstantStrings.SetNewLanguageEvent, SetSprite);
            }
        }


        public void SetSprite(in Sprite sprite)
        {
            _currentSprite = sprite;
            SetSprite();
        }

        public void SetSprite(string id)
        {
            idSprite = id;
            SetSprite();
        }

        public void SetSprite()
        {
            _currentSprite = isLocalize ? GetSpriteLocalized() : _currentSprite;

            if (_spriteManager != null)
            {
                _spriteManager.SetSprite(_currentSprite);
            }
        }

        public void SetDictionary()
        {
            dictionary = GetComponentInParent<ImagesDictionary>();
        }

        private Sprite GetSpriteLocalized()
        {
            return dictionary != null ? dictionary.GetSprite(idSprite) : null;
        }
    }
}

#endif
