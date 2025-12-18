using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("AudioMixer")]
    public AudioMixer mixer;

    [System.Serializable]
    public class NamedAudio
    {
        public string key;
        public AudioClip clip;
    }

    [Header("Listas de sonidos")]
    public List<NamedAudio> soundList = new();
    private Dictionary<string, AudioClip> soundDict;

    private AudioSource musicSource;
    private AudioSource SFXSource;

    private void Awake()
    {
        musicSource = AudioSettings.Instance.GetMusicSource();
        SFXSource = AudioSettings.Instance.GetSFXSource();

        soundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in soundList)
        {
            if (!soundDict.ContainsKey(sound.key))
                soundDict.Add(sound.key, sound.clip);
        }
    }

    private void Start()
    {
        if (musicSource != null && soundDict.ContainsKey("Music"))
        {
            musicSource.clip = soundDict["Music"];
            if (!musicSource.isPlaying) musicSource.Play();

            AudioSettings.Instance.SetVolume(VolumeType.Music, AudioSettings.Instance.MusicVolume);
        }
    }

    public void PlaySFX(string key)
    {
        if (SFXSource != null && soundDict.ContainsKey(key))
            SFXSource.PlayOneShot(soundDict[key]);
    }

    public void PlayMusic(string key)
    {
        if (musicSource != null && soundDict.ContainsKey(key))
        {
            musicSource.clip = soundDict[key];
            musicSource.Play();
        }
    }
}
