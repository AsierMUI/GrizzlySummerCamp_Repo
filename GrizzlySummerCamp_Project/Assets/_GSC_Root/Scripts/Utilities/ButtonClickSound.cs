using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonClickSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clickSound;
    public List<Button> buttons;

    private void Start()
    {
        if (buttons.Count == 0)
        {
            Debug.LogWarning("no hay botones");
            return;
        }

        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnClick(button));
        }
    }

    void OnClick(Button button)
    {
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);
    }
}
