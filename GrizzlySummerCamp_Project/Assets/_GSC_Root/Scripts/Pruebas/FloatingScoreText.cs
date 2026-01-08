using TMPro;
using UnityEngine;

public class FloatingScoreText : MonoBehaviour
{
    [Header("Movimiento")]
    public float floatSpeed = 1.5f;
    public float lifeTime = 1f;

    [Header("Fade")]
    public float fadeDuration = 0.5f;

    [Header("Colores")]
    public Color positiveColor = Color.green;
    public Color negativeColor = Color.red;

    private TMP_Text text;
    private Color startColor;
    private float timer;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    public void SetText(int score)
    {
        text.text = score > 0 ? $"+{score}" : score.ToString();

        startColor = score >= 0 ? positiveColor : negativeColor;
        text.color = startColor;
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