using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FishingController : MonoBehaviour
{
    //ESTE SCRIPT ADMINISTRA EL MOVIMIENTO DEL PEZ, EL WIN Y LOSE, LA GANANCIA Y PERDIDA DEL PROGRESO Y EL INICIO Y FIN DEL MINIJUEGO

    [Header("Movement Stats")]
    [SerializeField] Transform topPivot;
    [SerializeField] Transform bottomPivot;

    [Header("Fish Stats")]
    [SerializeField] Transform fish;
    float fishPosition;
    float fishDestination;
    [SerializeField] float fishTimer;
    [SerializeField] float timerMultiplicator = 3f;
    [SerializeField] float fishSpeed;
    [SerializeField] float smoothMotion = 1f;

    [Header("Hook Stats")]
    [SerializeField] Transform hook;
    float hookPosition;
    [SerializeField] float hookSize = 0.1f;
    [SerializeField] float hookPower;
    [SerializeField] float hookProgressLossSpeed;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.005f;
    [SerializeField] Transform progressBarContainer;

    [Header("Escape Bar")]
    [SerializeField] float failTimer;
    [SerializeField] Transform escapeBarContainer;
    float failTimerMax = 25f;

    [Header("Escape Bar Effects")]
    [SerializeField] float shakeThreshold = 0.2f;
    [SerializeField] float shakeIntensity = 3f;
    [SerializeField] float shakeSpeed = 25f;
    Vector3 escapeOriginalPos;

    [Header("UI References")]
    [SerializeField] GameObject fishingUI;
    [SerializeField] GameObject miniResultUI;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] float resultDisplayTime = 1f;

    bool isFishing = false;

    [Header("Sound Effects")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sonidoCaptura;
    [SerializeField] AudioClip sonidoEscape;

    Dificultad dificultadActual;

    void Start()
    {
        fishPosition = Random.Range(0f, 1f);
        fishDestination = fishPosition;

        if (escapeBarContainer != null)
            escapeOriginalPos = escapeBarContainer.localPosition;

        fishingUI.SetActive(false);
        miniResultUI.SetActive(false);
    }

    void Update()
    {
        if (!isFishing) return;

        Fish();
        Hook();
        ProgressCheck();
    }

    void ProgressCheck()
    {
        if (progressBarContainer != null)
        {
            Vector3 ls = progressBarContainer.localScale;
            ls.y = hookProgress;
            progressBarContainer.localScale = ls;
        }

        float min = hookPosition - hookSize / 1;
        float max = hookPosition + hookSize / 1;

        if (min < fishPosition && fishPosition < max)
            hookProgress = Mathf.MoveTowards(hookProgress, 1f, hookPower * Time.deltaTime);
        else
        {
            float dificultadMultiplicadorEscape =
                (dificultadActual == Dificultad.Facil) ? 1f :
                (dificultadActual == Dificultad.Normal) ? 2f : 4f;

            hookProgress = Mathf.Lerp(hookProgress, 0f, hookProgressLossSpeed * Time.deltaTime);
            failTimer -= Time.deltaTime;
            UpdateEscapeBar();

            if (failTimer < 0)
                Lose();
        }

        if (hookProgress >= 1f)
            Win();

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);
    }

    void Win()
    {
        isFishing = false;
        UpdateEscapeBar();
        fishingUI.SetActive(false);
        miniResultUI.SetActive(true);
        resultText.text = "Got it!";

        if (audioSource != null && sonidoCaptura != null)
            audioSource.PlayOneShot(sonidoCaptura);

        EventManager.OnFishCaught?.Invoke(dificultadActual);
        StartCoroutine(HideResultUIAfterDelay());
    }

    void Lose()
    {
        isFishing = false;
        UpdateEscapeBar();
        fishingUI.SetActive(false);
        miniResultUI.SetActive(true);
        resultText.text = "It escaped :(";

        if (audioSource != null && sonidoEscape != null)
            audioSource.PlayOneShot(sonidoEscape);

        EventManager.OnFishEscaped?.Invoke();
        StartCoroutine(HideResultUIAfterDelay());
    }

    IEnumerator HideResultUIAfterDelay()
    {
        yield return new WaitForSeconds(resultDisplayTime);
        miniResultUI.SetActive(false);
    }

    void Hook()
    {
        if (Input.GetMouseButton(0))
            hookPullVelocity += hookPullPower * Time.deltaTime;

        hookPullVelocity -= hookGravityPower * Time.deltaTime;
        hookPosition += hookPullVelocity;

        if (hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f)
            hookPullVelocity = 0f;

        if (hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f)
            hookPullVelocity = 0f;

        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);

        if (hook != null)
            hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }

    void UpdateEscapeBar()
    {
        if (escapeBarContainer == null) return;

        float t = 1f - (failTimer / failTimerMax);
        t = Mathf.Clamp01(t);

        Vector3 ls = escapeBarContainer.localScale;
        ls.y = t;
        escapeBarContainer.localScale = ls;

        if (t > shakeThreshold)
        {
            float shake = Mathf.Sin(Time.time * shakeSpeed) * shakeIntensity;
            escapeBarContainer.localPosition = escapeOriginalPos + new Vector3(shake, 0, 0);
        }
        else
        {
            escapeBarContainer.localPosition = escapeOriginalPos;
        }
    }

    void Fish()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0)
        {
            fishTimer = Random.value * timerMultiplicator;
            fishDestination = Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        if (fish != null)
            fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }

    public void StartFishing()
    {
        isFishing = true;
        fishingUI.SetActive(true);
        miniResultUI.SetActive(false);

        hookProgress = 0f;
        failTimer = 25f;
        failTimerMax = failTimer;
        fishPosition = Random.Range(0f, 1f);
        fishDestination = fishPosition;

        SeleccionarDificultadAleatoria();
    }

    void SeleccionarDificultadAleatoria()
    {
        dificultadActual = (Dificultad)Random.Range(0, 3);

        switch (dificultadActual)
        {
            case Dificultad.Facil:
                hookPower = 0.09f;
                hookProgressLossSpeed = 0.03f;
                break;
            case Dificultad.Normal:
                hookPower = 0.07f;
                hookProgressLossSpeed = 0.05f;
                break;
            case Dificultad.Dificil:
                hookPower = 0.03f;
                hookProgressLossSpeed = 0.07f;
                break;
        }
    }
}
