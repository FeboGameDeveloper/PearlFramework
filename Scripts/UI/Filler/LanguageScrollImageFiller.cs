#if LOCALIZATION

using System.Collections.Generic;
using UnityEngine;

namespace Pearl.UI
{
    public class LanguageScrollImageFiller : Filler<ImageElementInfo>
    {
        protected override ImageElementInfo GetCurrentValue()
        {
            var dictionaryFlag = AssetManager.LoadAsset<FlagsScriptableObject>("Flags");

            if (dictionaryFlag)
            {
                SystemLanguage currentLanguage = LocalizationManager.Language;
                return new ImageElementInfo(dictionaryFlag.Get(currentLanguage), currentLanguage.ToString(), false);
            }

            return default;
        }

        protected override List<ImageElementInfo> Take()
        {
            var dictionaryFlag = AssetManager.LoadAsset<FlagsScriptableObject>("Flags");

            if (dictionaryFlag)
            {
                List<ImageElementInfo> LanguageSwitchInfoList = new();

                foreach (var language in LocalizationManager.GetCurrentLanguages())
                {
                    LanguageSwitchInfoList.Add(new ImageElementInfo(dictionaryFlag.Get(language), language.ToString(), false));
                }

                return LanguageSwitchInfoList;
            }

            return null;
        }
    }
}

#endif
