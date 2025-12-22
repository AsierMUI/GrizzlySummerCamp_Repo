using UnityEngine;
using UnityEngine.UI;

public class CircularTimerUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider circularSlider;

    [Header("Refs")]
    [SerializeField] private MinigameManager minigameManager;

    private void Start()
    {
        if (minigameManager == null)
            minigameManager = MinigameManager.Instance;

        if (circularSlider == null)
            circularSlider = GetComponent<Slider>();

        circularSlider.minValue = 0f;
        circularSlider.maxValue = 1f;
        circularSlider.value = 1f;
    }

    private void Update()
    {
        if (minigameManager == null) return;

        float maxTime = minigameManager.GetMaxTime();
        float currentTime = minigameManager.GetCurrentTime();

        if (maxTime <= 0) return;

        float normalizedTime = Mathf.Clamp01(currentTime / maxTime);
        circularSlider.value = normalizedTime;
    }
}