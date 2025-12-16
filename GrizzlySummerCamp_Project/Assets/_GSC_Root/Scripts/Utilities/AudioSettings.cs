using NUnit.Framework.Internal.Filters;
using UnityEngine;
using UnityEngine.Audio;

[DefaultExecutionOrder(-100)]
public class AudioSettings : MonoBehaviour
{
    public static AudioSettings Instance;

    [Header("AudioMixer")]
    public AudioMixer mixer;

    [Header("AudioSources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Volúmenes")]
    public float masterVolume = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    [Header("Mute")]
    public bool masterMuted;
    public bool musicMuted;
    public bool sfxMuted;

    [Header("Last Volume Before Mute")]
    public float masterLastVolume = 1f;
    public float musicLastVolume = 1f;
    public float sfxLastVolume = 1f;

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

    #region Volumes & Mute
    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        PlayerPrefs.SetFloat("MasterVolume", value);

        if (!masterMuted)
        {
            mixer.SetFloat("Master", LinearToDb(value));
            masterLastVolume = value;
            PlayerPrefs.SetFloat("MasterLastVolume", masterLastVolume);
        }
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);

        if (!musicMuted)
        {
            mixer.SetFloat("Music", LinearToDb(value));
            musicLastVolume = value;
            PlayerPrefs.SetFloat("MusicLastVolume", musicLastVolume);
        }

        if (musicSource != null)
            musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);

        if (!sfxMuted)
        {
            mixer.SetFloat("SFX", LinearToDb(value));
            sfxLastVolume = value;
            PlayerPrefs.SetFloat("SFXLastVolume", sfxLastVolume);
        }

        if (sfxSource != null)
            sfxSource.volume = value;
    }

    public void SetMasterMute(bool mute)
    {
        if (mute && !masterMuted) masterLastVolume = masterVolume;
        masterMuted = mute;
        PlayerPrefs.SetInt("MasterMuted", mute ? 1 : 0);
        PlayerPrefs.SetFloat("MasterLastVolume", masterLastVolume);
        mixer.SetFloat("Master", mute ? MUTE_DB : LinearToDb(masterLastVolume));
    }

    public void SetMusicMute(bool mute)
    {
        if (mute && !musicMuted) musicLastVolume = musicVolume;
        musicMuted = mute;
        PlayerPrefs.SetInt("MusicMuted", mute ? 1 : 0);
        PlayerPrefs.SetFloat("MusicLastVolume", musicLastVolume);
        mixer.SetFloat("Music", mute ? MUTE_DB : LinearToDb(musicLastVolume));

        if (musicSource != null)
            musicSource.volume = mute ? 0f : musicLastVolume;
    }

    public void SetSFXMute(bool mute)
    {
        if (mute && !sfxMuted) sfxLastVolume = sfxVolume;
        sfxMuted = mute;
        PlayerPrefs.SetInt("SFXMuted", mute ? 1 : 0);
        PlayerPrefs.SetFloat("SFXLastVolume", sfxLastVolume);
        mixer.SetFloat("SFX", mute ? MUTE_DB : LinearToDb(sfxLastVolume));

        if (sfxSource != null)
            sfxSource.volume = mute ? 0f : sfxLastVolume;
    }
    #endregion

    #region Load & Helpers
    private void LoadVolumes()
    {
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        masterMuted = PlayerPrefs.GetInt("MasterMuted", 0) == 1;
        musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        sfxMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        masterLastVolume = PlayerPrefs.GetFloat("MasterLastVolume", masterVolume);
        musicLastVolume = PlayerPrefs.GetFloat("MusicLastVolume", musicVolume);
        sfxLastVolume = PlayerPrefs.GetFloat("SFXLastVolume", sfxVolume);

        mixer.SetFloat("Master", masterMuted ? MUTE_DB : LinearToDb(masterLastVolume));
        mixer.SetFloat("Music", musicMuted ? MUTE_DB : LinearToDb(musicLastVolume));
        mixer.SetFloat("SFX", sfxMuted ? MUTE_DB : LinearToDb(sfxLastVolume));

        // Aplicar volúmenes a los AudioSources
        if (musicSource != null) musicSource.volume = musicMuted ? 0f : musicLastVolume;
        if (sfxSource != null) sfxSource.volume = sfxMuted ? 0f : sfxLastVolume;
    }

    private float LinearToDb(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

    public AudioSource GetMusicSource() => musicSource;
    public AudioSource GetSFXSource() => sfxSource;
    #endregion
}
