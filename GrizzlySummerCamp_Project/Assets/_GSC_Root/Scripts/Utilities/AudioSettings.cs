using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettings : MonoBehaviour
{

    public static AudioSettings Instance;

    [Header("AudioMixer")]
    public AudioMixer mixer;

    [Header("Volúmenes")]
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolumes();
    }

    #region VOLUMES
    public void SetMasterVolume(float value) 
    {
        masterVolume = value;
        mixer.SetFloat("Master", LinearToDb(value));
        PlayerPrefs.SetFloat("MasterVolume",value);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        mixer.SetFloat("Music", LinearToDb(value));
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        mixer.SetFloat("SFX", LinearToDb(value));
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private float LinearToDb(float value) 
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

    public void MuteAll(bool mute) 
    {
        mixer.SetFloat("Master", mute ? -80f : LinearToDb(masterVolume));
    }

    private void LoadVolumes() 
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);    
    }


    #endregion

}
