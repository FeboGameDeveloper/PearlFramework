using Pearl.Pools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Pearl.Audio
{
    public class AudioTmpManager : MonoBehaviour
    {
        [SerializeField]
        private uint maxNumberAudio = 3;

        [SerializeField]
        private GameObject audio2DPrefab = null;
        [SerializeField]
        private GameObject audioPrefab = null;

        private List<AudioSourceManager> _audioSources2D = new List<AudioSourceManager>();
        private List<AudioSourceManager> _audioSources = new List<AudioSourceManager>();


        // Start is called before the first frame update
        protected void Awake()
        {
            PoolManager.InstantiatePool(audio2DPrefab, true, maxNumberAudio, true);
            PoolManager.InstantiatePool(audioPrefab, true, maxNumberAudio, true);
        }

        public AudioSourceManager Play(in AudioClip clip, in AudioMixerGroup mixerGroup, in Vector3 position, bool usePause = true)
        {
            return Play(new ComplexAudioClip(clip), mixerGroup, _audioSources, audioPrefab, position, usePause);
        }

        public AudioSourceManager Play(in ComplexAudioClip clip, in AudioMixerGroup mixerGroup, in Vector3 position, bool usePause = true)
        {
            return Play(clip, mixerGroup, _audioSources, audioPrefab, position, usePause);
        }

        public AudioSourceManager Play2D(in AudioClip clip, in AudioMixerGroup mixerGroup, bool usePause = true)
        {
            return Play(new ComplexAudioClip(clip), mixerGroup, _audioSources2D, audio2DPrefab, Vector3.zero, usePause);
        }

        public AudioSourceManager Play2D(in ComplexAudioClip clip, in AudioMixerGroup mixerGroup, bool usePause = true)
        {
            return Play(clip, mixerGroup, _audioSources2D, audio2DPrefab, Vector3.zero, usePause);
        }

        private AudioSourceManager Play(in ComplexAudioClip clip, in AudioMixerGroup mixerGroup, in List<AudioSourceManager> listSource, in GameObject sourcePrefab, in Vector3 position, bool usePause = true)
        {
            if (listSource == null)
            {
                return null;
            }

            AudioSourceManager audioSource = null;

            foreach (AudioSourceManager source in listSource)
            {
                if (source && !source.IsWorking())
                {
                    audioSource = source;
                    break;
                }
            }

            if (audioSource == null)
            {
                PoolManager.Instantiate<AudioSourceManager>(out audioSource, sourcePrefab, position, transform);
                if (audioSource)
                {
                    listSource.Add(audioSource);
                }
            }

            if (audioSource == null)
            {
                audioSource = RandomExtend.GetRandomElement(listSource);
            }

            if (audioSource != null)
            {
                audioSource.StopSoundInPause = usePause;
                audioSource.Stop();
                audioSource.OutputAudioMixerGroup = mixerGroup;
                audioSource.SetClip(clip, true);
            }

            return audioSource;
        }
    }
}
