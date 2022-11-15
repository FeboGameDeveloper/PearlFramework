#if NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pearl.Audio;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class SoundManagerTask : ActionTask
    {
        public enum SoundManagerTaskEnum { SetClip, ChangePitch, StopMusic }


        public BBParameter<bool> isMusicBB;
        [Conditional("isMusicBB", 0)]
        public BBParameter<AudioSource> audioSourceBB;

        public BBParameter<SoundManagerTaskEnum> actionBB;

        [Conditional("actionBB", (int)SoundManagerTaskEnum.SetClip)]
        public BBParameter<ComplexAudioClip> audioClipBB;
        [Conditional("actionBB", (int)SoundManagerTaskEnum.ChangePitch)]
        public BBParameter<float> newPitchBB;

        protected override void OnExecute()
        {
            ComplexAudioClip complexClip = audioClipBB.value;

            if (isMusicBB.value)
            {
                if (actionBB.value == SoundManagerTaskEnum.ChangePitch)
                {
                    AudioManager.ChangePitchMusic(newPitchBB.value);
                }
                else if (actionBB.value == SoundManagerTaskEnum.SetClip)
                {
                    AudioManager.PlayMusic(complexClip);
                }
                else
                {
                    AudioManager.StopMusic();
                }
            }
            else if (audioSourceBB.value != null)
            {
                AudioSource aux = audioSourceBB.value;

                if (actionBB.value == SoundManagerTaskEnum.ChangePitch)
                {
                    aux.pitch = newPitchBB.value;
                }
                else if (actionBB.value == SoundManagerTaskEnum.SetClip)
                {
                    if (complexClip.clips.IsAlmostSpecificCount())
                    {
                        aux.clip = complexClip.clips[0];
                    }
                    aux.Play();
                }
                else
                {
                    aux.Stop();
                }
            }
            EndAction();
        }
    }
}

#endif
