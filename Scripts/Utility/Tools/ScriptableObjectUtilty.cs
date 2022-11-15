using UnityEngine;

namespace Pearl
{
    //classe di utilit� ch recuper� degli scriptable object
    public class ScriptableObjectUtilty
    {
        public static Sprite GetFlags(SystemLanguage language)
        {
            var flags = AssetManager.LoadAsset<FlagsScriptableObject>("FlagsList");
            if (flags)
            {
                return flags.Get(language);
            }

            return null;
        }
    }
}