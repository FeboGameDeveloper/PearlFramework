#if LOCALIZATION
using UnityEngine;

namespace Pearl
{
    [CreateAssetMenu(fileName = "DictionaryText", menuName = "Pearl/Language/Text", order = 1)]
    public class DictionaryTextScriptableObject : ScriptableObject
    {
        [SerializeField]
        private StringSystemLanguageTextAssetDictionaryDictionary vocabolary = null;

        public TextAsset Get(string ID)
        {
            if (ID != null && vocabolary != null)
            {
                ID = ID.Trim();

                if (vocabolary.IsNotNullAndTryGetValue(ID, out var languageDictonary) && languageDictonary.TryGetValue(LocalizationManager.Language, out TextAsset textAsset))
                {
                    return textAsset;
                }
            }
return null;
        }
    }
}

#endif
