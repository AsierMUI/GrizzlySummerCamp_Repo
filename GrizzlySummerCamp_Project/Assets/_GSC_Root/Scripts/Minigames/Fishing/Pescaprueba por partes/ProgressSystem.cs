using UnityEngine;
using UnityEngine.UI;

public class ProgressSystem : MonoBehaviour
{
    [Header("References")]
    public Slider progressSlider;
    public Slider escapeSlider;

    [Header("Progress")]
    public float hookProgress;
    public float hookPower = 0.07f;
    public float hookProgressLossSpeed = 0.03f;

    [Header("Fail Timer")]
    public float failTimer = 20f;
    private float failTimerMax;

    [Header("Escape Shake")]
    public float shakeThreshold = 0.2f;
    public float shakeIntensity = 3f;
    public float shakeSpeed = 25f;
    private Vector3 escapeOriginalPos;

    public System.Action OnWin;
    public System.Action OnLose;

    public void Init()
    {
        hookProgress = 0;
        failTimerMax = failTimer;

        if (escapeSlider != null)
            escapeOriginalPos = escapeSlider.transform.localPosition;
    }

    public void Tick(float hookPos, float hookSize, float fishPos)
    {
        progressSlider.value = hookProgress;

        float min = hookPos - hookSize / 2;
        float max = hookPos + hookSize / 2;

        if (fishPos > min && fishPos < max)
        {
            hookProgress = Mathf.MoveTowards(hookProgress, 1f, hookPower * Time.deltaTime);
        }
        else
        {
            hookProgress -= hookProgressLossSpeed * Time.deltaTime;
            failTimer -= Time.deltaTime;
            UpdateEscapeBar();

            if (failTimer <= 0)
                OnLose?.Invoke();
        }

        hookProgress = Mathf.Clamp01(hookProgress);

        if (hookProgress >= 1f)
            OnWin?.Invoke();
    }

    private void UpdateEscapeBar()
    {
        if (escapeSlider == null) return;

        float t = 1f - (failTimer / failTimerMax);

        escapeSlider.value = t;

        if (t > shakeThreshold)
        {
            float shake = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
            escapeSlider.transform.localPosition = escapeOriginalPos + new Vector3(shake, 0, 0);
        }
        else
        {
            escapeSlider.transform.localPosition = escapeOriginalPos;
        }
    }
}
