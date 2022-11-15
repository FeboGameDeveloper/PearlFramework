using UnityEngine;

namespace Pearl.Audio
{
    public enum UIAudioStateEnum { OnClick, OnScroll, OnBack }

    public static class AudioUI
    {
        private readonly static Audio2DScriptableObject audioScriptablObject;

        static AudioUI()
        {
            audioScriptablObject = AssetManager.LoadAsset<Audio2DScriptableObject>("Audio2D");
        }


        public static void PlayAudioSound(UIAudioStateEnum state, bool usePause = true)
        {
            if (audioScriptablObject)
            {
                AudioManager.Play2DAudio(Get(state), AudioManager.UIMixer, usePause);
            }
        }

        public static AudioClip Get(UIAudioStateEnum state)
        {
            if (audioScriptablObject)
            {
                return audioScriptablObject.GetClip(state.ToString());
            }
            return null;
        }

    }
}
