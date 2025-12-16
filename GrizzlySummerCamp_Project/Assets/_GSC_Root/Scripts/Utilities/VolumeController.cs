using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    public enum VolumeType { Master, Music, SFX }

    [Header("Type")]
    public VolumeType volumeType;

    [Header("UI")]
    public Slider slider;
    public TMP_Text volumeText;
    public Button muteButton;

    private float lastVolumeBeforeMute = 1f;

    private void Start()
    {
        bool muted = IsMuted();
        lastVolumeBeforeMute = GetLastVolumeBeforeMute();

        float displayValue = muted ? 0f : lastVolumeBeforeMute;
        slider.SetValueWithoutNotify(displayValue);
        UpdateText(displayValue);

        slider.onValueChanged.AddListener(OnSliderChanged);
        if (muteButton != null)
            muteButton.onClick.AddListener(ToggleMute);
    }

    private void OnSliderChanged(float value)
    {
        if (value <= 0.001f) SetMute(true);
        else
        {
            SetMute(false);
            lastVolumeBeforeMute = value;
            SaveLastVolume(lastVolumeBeforeMute);
            SetVolume(value);
        }
        UpdateText(value);
    }

    private void ToggleMute()
    {
        bool muted = !IsMuted();
        SetMute(muted);

        float newValue = muted ? 0f : lastVolumeBeforeMute;
        if (!muted) SetVolume(lastVolumeBeforeMute);

        SaveLastVolume(lastVolumeBeforeMute);
        slider.SetValueWithoutNotify(newValue);
        UpdateText(newValue);
    }

    private void SaveLastVolume(float value)
    {
        switch (volumeType)
        {
            case VolumeType.Master: AudioSettings.Instance.masterLastVolume = value; PlayerPrefs.SetFloat("MasterLastVolume", value); break;
            case VolumeType.Music: AudioSettings.Instance.musicLastVolume = value; PlayerPrefs.SetFloat("MusicLastVolume", value); break;
            case VolumeType.SFX: AudioSettings.Instance.sfxLastVolume = value; PlayerPrefs.SetFloat("SFXLastVolume", value); break;
        }
    }

    private float GetLastVolumeBeforeMute() => volumeType switch
    {
        VolumeType.Master => AudioSettings.Instance.masterLastVolume,
        VolumeType.Music => AudioSettings.Instance.musicLastVolume,
        VolumeType.SFX => AudioSettings.Instance.sfxLastVolume,
        _ => 1f
    };

    private bool IsMuted() => volumeType switch
    {
        VolumeType.Master => AudioSettings.Instance.masterMuted,
        VolumeType.Music => AudioSettings.Instance.musicMuted,
        VolumeType.SFX => AudioSettings.Instance.sfxMuted,
        _ => false
    };

    private void SetVolume(float value)
    {
        switch (volumeType)
        {
            case VolumeType.Master: AudioSettings.Instance.SetMasterVolume(value); break;
            case VolumeType.Music: AudioSettings.Instance.SetMusicVolume(value); break;
            case VolumeType.SFX: AudioSettings.Instance.SetSFXVolume(value); break;
        }
    }

    private void SetMute(bool mute)
    {
        switch (volumeType)
        {
            case VolumeType.Master: AudioSettings.Instance.SetMasterMute(mute); break;
            case VolumeType.Music: AudioSettings.Instance.SetMusicMute(mute); break;
            case VolumeType.SFX: AudioSettings.Instance.SetSFXMute(mute); break;
        }
    }

    private void UpdateText(float value)
    {
        if (volumeText != null)
            volumeText.text = Mathf.RoundToInt(value * 100f) + "%";
    }
}
