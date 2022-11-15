#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace Pearl
{
    [Category("Pearl")]
    public class PlayAudio2DTask : ActionTask
    {
        public BBParameter<AudioClip> clip = null;

        public BBParameter<AudioMixerGroup> mixerGroup = null;

        protected override void OnExecute()
        {
            if (clip.value != null && mixerGroup.value != null)
            {
                AudioManager.Play2DAudio(clip.value, mixerGroup.value);
            }
            EndAction();
        }
    }
}

#endif
