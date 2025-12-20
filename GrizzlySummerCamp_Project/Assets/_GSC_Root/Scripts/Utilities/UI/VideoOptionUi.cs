using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class VideoOptionsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    void Start()
    {
        var video = VideoManager.Instance;
        
        //Resoluciones
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(video.GetResolutionOptions());
        resolutionDropdown.SetValueWithoutNotify(video.GetCurrentResolutionIndex());

        //Fullscreen
        fullscreenToggle.SetIsOnWithoutNotify(video.GetFullscreen());

        //Listeners
        resolutionDropdown.onValueChanged.AddListener(video.SetResolution);
        fullscreenToggle.onValueChanged.AddListener(video.SetFullscreen);

    }
}
