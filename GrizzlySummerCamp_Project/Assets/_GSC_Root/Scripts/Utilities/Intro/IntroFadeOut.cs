using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroFadeOut : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float blackTime = 2f;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        fadeImage.color = new Color(0, 0, 0, 1);
        fadeImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(blackTime);

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = 1f - (t / fadeDuration);
            fadeImage.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
    }
}