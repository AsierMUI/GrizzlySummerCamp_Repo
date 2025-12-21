using System.Collections;
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

    private const float MUTE_DB = -80f;

    //Permite referenciarlo pero NO modificarlo. A modo de controlar si "x" valor es "y" o "z" o sí "x" existe.
    public float MasterVolume { get; private set; }
    public float MusicVolume { get; private set; }
    public float SFXVolume { get; private set; }

    //Bools de control Mute
    public bool MasterMuted { get; private set; }
    public bool MusicMuted { get; private set; }
    public bool SFXMuted { get; private set; }

    //Floats de control before Mute
    private float masterLast;
    private float musicLast;
    private float sfxLast;

    //FADE IN & OUT
    private Coroutine musicFadeCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        Load();
        ApplyAll();
    }

    #region Volumes
    public void SetVolume(VolumeType type, float value)
    {
        value = Mathf.Clamp01(value);

        switch (type)
        {
            case VolumeType.Master:
                MasterVolume = value;
                masterLast = value;
                PlayerPrefs.SetFloat("MasterVolume", value);
                mixer.SetFloat("Master", ToDb(value));
                break;

            case VolumeType.Music:
                MusicVolume = value;
                musicLast = value;
                PlayerPrefs.SetFloat("MusicVolume", value);
                mixer.SetFloat("Music", ToDb(value)); break;

            case VolumeType.SFX:
                SFXVolume = value;
                sfxLast = value;
                PlayerPrefs.SetFloat("SFXVolume", value);
                mixer.SetFloat("SFX", ToDb(value));
                break;
        }
    }


    #endregion

    #region Mute
    public void SetMute(VolumeType type, bool mute)
    {
        switch (type)
        {
            case VolumeType.Master:
                MasterMuted = mute;
                PlayerPrefs.SetInt("MasterMuted", mute ? 1 : 0);
                mixer.SetFloat("Master", mute ? MUTE_DB : ToDb(masterLast));
                break;

            case VolumeType.Music:
                MusicMuted = mute;
                PlayerPrefs.SetInt("MusicMuted", mute ? 1 : 0);
                mixer.SetFloat("Music", mute ? MUTE_DB : ToDb(musicLast));
                break;

            case VolumeType.SFX:
                SFXMuted = mute;
                PlayerPrefs.SetInt("SFXMuted", mute ? 1 : 0);
                mixer.SetFloat("SFX", mute ? MUTE_DB : ToDb(sfxLast));
                break;
        }
    }
    #endregion

    #region Fade Music
    //Funciona?
    public void FadeMusicOut(float duration)
    {
        if (musicFadeCoroutine != null)
            StopCoroutine(musicFadeCoroutine);

        musicFadeCoroutine = StartCoroutine(
            FadeMusicCoroutine(MusicVolume, 0f, duration)
        );
    }

    public void FadeMusicIn(float duration)
    {
        if (MusicMuted) return;

        if (musicFadeCoroutine != null)
            StopCoroutine(musicFadeCoroutine);

        musicFadeCoroutine = StartCoroutine(
            FadeMusicCoroutine(MusicVolume, musicLast, duration)
        );
    }

    private IEnumerator FadeMusicCoroutine(float from, float to, float duration)
    {
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float value = Mathf.Lerp(from, to, time / duration);
            SetVolume(VolumeType.Music, value);
            yield return null;
        }

        SetVolume(VolumeType.Music, to);
    }
    #endregion


    #region Init --Load & Helpers

    private void Load()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        MasterMuted = PlayerPrefs.GetInt("MasterMuted", 0) == 1;
        MusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        SFXMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        masterLast = MasterVolume;
        musicLast = MusicVolume;
        sfxLast = SFXVolume;
    }

    private void ApplyAll()
    {
        mixer.SetFloat("Master", MasterMuted ? MUTE_DB : ToDb(masterLast));
        mixer.SetFloat("Music", MusicMuted ? MUTE_DB : ToDb(musicLast));
        mixer.SetFloat("SFX", SFXMuted ? MUTE_DB : ToDb(sfxLast));

    }

    private float ToDb(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
    }

    #endregion

    public AudioSource GetMusicSource() => musicSource;
    public AudioSource GetSFXSource() => sfxSource;
}

public enum VolumeType { Master, Music, SFX }
