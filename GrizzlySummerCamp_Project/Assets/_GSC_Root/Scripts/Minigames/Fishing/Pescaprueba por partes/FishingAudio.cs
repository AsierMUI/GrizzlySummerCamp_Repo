using UnityEngine;

public class FishingAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoCaptura;
    public AudioClip sonidoEscape;

    public void PlayCapture() => audioSource.PlayOneShot(sonidoCaptura);
    public void PlayEscape() => audioSource.PlayOneShot(sonidoEscape);
}