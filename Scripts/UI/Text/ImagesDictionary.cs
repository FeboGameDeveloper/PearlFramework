#if LOCALIZATION

using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public class ImagesDictionary : MonoBehaviour
    {
        [SerializeField]
        private Dictionary<string, Dictionary<SystemLanguage, Sprite>> spriteDictionary = null;


        public Sprite GetSprite(in string ID)
        {
            Sprite newSprite = null;
            if (spriteDictionary.TryGetValue(ID, out var languageDictionary))
            {
                languageDictionary.TryGetValue(LocalizationManager.Language, out newSprite);
            }

            return newSprite;
        }

    }
}

#endif
