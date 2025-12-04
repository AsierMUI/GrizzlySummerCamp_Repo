using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonClickSound : MonoBehaviour
{
    [Tooltip("Botones qye reproducen sonido")]
    public List<Button> buttons;

    [Tooltip("Botones qye reproducen sonido")]
    public List<string> soundNames;

    /*
    public AudioSource audioSource;
    public List<AudioClip> clickSounds;
    */
    private void Start()
    {
        for (int i = 0; i < buttons.Count; i++) 
        {
            int index = i;
            buttons[i].onClick.AddListener(() =>
            {
                AudioManager.Current?.PlaySFX(soundNames[index]);
            });
        }
        /*
        if (buttons.Count == 0 || clickSounds.Count != buttons.Count)
        {
            Debug.LogWarning("no hay botones");
            return;
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            if (buttons[i]!=null)
            {
                buttons[i].onClick.AddListener(() => OnClick(index));
            }
        }
        */
    }

    public void PlaySound(int index = 0) 
    {
        AudioManager.Current?.PlaySFX(soundNames[index]);
    }

    /*
    public void OnClick(int buttonIndex)
    {
        if (audioSource != null && buttonIndex < clickSounds.Count)
            audioSource.PlayOneShot(clickSounds[buttonIndex]);
    }
    */
}
