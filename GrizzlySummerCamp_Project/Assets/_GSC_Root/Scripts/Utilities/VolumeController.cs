using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider slider;

    void Start()
    {
        //cargar el volumen guardado si es q existe
        float savedVolume = PlayerPrefs.GetFloat("Music", 1f);
        if (slider != null)
        {
            slider.value = savedVolume;

            SetVolume(savedVolume);

            slider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float value)
    {
        float dB = Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f;

        audioMixer.SetFloat("Music", dB);

        PlayerPrefs.SetFloat("Music", value);
    }
}
