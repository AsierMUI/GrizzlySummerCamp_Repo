using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class IntroVideoController : MonoBehaviour
{
    [Header("Video")]
    public VideoPlayer videoPlayer;

    [Header("UI")]
    public GameObject skipButton;
    public Image fadeImage;

    [Header("Scene")]
    public string hubSceneName = "SCN_HUB";

    [Header("Fade")]
    public float fadeDuration = 1f;

    private bool skipButtonVisible = false;
    private bool isEnding = false;

    void Start()
    {
        skipButton.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 0);

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void Update()
    {
        if (!skipButtonVisible && Input.anyKeyDown)
        {
            ShowSkipButton();
        }
    }

    void ShowSkipButton()
    {
        skipButtonVisible = true;
        skipButton.SetActive(true);
    }

    public void SkipIntro()
    {
        if (isEnding) return;

        videoPlayer.Stop();
        EndIntro();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        EndIntro();
    }

    void EndIntro()
    {
        if (isEnding) return;
        isEnding = true;

        GameSession.introAlreadyPlayed = true;
        StartCoroutine(FadeOutAndLoad());
    }

    IEnumerator FadeOutAndLoad()
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(hubSceneName);
    }
}