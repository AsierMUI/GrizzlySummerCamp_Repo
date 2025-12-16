using UnityEngine;
//using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{

    public enum VolumeType 
    {
        Master,
        Music,
        SFX    
    }

    [Header("Type")]
    public VolumeType volumeType;

    [Header("UI")]
    public Slider slider;
    public TMP_Text volumeText;
    public Button muteButton;

    private float lastVolumeBeforeMute = 1f;

    private void Start()
    {
        float volume = GetVolume();
        bool muted = IsMuted();

        if (!muted)
            lastVolumeBeforeMute = volume;

        slider.SetValueWithoutNotify(muted ? 0f : volume);
        UpdateText(muted ? 0f : volume);

        slider.onValueChanged.AddListener(OnSliderChanged);

        if (muteButton != null)
            muteButton.onClick.AddListener(ToggleMute);
    }

    private void OnSliderChanged(float value)
    {
        if (value <= 0.001f)
        {
            SetMute(true);
            UpdateText(0f);
            return;
        }

        SetMute(false);
        lastVolumeBeforeMute = value;
        SetVolume(value);
        UpdateText(value);
    }


    private void ToggleMute()
    {
        bool muted = !IsMuted();
        SetMute(muted);

        float newValue = muted ? 0f : lastVolumeBeforeMute;
        slider.SetValueWithoutNotify(newValue);
        UpdateText(newValue);
    }
    private float GetVolume()
    {
        return volumeType switch
        {
            VolumeType.Master => AudioSettings.Instance.masterVolume,
            VolumeType.Music => AudioSettings.Instance.musicVolume,
            VolumeType.SFX => AudioSettings.Instance.sfxVolume,
            _ => 1f
        };
    }
    private bool IsMuted()
    {
        return volumeType switch
        {
            VolumeType.Master => AudioSettings.Instance.masterMuted,
            VolumeType.Music => AudioSettings.Instance.musicMuted,
            VolumeType.SFX => AudioSettings.Instance.sfxMuted,
            _ => false
        };
    }
    private void SetVolume(float value)
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                AudioSettings.Instance.SetMasterVolume(value);
                break;
            case VolumeType.Music:
                AudioSettings.Instance.SetMusicVolume(value);
                break;
            case VolumeType.SFX:
                AudioSettings.Instance.SetSFXVolume(value);
                break;
        }
    }
    private void SetMute(bool mute)
    {
        switch (volumeType)
        {
            case VolumeType.Master:
                AudioSettings.Instance.SetMasterMute(mute);
                break;
            case VolumeType.Music:
                AudioSettings.Instance.SetMusicMute(mute);
                break;
            case VolumeType.SFX:
                AudioSettings.Instance.SetSFXMute(mute);
                break;
        }
    }
    private void UpdateText(float value)
    {
        if (volumeText != null)
            volumeText.text = Mathf.RoundToInt(value * 100f) + "%";
    }
    /*
    [Header("Audio")]
    public AudioMixer audioMixer;
    public string exposedParam = "Music";
    public string saveKey = "MusicVolume";

    [Header("UI")]
    public Slider slider;
    public TMP_Text volumeText;
    public Button muteButton;
    public Button resetButton;

    private float lastVolumeBeforeMute = 1f;
    private bool isMuted = false;

    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat(saveKey, 1f);
        slider.value = savedVolume;

        ApplyVolume(savedVolume);
        UpdateText(savedVolume);

        slider.onValueChanged.AddListener(value =>
            {
                if (isMuted && value > 0f)
                {
                    isMuted = false;
                    lastVolumeBeforeMute = value;
                }
                ApplyVolume(value);
                UpdateText(value);
            });

        if (muteButton !=null)
        {
            muteButton.onClick.AddListener(ToggleMute);
        }

        if (resetButton !=null)
        {
            resetButton.onClick.AddListener(ResetVolume);
        }
    }

    void ApplyVolume(float value) 
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;
        audioMixer.SetFloat(exposedParam, dB);

        if (!isMuted) 
        {
            PlayerPrefs.SetFloat(saveKey, value);
        }
    }

    void UpdateText(float value) 
    {
        int percent = Mathf.RoundToInt(value * 100f);
        if (volumeText != null)
        {
            volumeText.text = percent + "%";
        }
    }

    public void ToggleMute() 
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            lastVolumeBeforeMute = slider.value;
            slider.value = 0;
        }
        else 
        {
            slider.value = lastVolumeBeforeMute;
        }
    }

    public void ResetVolume()
    {
        if (!isMuted) 
        {
            slider.value = 1f;
            lastVolumeBeforeMute = 1f;
            UpdateText(1f);
        }
    }
    */
}
