using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEditor.MemoryProfiler;

public class Pesca_Prueba : MonoBehaviour
{
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
    //[SerializeField] float shakeIntensityIncrease = 0.1f;
    [SerializeField] float shakeSpeed = 25f;

    Vector3 escapeOriginalPos;

    [Header("UI References")]
    [SerializeField] GameObject fishingUI;
    [SerializeField] GameObject miniResultUI;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] float resultDisplayTime = 1f;

    [Header("Final Result UI")]
    [SerializeField] GameObject finalUI;
    [SerializeField] TextMeshProUGUI mensajeFinalText;

    [Header("Instructions UI")]
    [SerializeField] GameObject instruccionesUI;

    [Header("Fishing Time Limit")]
    [SerializeField] Temporizador temporizador;
    [SerializeField] float fishingTimer;

    [Header("Score System")]
    [SerializeField] TextMeshProUGUI puntosTexto;
    [SerializeField] Image insigniaImage;
    [SerializeField] Sprite insigniaBronce;
    [SerializeField] Sprite insigniaPlata;
    [SerializeField] Sprite insigniaOro;
    int puntos = 0;

    bool minijuegoTerminado = false;
    bool isFishing = false;

    [Header("Sound Effects")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip sonidoCaptura;
    [SerializeField] AudioClip sonidoEscape;


    //prueba dificultad
    enum Dificultad { Facil, Normal, Dificil }
    Dificultad dificultadActual;


    void Start()
    {
        Time.timeScale = 0f;
        if (temporizador == null) 
            temporizador = FindFirstObjectByType<Temporizador>();

        fishPosition = Random.Range(0f, 1f);
        fishDestination = fishPosition;
        fishingTimer = 120f;

        if (instruccionesUI != null)
            instruccionesUI.SetActive(true);

        if (escapeBarContainer != null) 
        {
            escapeOriginalPos = escapeBarContainer.localPosition;
        }

        fishingUI.SetActive(false);
        miniResultUI.SetActive(false);
        finalUI.SetActive(false);

        ControlarMovimientoBarco();

    }
    void Update()
    {
        if (Time.timeScale > 0)
            ControlarMovimientoBarco();

        if (minijuegoTerminado) return;

        if (fishingTimer > 0f)
            fishingTimer -= Time.deltaTime;
        else
        {
            minijuegoTerminado = true;
            MostrarResultadoFinal();
            return;
        }

        if (!isFishing) return;

        Fish();
        Hook();
        ProgressCheck();
    }

    void ControlarMovimientoBarco()
    {
        var boat = FindFirstObjectByType<BoatMovement>();
        var pauseGame = FindFirstObjectByType<PauseGame>();

        bool hayUIBloqueante =
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
        else if (!hayUIBloqueante)
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

    void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null) { audioSource.PlayOneShot(clip); }
    }

    void ProgressCheck()
    {
        Vector3 ls = progressBarContainer.localScale;
        ls.y = hookProgress;
        progressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 1;
        float max = hookPosition + hookSize / 1;

        if (min < fishPosition && fishPosition < max)
            hookProgress = Mathf.MoveTowards(hookProgress, 1f, hookPower * Time.deltaTime);
        else
        {

            //modificacion tiempo que tarda en escapar segund su dificultad
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
        failTimer = failTimerMax;
        UpdateEscapeBar();
        fishingUI.SetActive(false);
        miniResultUI.SetActive(true);
        resultText.text = "Got it!";

        ReproducirSonido(sonidoCaptura);

        int puntosGanados = PuntosPorDificultad();
        puntos += puntosGanados;

        puntosTexto.text = "Points: " + puntos;
        ActualizarInsignia();

        StartCoroutine(HideResultUIAfterDelay());
    }
    int PuntosPorDificultad()
    {
        switch (dificultadActual)
        {
            case Dificultad.Facil:
                return 50;
            case Dificultad.Normal:
                return 100;
            case Dificultad.Dificil:
                return 200;
            default: return 100;
        }
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

        //Vibración al llegar al 50% del threshold para fallar 
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

        //velocidad segun dificultad
        float dificultadMultiplicadorPez =
            (dificultadActual == Dificultad.Facil) ? 1.8f :
            (dificultadActual == Dificultad.Normal) ? 1f : 0.5f;

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
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
        //UpdateEscapeBar();
        fishPosition = Random.Range(0f, 1f);
        fishDestination = fishPosition;

        SeleccionarDificultadAleatoria();
        ControlarMovimientoBarco();
    }
    void SeleccionarDificultadAleatoria()
    {
        dificultadActual = (Dificultad)Random.Range(0, 3);
        Debug.Log("Dificultad seleccionada:" + dificultadActual);

        //parametros base
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
                hookProgressLossSpeed = 0.1f;
                break;
        }
    }
    void ActualizarInsignia()
    {

        if (insigniaImage == null) return;

        if (puntos >= 300)
            insigniaImage.sprite = insigniaOro;
        else if (puntos >= 200)
            insigniaImage.sprite = insigniaPlata;
        else if (puntos >= 100)
            insigniaImage.sprite = insigniaBronce;
        else
            insigniaImage.sprite = null;
    }
    int ObtenerNivelMedalla() 
    {
        if (puntos >= 300) return 3;
        if (puntos >= 200) return 2;
        if (puntos >= 100) return 1;
        return 0;
    }
    void MostrarResultadoFinal()
    {
        isFishing = false;
        fishingUI.SetActive(false);
        miniResultUI.SetActive(false);
        finalUI.SetActive(true);

        string mensaje;
        if (puntos >= 300)
            mensaje = "!Wow!";
        else if (puntos >= 200)
            mensaje = "Incredible!";
        else if (puntos >= 100)
            mensaje = "Well done";
        else if (puntos == 0)
            mensaje = "Oops :(";
        else
            mensaje = "Oops :(";

        if (mensajeFinalText != null)
            mensajeFinalText.text = mensaje;

        // guardar la insignia guardada si se termina el tiempo
        int nivel = ObtenerNivelMedalla();
        if (InsigniaManager.Instance != null)
            InsigniaManager.Instance.GuardarInsignia(nivel);
    }
    public void EmpezarJuego() 
    {
        instruccionesUI.SetActive(false);
        Time.timeScale = 1f;
        if (temporizador != null)
            temporizador.ActivarTemporizador();

        var boat = FindFirstObjectByType<BoatMovement>();
        if (boat != null)
            boat.canMove = true;
    }
}
