using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-100)]
public class AudioSettings : MonoBehaviour
{

    public static AudioSettings Instance;

    [Header("AudioMixer")]
    public AudioMixer mixer;

    [Header("Volúmenes")]
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;


    [Header("Muted Volume")]
    public bool masterMuted;
    public bool musicMuted;
    public bool sfxMuted;

    private const float MUTE_DB = -80f;


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

    #region Volumes&Mute
    public void SetMasterVolume(float value) 
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume",value);
        
        if (!masterMuted)
            mixer.SetFloat("Master", LinearToDb(value));

    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);

        if (!musicMuted)
            mixer.SetFloat("Music", LinearToDb(value));
    }
    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
        
        if (!sfxMuted)
            mixer.SetFloat("SFX", LinearToDb(value));
    }

    public void SetMasterMute(bool mute) 
    {
        masterMuted = mute;
        PlayerPrefs.SetInt("MasterMuted", mute ? 1 : 0);

        mixer.SetFloat("Master", mute ? MUTE_DB : LinearToDb(masterVolume));
    }

    public void SetMusicMute(bool mute)
    {
        musicMuted = mute;
        PlayerPrefs.SetInt("MusicMuted", mute ? 1 : 0);

        mixer.SetFloat("Music", mute ? MUTE_DB : LinearToDb(musicVolume));
    }

    public void SetSFXMute(bool mute)
    {
        sfxMuted = mute;
        PlayerPrefs.SetInt("SFXMuted", mute ? 1 : 0);

        mixer.SetFloat("SFX", mute ? MUTE_DB : LinearToDb(sfxVolume));
    }

    #endregion


    #region VOLUMEStoLOAD
    /*
    public void MuteAll(bool mute) 
    {
        mixer.SetFloat("Master", mute ? -80f : LinearToDb(masterVolume));
    }
    */
    private void LoadVolumes() 
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterMuted = PlayerPrefs.GetInt("MasterMuted", 0) == 1;
        musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        /*
        SetMasterVolume(masterMuted);
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);    
        */

        SetMasterMute(masterMuted);
        SetMusicMute(musicMuted);
        SetSFXMute(sfxMuted);
    }

    #endregion
    private float LinearToDb(float value) 
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

}
