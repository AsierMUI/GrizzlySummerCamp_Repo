using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Video;
public class FadeInOnStart : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    IEnumerator Start()
    {
        fadeImage.color = new Color(0, 0, 0, 1);

        yield return null;

        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(1, 0, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        fadeImage.color = new Color(0, 0, 0, 0);
    }
}