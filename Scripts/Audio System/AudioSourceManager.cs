using Pearl.Debug;
using Pearl.Events;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace Pearl
{
    [Serializable]
    public struct ComplexAudioClip
    {
        public AudioClip[] clips;

        public ComplexAudioClip(params AudioClip[] clips)
        {
            this.clips = clips;
        }
    }

    public class AudioSourceManager : MonoBehaviour, IReset
    {
        #region Insepctor
        [SerializeField]
        private AudioSource audioSource = null;
        [SerializeField]
        private bool stopSoundInPause = true;
        [SerializeField]
        private bool random = false;

        public UnityEvent OnFinishClip = null;
        public UnityEvent OnFinishPieceOfClip = null;
        #endregion

        #region private fields
        private bool _isPause = false;
        private AudioClip[] _clips;
        private bool _sequences = false;
        private int _currentIndex = 0;
        private bool _sourceLoop = false;
        private bool _start;
        #endregion

        #region propietes
        public bool IsPause { get { return _isPause; } }

        public bool Loop { set { _sourceLoop = value; } }


        public AudioMixerGroup OutputAudioMixerGroup
        {
            get
            {
                if (audioSource != null)
                {
                    return audioSource.outputAudioMixerGroup;
                }
                return null;
            }
            set
            {
                if (audioSource != null)
                {
                    audioSource.outputAudioMixerGroup = value;
                }
            }
        }

        public float Percent
        {
            get
            {
                if (audioSource != null && audioSource.clip != null)
                {
                    return audioSource.time / audioSource.clip.length;
                }
                return 0;
            }
        }

        public bool StopSoundInPause
        {
            get
            {
                return stopSoundInPause;
            }
            set
            {
                if (gameObject.activeSelf)
                {
                    if (!stopSoundInPause && value)
                    {
                        PearlEventsManager.AddAction<bool>(ConstantStrings.Pause, Pause);
                    }
                    else if (stopSoundInPause && !value)
                    {
                        PearlEventsManager.RemoveAction<bool>(ConstantStrings.Pause, Pause);
                    }
                }

                stopSoundInPause = value;
            }
        }
        #endregion

        #region UnityCallbacks
        protected void Awake()
        {
            if (audioSource)
            {
                _sourceLoop = audioSource.loop;
            }
        }

        protected void Reset()
        {
            audioSource = GetComponent<AudioSource>();
        }

        protected void Update()
        {
            if (_start && (!IsWorking() || IsFinish()))
            {
                FinishClipCallback();
            }
        }

        protected void OnEnable()
        {
            if (stopSoundInPause)
            {
                PearlEventsManager.AddAction<bool>(ConstantStrings.Pause, Pause);
            }
        }

        protected void OnDisable()
        {
            if (stopSoundInPause)
            {
                PearlEventsManager.RemoveAction<bool>(ConstantStrings.Pause, Pause);
            }
        }
        #endregion


        public void DestroyAudioSource()
        {
            GameObjectExtend.DestroyExtend(gameObject);
        }

        public void Play()
        {
            if (audioSource)
            {
                audioSource.Play();
                _start = true;
            }
        }

        public bool IsFinish()
        {
            return Percent >= 1;
        }

        public bool IsWorking()
        {
            return IsPlaying() || IsPause;
        }

        public bool IsPlaying()
        {
            if (audioSource)
            {
                return audioSource.isPlaying;
            }
            return false;
        }

        public void SetClip(bool play = false)
        {
            if (_clips != null && _currentIndex < _clips.Length && audioSource != null)
            {
                audioSource.clip = _clips[_currentIndex];
            }

            if (play)
            {
                Play();
            }
        }

        public void SetClip(in AudioClip clip, bool play = false)
        {
            if (audioSource && clip != null)
            {
                _sequences = false;
                _clips = new AudioClip[] { clip };
                audioSource.loop = _sourceLoop;
                _currentIndex = 0;
                SetClip(play);
            }
        }

        public void SetClips(AudioClip[] clips, bool play = false)
        {
            if (audioSource && clips.IsAlmostSpecificCount())
            {
                if (clips.Length == 1)
                {
                    SetClip(clips[0], play);
                }
                else
                {
                    _sequences = true;
                    _clips = clips;
                    _sourceLoop = audioSource.loop;
                    audioSource.loop = false;
                    _currentIndex = random ? UnityEngine.Random.Range(0, _clips.Length) : 0;
                    SetClip(play);
                }
            }
        }

        public void SetClip(ComplexAudioClip complexClips, bool play = false)
        {
            SetClips(complexClips.clips, play);
        }

        public void SetPicth(float newPitch)
        {
            if (audioSource)
            {
                audioSource.pitch = newPitch;
            }
        }

        public void SetChangeSpeed(float newSpeed)
        {
            SetPicth(newSpeed);
            var mixerGroup = audioSource.outputAudioMixerGroup;
            if (!mixerGroup.audioMixer.SetFloat("musicPicther", 1f / newSpeed))
            {
                LogManager.LogWarning("The parameter is not enable for speed");
            }
        }

        public void Stop()
        {
            if (audioSource)
            {
                audioSource.Stop();
                ResetAudioSource();
            }
        }

        public void Pause(bool isPause)
        {
            _isPause = isPause;
            if (audioSource && stopSoundInPause)
            {
                if (isPause)
                {
                    audioSource.Pause();
                }
                else
                {
                    audioSource.UnPause();
                }
            }
        }

        private void ResetAudioSource()
        {
            _start = false;
            _isPause = false;
            _currentIndex = 0;
            audioSource.loop = _sourceLoop;
        }

        private void FinishClipCallback()
        {
            OnFinishPieceOfClip?.Invoke();

            if (_sequences)
            {
                if (random)
                {
                    _currentIndex = UnityEngine.Random.Range(0, _clips.Length);
                }
                else
                {
                    _currentIndex = _sourceLoop ? MathfExtend.ChangeInCircle(_currentIndex, 1, _clips.Length) : ++_currentIndex;
                }

                if (_sourceLoop || _currentIndex < _clips.Length)
                {
                    SetClip(true);
                }
                else
                {
                    FinishClip();
                }
            }
            else
            {
                FinishClip();
            }
        }

        private void FinishClip()
        {
            ResetAudioSource();
            OnFinishClip?.Invoke();
        }

        public void ResetElement()
        {
        }

        public void DisableElement()
        {
        }
    }
}