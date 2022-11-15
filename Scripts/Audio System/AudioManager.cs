using Pearl.Events;
using Pearl.Storage;
using Pearl.Tweens;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

namespace Pearl.Audio
{
    public enum ChannelDeafultEnum { Effects, Music, Voice }


    [Serializable]
    public struct SnapshotWeight
    {
        public string snapShotName;
        public float snapShotWeight;

        public SnapshotWeight(in string snapShotName, in float snapShotWeight)
        {
            this.snapShotName = snapShotName;
            this.snapShotWeight = snapShotWeight;
        }
    }


    /// <summary>
    ///  The singleton class automates the sound system using a mixer: examines its channels and creates a 
    ///  container for each of them. Containers allow the class to easily modify the audio.
    /// </summary>
    public class AudioManager : PearlBehaviour, ISingleton, IStoragePlayerPrefs
    {
        #region Inspector Fields
        [SerializeField]
        private AudioTmpManager audioTmpManager = null;
        [SerializeField]
        private AudioSourceManager musicSource = null;
        [SerializeField]
        private string UIGroupMixer = ChannelDeafultEnum.Effects.ToString();

        /// <summary>
        /// The audioMixer
        /// </summary>
        [SerializeField]
        private AudioMixer audioMixer = null;
        [SerializeField]
        private float minRangeAudioDb = -50f;

        [Range(minDb, 0)]
        [SerializeField]
        private float volumeDefault = -5f;

        [SerializeField]
        private string musicVolumeString = "musicVolume";
        [SerializeField]
        private string effectsVolumeString = "effectVolume";
        [SerializeField]
        private string voiceVolumeString = "voiceVolume";

        [SerializeField]
        private string musicPitcherString = "musicPicther";
        [SerializeField]
        private string effectsPitcherString = "effectPicther";
        [SerializeField]
        private string voicePitcherString = "voicePicther";

        [StoragePlayerPrefs("MusicVolume")]
        private float musicVolume;
        [StoragePlayerPrefs("MasterVolume")]
        private float masterVolume;
        [StoragePlayerPrefs("EffectsVolume")]
        private float effectVolume;
        [StoragePlayerPrefs("VoiceVolume")]
        private float voiceVolume;

        private const float minDb = -80f;

        private List<AudioSource> audioSourceInPause = new();
        private Dictionary<string, TweenContainer> mixerTweener = new();
        private Dictionary<AudioSource, TweenContainer> audioSouceTweener = new();

        #endregion

        #region Static interfaces
        /// <summary>
        /// Play an audiosource with specific audioclip
        /// </summary>
        public static void Play(AudioSource soruce, AudioClip clip)
        {
            if (soruce)
            {
                soruce.clip = clip;
                soruce.Play();
            }
        }
        #endregion

        #region Unity CallBacks
        protected override void Awake()
        {
            base.Awake();

            musicVolume = volumeDefault;
            effectVolume = volumeDefault;
            voiceVolume = volumeDefault;

            PearlEventsManager.AddAction<bool>(ConstantStrings.Pause, Pause);
        }

        protected override void Start()
        {
            base.Start();

            StoragePlayerPrefs.Load(this);

            SetMixerVar(musicVolumeString, musicVolume, false);
            SetMixerVar(effectsVolumeString, effectVolume, false);
            SetMixerVar(voiceVolumeString, voiceVolume, false);
        }

        protected void OnApplicationPause(bool pause)
        {
            if (musicSource)
            {
                //musicSource.Pause(pause);
            }
        }

