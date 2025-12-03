using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> clickSounds;
    public List<Button> buttons;

    private void Start()
    {
        if (buttons.Count == 0 || clickSounds.Count != buttons.Count)
        {
            Debug.LogWarning("no hay botones");
            return;
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i;
            Debug.Log(buttons + " numero");
            if (buttons[i]!=null)
            {
                buttons[i].onClick.AddListener(() => OnClick(index));
            }
        }
    }

    public void OnClick(int buttonIndex)
    {
        if (audioSource != null && buttonIndex < clickSounds.Count)
            audioSource.PlayOneShot(clickSounds[buttonIndex]);
    }
}
