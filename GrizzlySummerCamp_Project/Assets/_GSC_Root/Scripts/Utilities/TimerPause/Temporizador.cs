using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Temporizador : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text timeText;

    [Header("Opcional")]
    [SerializeField] private GameObject loadingUI;

    private float maxTime;

    private void Start()
    {
        if (MinigameManager.Instance != null)
        {
            maxTime = MinigameManager.Instance.GetMaxTime();
            slider.maxValue = maxTime;
        }
    }

    private void Update()
    {

        if (loadingUI != null && loadingUI.activeSelf) return;

        if (MinigameManager.Instance == null) return;

        float currentTime = MinigameManager.Instance.GetCurrentTime();

        slider.value = currentTime;

        if (timeText != null)
            timeText.text = Mathf.CeilToInt(currentTime).ToString();
    }
}