        protected void OnApplicationFocus(bool hasFocus)
        {
            if (musicSource)
            {
                //musicSource.Pause(!hasFocus);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PearlEventsManager.RemoveAction<bool>(ConstantStrings.Pause, Pause);
        }
        #endregion

        #region static Propieties
        public static bool GetIstance(out AudioManager result)
        {
            return Singleton<AudioManager>.GetIstance(out result);
        }

        public static void SaveAudio()
        {
            if (GetIstance(out var audioManager))
            {
                StoragePlayerPrefs.Save(audioManager);
            }
        }

        public static string UIMixer
        {
            get
            {
                if (GetIstance(out var audioManager))
                {
                    return audioManager.UIGroupMixer;
                }
                else return null;
            }
        }

        public static AudioSourceManager MusicSource
        {
            get
            {
                if (GetIstance(out var audioManager))
                {
                    return audioManager.musicSource;
                }
                else return null;
            }
        }
        #endregion

        #region Music region
        public static void ChangeSpeed(float newSpeed)
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.ChangeSpeedInternal(newSpeed);
            }
        }

        public void ChangeSpeedInternal(float newSpeed)
        {
            if (musicSource)
            {
                musicSource.SetChangeSpeed(newSpeed);
            }
        }

        public static void PlayMusic(bool ignoreIfIsPlaying)
        {
            if (GetIstance(out var audioManager))
            {
                if (audioManager && (ignoreIfIsPlaying || !audioManager.musicSource.IsPlaying()))
                {
                    audioManager.musicSource.Play();
                }
            }
        }

