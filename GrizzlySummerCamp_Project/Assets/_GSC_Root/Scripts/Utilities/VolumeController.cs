using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
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
}
