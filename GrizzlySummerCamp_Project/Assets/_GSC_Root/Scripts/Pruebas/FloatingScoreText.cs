using TMPro;
using UnityEngine;

public class FloatingScoreText : MonoBehaviour
{
    [Header("Movimiento")]
    public float floatSpeed = 1.5f;
    public float lifeTime = 1f;

    [Header("Fade")]
    public float fadeDuration = 0.5f;

    private TMP_Text text;
    private Color startColor;
    private float timer;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
        startColor = text.color;
    }

    public void SetText(int score)
    {
        text.text = "+" + score;
    }

    private void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        timer += Time.deltaTime;

        if (timer >= lifeTime - fadeDuration)
        {
            float t = (timer - (lifeTime - fadeDuration)) / fadeDuration;
            Color c = startColor;
            c.a = Mathf.Lerp(startColor.a, 0f, t);
            text.color = c;
        }

        if (timer >= lifeTime)
            Destroy(gameObject);
    }
}