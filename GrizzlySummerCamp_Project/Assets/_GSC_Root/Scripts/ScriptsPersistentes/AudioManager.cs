using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

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
            musicSource.Play();
        }
    }

    #region Play Function

    public void PlaySFX(string key, float volumeMultiplier = 1f)
    {
        if (SFXSource != null && soundDict.ContainsKey(key))
            SFXSource.PlayOneShot(soundDict[key], volumeMultiplier);
    }

    public void PlayMusic(string key)
    {
        if (musicSource != null && soundDict.ContainsKey(key))
        {
            musicSource.clip = soundDict[key];
            musicSource.Play();
        }
    }
    #endregion
}
