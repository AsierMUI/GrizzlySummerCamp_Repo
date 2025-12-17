using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private VolumeType type;
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    [SerializeField] private Button muteButton;
    private void Start()
    {
        float value = GetVolume();
        bool muted = IsMuted();

        slider.SetValueWithoutNotify(muted ? 0f : value);
        UpdateText(slider.value);

        slider.onValueChanged.AddListener(OnSliderChanged);
        muteButton?.onClick.AddListener(ToggleMute);
    }

    private void OnSliderChanged(float value)
    {
        AudioSettings.Instance.SetMute(type, value <= 0.001f);
        AudioSettings.Instance.SetVolume(type, value);
        UpdateText(value);
    }

    private void ToggleMute()
    {
        bool newMute = !IsMuted();
        AudioSettings.Instance.SetMute(type, newMute);
        slider.SetValueWithoutNotify(newMute ? 0f : GetVolume());
        UpdateText(slider.value);
    }

    private float GetVolume() => type switch
    {
        VolumeType.Master => AudioSettings.Instance.MasterVolume,
        VolumeType.Music => AudioSettings.Instance.MusicVolume,
        VolumeType.SFX => AudioSettings.Instance.SFXVolume,
        _ => 1f
    };

    private bool IsMuted() => type switch
    {
        VolumeType.Master => AudioSettings.Instance.MasterMuted,
        VolumeType.Music => AudioSettings.Instance.MusicMuted,
        VolumeType.SFX => AudioSettings.Instance.SFXMuted,
        _ => false
    };

    private void UpdateText(float v)
    {
        if (text != null)
            text.text = Mathf.RoundToInt(v * 100f) + "%";
    }
}
