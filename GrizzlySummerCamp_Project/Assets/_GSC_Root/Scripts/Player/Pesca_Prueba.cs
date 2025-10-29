using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

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
    [SerializeField] float hookPower = 0.3f;
    [SerializeField] float hookProgressLossSpeed = 0.1f;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] float hookPullPower = 0.01f;
    [SerializeField] float hookGravityPower = 0.005f;
    [SerializeField] Transform progressBarContainer;
    [SerializeField] float failTimer;

    [Header("UI References")]
    [SerializeField] GameObject fishingUI;
    [SerializeField] GameObject resultUI;
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] float resultDisplayTime = 3f;

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

    [SerializeField] TextMeshProUGUI mensajeFinalText;

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

        fishingUI.SetActive(false);
        resultUI.SetActive(false);

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

        bool hayUIBloqueante =
            (instruccionesUI != null && instruccionesUI.activeSelf) ||
            (fishingUI != null && fishingUI.activeSelf) ||
            (resultUI != null && resultUI.activeSelf) ||
            (temporizador != null && temporizador.gameObject.activeInHierarchy &&
            TieneCanvasResultadoActivo());

        if (hayUIBloqueante)
        {
            boat.canMove = false;
            boat.ResetVelocity();
        }
        else
        {
            boat.canMove = true;
        }
    }

    bool TieneCanvasResultadoActivo()
    {
        return temporizador != null &&
            temporizador.GetCanvasResultado() != null &&
            temporizador.GetCanvasResultado().activeSelf;
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
            hookProgress = Mathf.Lerp(hookProgress, 0f, hookProgressLossSpeed * Time.deltaTime);
            failTimer -= Time.deltaTime;
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
        fishingUI.SetActive(false);
        resultUI.SetActive(true);
        resultText.text = "Got it!";

        puntos += 100;
        puntosTexto.text = "Points: " + puntos;
        ActualizarInsignia();

        StartCoroutine(HideResultUIAfterDelay());
    }

    void Lose()
    {
        isFishing = false;
        fishingUI.SetActive(false);
        resultUI.SetActive(true);
        resultText.text = "It escaped :(";

        StartCoroutine(HideResultUIAfterDelay());
    }

    IEnumerator HideResultUIAfterDelay()
    {
        yield return new WaitForSeconds(resultDisplayTime);
        resultUI.SetActive(false);
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

    public void StartFishing()
    {
        isFishing = true;

        fishingUI.SetActive(true);
        resultUI.SetActive(false);

        hookProgress = 0f;
        failTimer = 25f;
        fishPosition = Random.Range(0f, 1f);
        fishDestination = fishPosition;

        ControlarMovimientoBarco();
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

    void MostrarResultadoFinal()
    {
        isFishing = false;
        fishingUI.SetActive(false);

        string mensaje =
            (puntos >= 300) ? "¡Wow!" :
            (puntos >= 200) ? "Incredible!" :
            (puntos >= 100) ? "Well done!" : "Oops :(";

        if (mensajeFinalText != null)
            mensajeFinalText.text = mensaje;
    }

    public void EmpezarJuego() 
    {
        instruccionesUI.SetActive(false);
        Time.timeScale = 1f;
        temporizador.IniciarTemporizador();

        var boat = FindFirstObjectByType<BoatMovement>();
        if (boat != null)
            boat.canMove = true;
    }

}
