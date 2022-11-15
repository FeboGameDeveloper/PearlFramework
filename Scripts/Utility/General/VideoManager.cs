using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Pearl
{
    [Serializable]
    public struct ComplexVideoClip
    {
        public float speedVideo;
        public VideoClip[] clips;

        public ComplexVideoClip(float speed, params VideoClip[] clips)
        {
            this.clips = clips;
            this.speedVideo = speed;
        }

        public ComplexVideoClip(params VideoClip[] clips)
        {
            this.clips = clips;
            this.speedVideo = 1;
        }
    }

    //Classe che gestisce i video
    public class VideoManager : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer videoPlayer = null;

        public UnityEvent OnFinish;

        private VideoClip[] _clips;
        private bool _sequences = false;
        private int _currentIndex = 0;
        private bool sourceLoop = false;

        protected void Start()
        {
            if (videoPlayer)
            {
                videoPlayer.loopPointReached += EndReached;
            }
        }

        protected void OnDestroy()
        {
            if (videoPlayer)
            {
                videoPlayer.loopPointReached -= EndReached;
            }
        }

        public void SetVideo(bool play = false)
        {
            if (_clips != null && _currentIndex < _clips.Length && videoPlayer != null)
            {
                videoPlayer.clip = _clips[_currentIndex];
            }

            if (play)
            {
                videoPlayer.Play();
            }
        }

        public void Play()
        {
            if (videoPlayer != null)
            {
                videoPlayer.Play();
            }
        }

        public void SetVideo(in VideoClip clip, bool play = false)
        {
            if (videoPlayer && clip != null)
            {
                _sequences = false;
                _clips = new VideoClip[] { clip };
                _currentIndex = 0;
                SetVideo(play);
            }
        }

        public void SeVideos(VideoClip[] clips, bool play = false)
        {
            if (videoPlayer && clips.IsAlmostSpecificCount())
            {
                sourceLoop = videoPlayer.isLooping;

                if (clips.Length == 1)
                {
                    SetVideo(clips[0], play);
                }
                else
                {
                    _sequences = true;
                    _clips = clips;
                    _currentIndex = 0;
                    SetVideo(play);
                }
            }
        }

        public void SetVideo(ComplexVideoClip complexClips, bool play = false)
        {
            if (videoPlayer != null)
            {
                videoPlayer.playbackSpeed = complexClips.speedVideo;
            }
            SeVideos(complexClips.clips, play);
        }

        public void Stop()
        {
            if (videoPlayer != null)
            {
                videoPlayer.Stop();
                videoPlayer.targetTexture.Release();
            }
        }

        public void Release()
        {
            if (videoPlayer != null && videoPlayer.targetTexture.IsNotNull(out var targetTexture))
            {
                targetTexture.Release();
            }
        }

        private void EndReached(VideoPlayer videoPlayer)
        {
            if (_sequences)
            {
                _currentIndex = sourceLoop ? MathfExtend.ChangeInCircle(_currentIndex, 1, _clips.Length) : ++_currentIndex;

                if (sourceLoop || _currentIndex < _clips.Length)
                {
                    SetVideo(true);
                }
                else
                {
                    OnFinish?.Invoke();
                }
            }
            else
            {
                OnFinish?.Invoke();
            }
        }
    }
}
