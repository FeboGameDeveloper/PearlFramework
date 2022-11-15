using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    [CreateAssetMenu(fileName = "FlagsList", menuName = "Pearl/Language/Flags", order = 1)]

    public class FlagsScriptableObject : ScriptableObject
    {
        [SerializeField]
        private Dictionary<SystemLanguage, Sprite> flagsDictonary = null;

        public Sprite Get(SystemLanguage language)
        {
            if (flagsDictonary != null && flagsDictonary.TryGetValue(language, out Sprite flagSprite))
            {
                return flagSprite;
            }
            return null;
        }
    }
}
