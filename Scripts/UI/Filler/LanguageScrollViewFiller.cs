#if LOCALIZATION

using System.Collections.Generic;
using UnityEngine;

namespace Pearl.UI
{
    public class LanguageScrollViewFiller : Filler<string>
    {
        protected override string GetCurrentValue()
        {
            return LocalizationManager.Language.ToString();
        }

        protected override List<string> Take()
        {
            return EnumExtend.ConvertInListString<SystemLanguage>(LocalizationManager.GetCurrentLanguages());
        }
    }
}

#endif
