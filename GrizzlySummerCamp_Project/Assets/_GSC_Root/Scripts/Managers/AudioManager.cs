using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Current;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("AudioMixer")]
    public AudioMixer mixer;

    [System.Serializable]
    public class NamedAudio 
    {
        public string key;
        public AudioClip clip;
    }

    [Header("Listas de sonidos")/*"Sonidos escena"*/]
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

    public void SetMusicVolume(float value) => mixer.SetFloat("Music", Mathf.Log10(value) * 20);
    public void SetSFXVolume(float value) => mixer.SetFloat("SFX", Mathf.Log10(value) * 20);

}
