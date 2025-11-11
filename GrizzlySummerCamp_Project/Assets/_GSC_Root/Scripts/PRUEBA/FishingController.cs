using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class FishingController : MonoBehaviour
{
    //ESTE SCRIPT ADMINISTRA EL MOVIMIENTO DEL PEZ, EL WIN Y LOSE, LA GANANCIA Y PERDIDA DEL PROGRESO Y EL INICIO Y FIN DEL MINIJUEGO

    [Header("Movement Stats")]
    public Transform topPivot;
    public Transform bottomPivot;

    [Header("Fish Stats")]
    public Transform fish;
    private float fishPosition;
    private float fishDestination;
    public float fishTimer;
    public float timerMultiplicator = 3f;
    private float fishSpeed;
    public float smoothMotion = 1f;

    [Header("Hook Stats")]
    public Transform hook;
    private float hookPosition;
    public float hookSize = 0.1f;
    private float hookProgress;
    private float hookPullVelocity;
    public float hookPullPower = 0.01f;
    public float hookGravityPower = 0.005f;
    public float hookPower;
    public float hookProgressLossSpeed;
    public Transform progressBarContainer;

    [Header("Escape Bar")]
    public Transform escapeBarContainer;
    private float failTimer;
    private float failTimerMax = 25f;
    public float shakeThreshold = 0.2f;
    public float shakeIntensity = 3f;
    public float shakeSpeed = 25f;
    private Vector3 escapeOriginalPos;

    [Header("UI References")]
    public GameObject instruccionesUI;
    public GameObject fishingUI;
    public GameObject miniResultUI;
    public TextMeshProUGUI resultText;
    public float resultDisplayTime = 1f;

    [Header("Final Result UI")]
    public GameObject finalUI;
    public TextMeshProUGUI mensajeFinalText;

    [Header("Fishing Time Limit")]
    public Temporizador temporizador;

    [Header("Score System")]
    public TextMeshProUGUI puntosTexto;
    public Image insigniaImage;
    public Sprite insigniaBronce;
    public Sprite insigniaPlata;
    public Sprite insigniaOro;
    private int puntos = 0;

    [Header("Sound Effects")]
    public AudioSource audioSource;
    public AudioClip sonidoCaptura;
    public AudioClip sonidoEscape;

    private bool isFishing = false;
    private bool minijuegoTerminado = false;

    private Dificultad dificultadActual;

    void Start()
    {
        Time.timeScale = 0f;

        if (temporizador == null) temporizador = FindFirstObjectByType<Temporizador>();

        fishPosition = Random.Range(0f, 1f);
        fishDestination = fishPosition;

        if (instruccionesUI != null)
            instruccionesUI.SetActive(true);

        fishingUI.SetActive(false);
        miniResultUI.SetActive(false);
        finalUI.SetActive(false);

        if (escapeBarContainer != null)
            escapeOriginalPos = escapeBarContainer.localPosition;

        ControlarMovimientoBarco();
    }

    void Update()
    {
        if (!isFishing) return;

        Fish();
        Hook();
        ProgressCheck();
    }

    // Esta función debe llamarse cuando el jugador entra en una zona de pesca
    public void InteractuarZonaPesca()
    {
        if (instruccionesUI != null)
            instruccionesUI.SetActive(false);

        StartFishing();
    }

    void StartFishing()
    {
        isFishing = true;

        fishingUI.SetActive(true);
        miniResultUI.SetActive(false);

        hookProgress = 0f;
        failTimer = failTimerMax;

        fishPosition = Random.Range(0f, 1f);
        fishDestination = fishPosition;

        SeleccionarDificultadAleatoria();

        ControlarMovimientoBarco();
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

    void Fish()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0)
        {
            fishTimer = Random.value * timerMultiplicator;
            fishDestination = Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(bottomPivot.position, topPivot.position, fishPosition);
    }

    void Hook()
    {
        if (Input.GetMouseButton(0))
            hookPullVelocity += hookPullPower * Time.deltaTime;

        hookPullVelocity -= hookGravityPower * Time.deltaTime;
        hookPosition += hookPullVelocity;

        if (hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f) hookPullVelocity = 0f;
        if (hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f) hookPullVelocity = 0f;

        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);
        hook.position = Vector3.Lerp(bottomPivot.position, topPivot.position, hookPosition);
    }

    void ProgressCheck()
    {
        Vector3 ls = progressBarContainer.localScale;
        ls.y = hookProgress;
        progressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if (min < fishPosition && fishPosition < max)
            hookProgress = Mathf.MoveTowards(hookProgress, 1f, hookPower * Time.deltaTime);
        else
        {
            hookProgress = Mathf.Lerp(hookProgress, 0f, hookProgressLossSpeed * Time.deltaTime);
            failTimer -= Time.deltaTime;
            UpdateEscapeBar();

            if (failTimer < 0)
                Lose();
        }

        if (hookProgress >= 1f)
            Win();

        hookProgress = Mathf.Clamp01(hookProgress);
    }

    void Win()
    {
        isFishing = false;
        failTimer = failTimerMax;
        UpdateEscapeBar();
        fishingUI.SetActive(false);
        miniResultUI.SetActive(true);
        resultText.text = "Got it!";
        ReproducirSonido(sonidoCaptura);
        puntos += PuntosPorDificultad();
        puntosTexto.text = "Points: " + puntos;
        StartCoroutine(HideResultUIAfterDelay());
    }

    void Lose()
    {
        isFishing = false;
        failTimer = failTimerMax;
        UpdateEscapeBar();
        fishingUI.SetActive(false);
        miniResultUI.SetActive(true);
        resultText.text = "It escaped :(";
        ReproducirSonido(sonidoEscape);
        StartCoroutine(HideResultUIAfterDelay());
    }

    int PuntosPorDificultad()
    {
        return dificultadActual switch
        {
            Dificultad.Facil => 50,
            Dificultad.Normal => 100,
            Dificultad.Dificil => 200,
            _ => 100
        };
    }
    IEnumerator HideResultUIAfterDelay()
    {
        yield return new WaitForSeconds(resultDisplayTime);
        miniResultUI.SetActive(false);
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

    void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    void ControlarMovimientoBarco()
    {
        var boat = FindFirstObjectByType<BoatMovement>();
        var pauseGame = FindFirstObjectByType<PauseGame>();

        bool hayUIActiva =
            (instruccionesUI != null && instruccionesUI.activeSelf) ||
            (fishingUI != null && fishingUI.activeSelf) ||
            (miniResultUI != null && miniResultUI.activeSelf) ||
            (finalUI != null && finalUI.activeSelf) ||
            (pauseGame != null && pauseGame.menuPausa != null && pauseGame.menuPausa.activeSelf) ||
            (temporizador != null && temporizador.gameObject.activeInHierarchy &&
            TieneCanvasResultadoActivo());

        if (pauseGame != null && pauseGame.juegoPausado)
        {
            boat.canMove = false;
            boat.ResetVelocity();
        }
        else if (!hayUIActiva)
        {
            boat.canMove = true;
        }
        else
        {
            boat.canMove = false;
            boat.ResetVelocity();
        }
    }

    bool TieneCanvasResultadoActivo()
    {
        return temporizador != null &&
               temporizador.GetCanvasResultado() != null &&
               temporizador.GetCanvasResultado().activeSelf;
    }
}
