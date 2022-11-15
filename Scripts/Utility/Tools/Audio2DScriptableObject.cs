using UnityEngine;

namespace Pearl
{
    [CreateAssetMenu(fileName = "Audio2D", menuName = "Pearl/Audio/UI", order = 1)]

    public class Audio2DScriptableObject : ScriptableObject
    {
        [SerializeField]
        private StringAudioClipDictionary audioDictionary = null;

        public AudioClip GetClip(in string audioScìtring)
        {
            if (audioDictionary != null && audioDictionary.TryGetValue(audioScìtring, out AudioClip clip))
            {
                return clip;
            }
            return null;
        }
    }
}
