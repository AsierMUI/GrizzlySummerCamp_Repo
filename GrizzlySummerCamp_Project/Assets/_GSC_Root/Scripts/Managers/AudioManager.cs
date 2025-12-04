using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Current;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [System.Serializable]
    public class NamedAudio 
    {
        public string key;
        public AudioClip clip;
    }

    [Header("Sonidos escena")]
    public List<NamedAudio> soundList = new();

    private Dictionary<string, AudioClip> soundDict;

    private void Awake()
    {
        Current = this;

        soundDict = new Dictionary<string, AudioClip>();
        foreach (var sound in soundList) 
        {
            if (!soundDict.ContainsKey(sound.key))
            {
                soundDict.Add(sound.key, sound.clip);
            }
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

    public void PlaySFX(string key)
    {
        if (SFXSource != null && soundDict.ContainsKey(key))
            SFXSource.PlayOneShot(soundDict[key]);
    }
}
