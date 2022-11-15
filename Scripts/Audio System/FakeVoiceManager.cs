using UnityEngine;

namespace Pearl
{
    public class FakeVoiceManager : MonoBehaviour
    {
        [SerializeField]
        private AudioClip[] audioclips = null;
        [SerializeField]
        private AudioSourceManager audioSource = null;

        [SerializeField]
        private TextManager textManager = null;

        private AudioClip _currentAudioClip;
        private bool _active;

        private void Awake()
        {
            if (textManager)
            {
                textManager.OnStartWriteText.AddListener(Active);
                textManager.OnFinishWriteText.AddListener(Disactive);
                textManager.OnWait += Wait;
            }
        }

        private void OnDestroy()
        {
            if (textManager)
            {
                textManager.OnStartWriteText.RemoveListener(Active);
                textManager.OnFinishWriteText.RemoveListener(Disactive);
                textManager.OnWait -= Wait;
            }
        }

        private void Active()
        {
            _active = true;
        }

        private void Disactive()
        {
            _active = false;
        }

        private void Wait(bool value)
        {
            _active = !value;
        }

        private void Play()
        {
            _currentAudioClip = RandomExtend.GetRandomElement<AudioClip>(audioclips, _currentAudioClip);
            audioSource.Stop();
            audioSource.SetClip(_currentAudioClip);
            audioSource.Play();
        }


        public void Update()
        {
            if (!_active)
            {
                return;
            }

            if (!audioSource.IsPlaying() && !audioSource.IsPause)
            {
                Play();
            }
        }
    }
}