        public static void PlayMusic(AudioClip clip)
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.PlayMusicInternal(new ComplexAudioClip(clip));
            }
        }

        public static void PlayMusic(ComplexAudioClip complexAudioClip)
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.PlayMusicInternal(complexAudioClip);
            }
        }

        private void PlayMusicInternal(ComplexAudioClip complexAudioClip)
        {
            if (musicSource)
            {
                musicSource.SetClip(complexAudioClip, true);
            }
        }

        public static void StopMusic()
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.StopMusicInternal();
            }
        }

        private void StopMusicInternal()
        {
            if (musicSource)
            {
                musicSource.Stop();
            }
        }

        public static void ChangePitchMusic(float newPich)
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.ChangePitchMusicInternal(newPich);
            }
        }

        public void ChangePitchMusicInternal(float newPich)
        {
            if (musicSource)
            {
                musicSource.SetPicth(newPich);
            }
        }
        #endregion

        #region Pause region
        public static void DestroyAudioInPause()
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.audioSourceInPause.Clear();
            }
        }

        private void Pause(bool pause)
        {
            if (pause)
            {
                AudioSource[] audioSources = GameObject.FindObjectsOfType<AudioSource>();
                foreach (var audioSource in audioSources)
                {
                    if (audioSource)
                    {
                        if (!audioSource.TryGetComponent<AudioSourceManager>(out AudioSourceManager audioSourceManager) || audioSourceManager.StopSoundInPause)
                        {
                            audioSourceInPause.Add(audioSource);
                            audioSource.Pause();
                        }
                    }
                }
            }
            else
            {
                foreach (var audioSource in audioSourceInPause)
                {
                    if (audioSource)
                    {
                        audioSource.UnPause();
                    }
                }
                audioSourceInPause.Clear();
            }
        }
        #endregion

        #region 2DAudio
        public static AudioSourceManager Play2DAudio(AudioClip clip, ChannelDeafultEnum channelDefaultEnum = ChannelDeafultEnum.Effects, bool usePause = true)
        {
            return Play2DAudio(new ComplexAudioClip(clip), channelDefaultEnum, usePause);
        }

        public static AudioSourceManager Play2DAudio(ComplexAudioClip clip, ChannelDeafultEnum channelDefaultEnum = ChannelDeafultEnum.Effects, bool usePause = true)
        {
            return Play2DAudio(clip, channelDefaultEnum.ToString(), usePause);
        }

        public static AudioSourceManager Play2DAudio(AudioClip clip, string mixerGroupString, bool usePause = true)
        {
            return Play2DAudio(new ComplexAudioClip(clip), mixerGroupString, usePause);
        }

        public static AudioSourceManager Play2DAudio(ComplexAudioClip clip, string mixerGroupString, bool usePause = true)
        {
            if (GetIstance(out var audioManager))
            {
                return audioManager.Play2DAudioInternal(clip, mixerGroupString, usePause);
            }
            else return null;
        }


        private AudioSourceManager Play2DAudioInternal(ComplexAudioClip clip, string mixerGroupString, bool usePause = true)
        {
            if (audioMixer == null)
            {
                return null;
            }

            AudioMixerGroup[] groups = audioMixer.FindMatchingGroups(mixerGroupString);

            if (groups.IsAlmostSpecificCount())
            {
                return Play2DAudio(clip, groups[0], usePause);
            }

            return null;
        }

        public static AudioSourceManager Play2DAudio(AudioClip clip, AudioMixerGroup mixerGroup, bool usePause = true)
        {
            return Play2DAudio(new ComplexAudioClip(clip), mixerGroup, usePause);
        }

        public static AudioSourceManager Play2DAudio(ComplexAudioClip clip, AudioMixerGroup mixerGroup, bool usePause = true)
        {
            if (GetIstance(out var audioManager))
            {
                return audioManager.Play2DAudioInternal(clip, mixerGroup, usePause);
            }
            return null;
        }

        public static AudioSourceManager PlayAudio(AudioClip clip, AudioMixerGroup mixerGroup, Vector3 position, bool usePause = true)
        {
            if (GetIstance(out var audioManager))
            {
                return audioManager.PlayAudioInternal(clip, mixerGroup, position, usePause);
            }
            return null;
        }

        public static AudioSourceManager PlayAudio(AudioClip clip, Vector3 position, ChannelDeafultEnum channelDefaultEnum = ChannelDeafultEnum.Effects, bool usePause = true)
        {
            return PlayAudio(clip, channelDefaultEnum.ToString(), position, usePause);
        }

        public static AudioSourceManager PlayAudio(AudioClip clip, string mixerGroup, Vector3 position, bool usePause = true)
        {
            if (GetIstance(out var audioManager))
            {
                return audioManager.PlayAudioInternal(clip, mixerGroup, position, usePause);
            }
            return null;
        }

        private AudioSourceManager Play2DAudioInternal(ComplexAudioClip clip, AudioMixerGroup mixerGroup, bool usePause = true)
        {
            if (audioTmpManager)
            {
                return audioTmpManager.Play2D(clip, mixerGroup, usePause);
            }
            return null;
        }

        private AudioSourceManager PlayAudioInternal(AudioClip clip, AudioMixerGroup mixerGroup, Vector3 position, bool usePause = true)
        {
            if (audioTmpManager)
            {
                return audioTmpManager.Play(clip, mixerGroup, position, usePause);
            }
            return null;
        }

        private AudioSourceManager PlayAudioInternal(AudioClip clip, string mixerGroup, Vector3 position, bool usePause = true)
        {
            AudioMixerGroup[] groups = audioMixer.FindMatchingGroups(mixerGroup);

            if (groups.IsAlmostSpecificCount())
            {
                if (audioTmpManager)
                {
                    return audioTmpManager.Play(clip, groups[0], position, usePause);
                }
            }
            return null;
        }
        #endregion

        #region Mixer Functions
        public static void ChangeSnapshot(float timeToReach, params SnapshotWeight[] SnapshotStruct)
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.ChangeSnapShotInternal(timeToReach, SnapshotStruct);
            }
        }

        public void ChangeSnapShotInternal(float timeToReach, params SnapshotWeight[] SnapshotStruct)
        {
            if (SnapshotStruct != null && audioMixer != null)
            {
                AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[SnapshotStruct.Length];
                float[] weights = new float[SnapshotStruct.Length];
                int index = 0;

                foreach (var snapshotWeight in SnapshotStruct)
                {
                    AudioMixerSnapshot snapshot = audioMixer.FindSnapshot(snapshotWeight.snapShotName);

                    if (snapshot == null)
                    {
                        return;
                    }

                    if (index < snapshots.Length && index < weights.Length)
                    {
                        snapshots[index] = snapshot;
                        weights[index] = snapshotWeight.snapShotWeight;
                        index++;
                    }
                }

                audioMixer.TransitionToSnapshots(snapshots, weights, timeToReach);
            }
        }

        public static void SetMixerVolume(ChannelDeafultEnum channelDefaultEnum, float newVolume, bool isPercent)
        {
            SetMixerVar(GetMixerVolumeName(channelDefaultEnum), newVolume, isPercent);
        }

        public static void SetMixerVolume(ChannelDeafultEnum channelDefaultEnum, float newVolume, bool isPercent, float time)
        {
            SetMixerVar(GetMixerVolumeName(channelDefaultEnum), newVolume, isPercent, time);
        }

        /// <summary>
        /// Sets Volume of specific channel of mixer
        /// </summary>
        /// <param name = "newVolume">The volume to be reached</param>
        /// <param name = "isPercent">If it is false, the volume is expressed in decibels, it is true in a number in the interval [0, 1]</param>
        public static void SetMixerVar(in string nameVarMixer, float newVolume, bool isPercent)
        {
            SetMixerVar(nameVarMixer, newVolume, isPercent, 0);
        }

        /// <summary>
        /// Sets Volume of specific channel of mixer
        /// </summary>
        /// <param name = "newVolume">The volume to be reached</param>
        /// <param name = "isPercent">If it is false, the volume is expressed in decibels, it is true in a number in the interval [0, 1]</param>
        public static void SetMixerVar(string nameVarMixer, float newVolume, bool isPercent, float time)
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.SetMixerVarInternal(nameVarMixer, newVolume, isPercent, time);
            }
        }

        public void SetMixerVarInternal(string nameVarMixer, float newValue, bool isPercent, float time)
        {
            if (nameVarMixer != null && audioMixer != null)
            {
                newValue = isPercent ? ((newValue != 0) ? MathfExtend.ChangeRange01(newValue, minRangeAudioDb, 0) : minDb) : (newValue > minRangeAudioDb) ? Mathf.Clamp(newValue, minRangeAudioDb, 0) : newValue;

                if (time != 0)
                {
                    if (mixerTweener.IsNotNullAndTryGetValue(nameVarMixer, out TweenContainer tweener))
                    {
                        tweener?.Kill();
                    }
                    TweenContainer aux = audioMixer.MixerParamTween(nameVarMixer, time, true, ChangeModes.Time, newValue);
                    if (aux != null)
                    {
                        aux.OnComplete += OnCompleteMixer;
                    }
                    mixerTweener?.Update(nameVarMixer, aux);
                }
                else
                {
                    audioMixer.SetFloat(nameVarMixer, newValue);
                }

                SetNewValueInStoreData(nameVarMixer, newValue);
            }
        }

        public static float GetMixerVolume(ChannelDeafultEnum channelDefaultEnum, bool isPercent)
        {
            return GetMixerVar(GetMixerVolumeName(channelDefaultEnum), isPercent);
        }

        /// <summary>
        /// Returns the volume of specific channel of mixer
        /// </summary>
        public static float GetMixerVar(string nameVarMixer, bool isPercent)
        {
            if (GetIstance(out var audioManager))
            {
                return audioManager.GetMixerVarInternal(nameVarMixer, isPercent);
            }
            return -1f;
        }

        private float GetMixerVarInternal(string nameVarMixer, bool isPercent)
        {
            if (audioMixer && nameVarMixer != null)
            {
                audioMixer.GetFloat(nameVarMixer, out float volumeMixer);
                if (isPercent)
                {
                    volumeMixer = MathfExtend.Percent(volumeMixer, minRangeAudioDb, 0);
                }
                return volumeMixer;
            }

            return -1f;
        }

        private static string GetMixerVolumeName(ChannelDeafultEnum channelDefaultEnum)
        {
            if (GetIstance(out var audioManager))
            {
                switch (channelDefaultEnum)
                {
                    case ChannelDeafultEnum.Effects:
                        return audioManager.effectsVolumeString;
                    case ChannelDeafultEnum.Music:
                        return audioManager.musicVolumeString;
                    default:
                        return audioManager.voiceVolumeString;
                }
            }
            return null;
        }

        private static string GetMixerPitchName(ChannelDeafultEnum channelDefaultEnum)
        {
            if (GetIstance(out var audioManager))
            {
                switch (channelDefaultEnum)
                {
                    case ChannelDeafultEnum.Effects:
                        return audioManager.effectsPitcherString;
                    case ChannelDeafultEnum.Music:
                        return audioManager.musicPitcherString;
                    default:
                        return audioManager.voicePitcherString;
                }
            }
            return null;
        }
        #endregion

        #region AudioSource
        /// <summary>
        /// Sets Volume of specific AudioSource
        /// </summary>
        /// <param name = "newVolume">The volume to be reached</param>
        public static void SetAudioSourceVolume(AudioSource audioSource, float newVolume)
        {
            SetAudioSourceVolume(audioSource, newVolume, 0);
        }

        /// <summary>
        /// Sets Volume of specific AudioSource
        /// </summary>
        /// <param name = "newVolume">The volume to be reached</param>
        /// <param name = "time"> Time for volume transition</param>
        /// <param name = "curve">The transition curve.If the curve does not exist, the volume change is linear, if it exists, the change follows the curve.</param>
        public static void SetAudioSourceVolume(AudioSource audioSource, float newVolume, float time)
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.SetAudioSourceVolumeInternal(audioSource, newVolume, time);
            }
        }

        public void SetAudioSourceVolumeInternal(AudioSource audioSource, float newVolume, float time)
        {
            if (audioSource)
            {
                newVolume = Mathf.Clamp(newVolume, minRangeAudioDb, 0);

                if (time != 0)
                {
                    if (audioSouceTweener.IsNotNullAndTryGetValue(audioSource, out TweenContainer tweener))
                    {
                        tweener?.Kill();
                    }

                    TweenContainer aux = audioSource.VolumeTween(time, true, ChangeModes.Time, newVolume);
                    if (aux != null)
                    {
                        aux.OnComplete += OnCompleteSource;
                    }
                    audioSouceTweener?.Update(audioSource, aux);
                }
                else
                {
                    audioSource.volume = newVolume;
                }
            }
        }

        public static void SetSpeed(AudioSource audioSource, string nameVarPitcherMixer, float newSpeed)
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.SetSpeedInternal(audioSource, nameVarPitcherMixer, newSpeed);
            }
        }

        public static void SetSpeed(AudioSource audioSource, ChannelDeafultEnum channelDeafultEnum, float newSpeed)
        {
            if (GetIstance(out var audioManager))
            {
                audioManager.SetSpeedInternal(audioSource, GetMixerPitchName(channelDeafultEnum), newSpeed);
            }
        }


        public void SetSpeedInternal(AudioSource audioSource, string nameVarPitcherMixer, float newSpeed)
        {
            newSpeed = Mathf.Clamp(newSpeed, 0, 3);

            if (audioSource && audioMixer != null && newSpeed != 0)
            {
                audioSource.pitch = newSpeed;
                audioMixer.SetFloat(nameVarPitcherMixer, 1f / newSpeed);
            }
        }
        #endregion

        #region Privates

        private void SetNewValueInStoreData(in string nameStoreVar, float newValue)
        {
            if (nameStoreVar == musicVolumeString)
            {
                musicVolume = newValue;
            }
            if (nameStoreVar == effectsVolumeString)
            {
                effectVolume = newValue;
            }
            if (nameStoreVar == voiceVolumeString)
            {
                voiceVolume = newValue;
            }
        }

        private void OnCompleteMixer(TweenContainer tweenContainer, float error)
        {
            if (mixerTweener == null)
            {
                return;
            }


            var mixerElements = mixerTweener.Where(x => x.Value.CurrentState == TweenState.Complete);
            foreach (var e in mixerElements)
            {
                mixerTweener.Remove(e.Key);
            }
        }

        private void OnCompleteSource(TweenContainer tweenContainer, float error)
        {
            if (audioSouceTweener == null)
            {
                return;
            }

            var audioElements = audioSouceTweener.Where(x => x.Value.CurrentState == TweenState.Complete);
            foreach (var e in audioElements)
            {
                audioSouceTweener.Remove(e.Key);
            }
        }
        #endregion
    }
}

