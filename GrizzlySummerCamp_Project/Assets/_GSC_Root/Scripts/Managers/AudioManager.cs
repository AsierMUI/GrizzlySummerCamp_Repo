using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Current;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header("Audio Clip")]
    public AudioClip background;
    public AudioClip walking;
    public AudioClip fishing;
    public AudioClip boat;
    public AudioClip bonk;
    public AudioClip paper;


    private void Awake()
    {
        Current = this;
    }

    private void Start()
    {
        if (background != null) 
        {
            musicSource.clip = background;
            musicSource.Play();
        }
        /*
        musicSource.clip = background;
        musicSource.Play();
         */
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip!=null) return;
        SFXSource.PlayOneShot(clip);
    }

    public void PlaySFX(string clipName) 
    {
        AudioClip clip = GetClipByName(clipName);
        if (clip != null) 
        {
            SFXSource.PlayOneShot(clip);
        }
    }

    private AudioClip GetClipByName(string name) 
    {
        return name switch
        {
            "background" => background,
            "walking" => walking,
            "fishing" => fishing,
            "boat" => boat,
            "bonk" => bonk,
            "paper" => paper,
            _ => null
        };
    }
}
