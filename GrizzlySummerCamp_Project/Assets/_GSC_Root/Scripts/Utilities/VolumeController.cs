using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeController : MonoBehaviour
{
    #region UI references
    [Header("UI References")]
    [Tooltip("This is the type of audio")]
    [SerializeField] private VolumeType type;
    [Tooltip("Here goes the Slider for changing the audio level")]
    [SerializeField] private Slider slider;
    [Tooltip("Here goes the TMP_text wich will show the audio level")]
    [SerializeField] private TMP_Text text;
    [Tooltip("Here goes the mute button")]
    [SerializeField] private Button muteButton;
    #endregion

    #region On Start
    private void Start()
    {
        float value = GetVolume();
        bool muted = IsMuted();

        if (slider != null)
            slider.SetValueWithoutNotify(muted ? 0f : value);
        UpdateText(slider.value);

        slider.onValueChanged.AddListener(OnSliderChanged);
        muteButton?.onClick.AddListener(ToggleMute);
    }
    private float GetVolume() => type switch
    {
        VolumeType.Master => AudioSettings.Instance.MasterVolume,
        VolumeType.Music => AudioSettings.Instance.MusicVolume,
        VolumeType.SFX => AudioSettings.Instance.SFXVolume,
        _ => 1f
    };
    #endregion

    #region Updating Values
    private void OnSliderChanged(float value)
    {
        AudioSettings.Instance.SetVolume(type, value);
        UpdateText(value);
    }
    private void UpdateText(float v)
    {
        if (text != null)
            text.text = Mathf.RoundToInt(v * 100f) + "%";
    }
    #endregion

    #region Mute
    private void ToggleMute()
    {
        bool newMute = !IsMuted();
        AudioSettings.Instance.SetMute(type, newMute);

        slider.SetValueWithoutNotify(newMute ? 0f : GetVolume());
        UpdateText(slider.value);
    }
    private bool IsMuted() => type switch
    {
        VolumeType.Master => AudioSettings.Instance.MasterMuted,
        VolumeType.Music => AudioSettings.Instance.MusicMuted,
        VolumeType.SFX => AudioSettings.Instance.SFXMuted,
        _ => false
    };
    #endregion

}
