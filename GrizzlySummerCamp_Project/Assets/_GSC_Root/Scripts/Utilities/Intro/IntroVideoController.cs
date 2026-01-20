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

    [Header("Skip UI")]
    public float hideSkipAfterSeconds = 3f;

    private bool skipButtonVisible = false;
    private bool isEnding = false;
    private float lastInputTime = 0f;

    void Start()
    {
        skipButton.SetActive(false);
        fadeImage.color = new Color(0, 0, 0, 0);

        videoPlayer.loopPointReached += OnVideoFinished;
        videoPlayer.Play();
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            ShowSkipButton();
            lastInputTime = Time.unscaledTime;
        }

        if (skipButtonVisible && Time.unscaledTime - lastInputTime > hideSkipAfterSeconds)
        {
            HideSkipButton();
        }
    }

    void ShowSkipButton()
    {
        skipButtonVisible = true;
        skipButton.SetActive(true);
    }

    void HideSkipButton()
    {
        skipButtonVisible = false;
        skipButton.SetActive(false);
    }

    public void SkipIntro()
    {
        if (isEnding) return;
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

        videoPlayer.Stop();
        SceneManager.LoadScene(hubSceneName);
    }
}