using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FMControler : MonoBehaviour
{
    [Header("Animator")]
    [SerializeField] Animator niñaAnimator;

    [Header("Modules")]
    [SerializeField] FishMovement fish;
    [SerializeField] HookController hook;
    [SerializeField] ProgressSystem progress;
    [SerializeField] FishingUIController ui;
    [SerializeField] FishingAudio audioFX;

    [Header("Timer")]
    [SerializeField] Temporizador temporizador;

    [Header("General UI")]
    [SerializeField] GameObject loadingUI;

    bool isFishing = false;
    bool minijuegoTerminado = false;

    int puntos = 0;
    int escenaIndex;

    private void Start()
    {
        Time.timeScale = 0f;
        if (temporizador == null)
            temporizador = FindFirstObjectByType<Temporizador>();

        escenaIndex = SceneManager.GetActiveScene().buildIndex;

        if (loadingUI) loadingUI.SetActive(true);
        ui.HideFishingUI();
        ui.HideResult();
    }

    private void Update()
    {
        if (minijuegoTerminado || !isFishing || Time.timeScale == 0f)
        return;

        if (niñaAnimator != null)
            niñaAnimator.SetBool("isFishing", true);

        fish.Tick();
        hook.Tick();
        progress.Tick(hook.hookPosition, hook.hookSize, fish.fishPosition);
    }

    public void StartFishing()
    {
        isFishing = true;
        ui.ShowFishingUI();
        ui.HideResult();

        progress.Init();
        hook.ResetHook();

        fish.fishPosition = Random.Range(0f, 1f);
        fish.fishDestination = fish.fishPosition;

        SeleccionarDificultadAleatoria();

        if (niñaAnimator != null)
            niñaAnimator.SetTrigger("cast");

        AudioManager.Current?.PlaySFX("Splash");
    }

    void Win()
    {
        isFishing = false;

        ui.HideFishingUI();
        ui.ShowResult("Got it!");

        puntos += PuntosPorDificultad();
        ui.UpdatePoints(puntos);

        audioFX.PlayCapture();

        StartCoroutine(HideResult());
    }

    void Lose()
    {
        isFishing = false;

        ui.HideFishingUI();
        ui.ShowResult("It escaped :(");

        audioFX.PlayEscape();

        StartCoroutine(HideResult());
    }

    IEnumerator HideResult()
    {
        yield return new WaitForSeconds(1f);
        ui.HideResult();
    }

    void SeleccionarDificultadAleatoria()
    {
        float r = Random.value;

        if (r < 0.4f) fish.dificultadActual = FishMovement.Dificultad.Facil;
        else if (r < 0.8f) fish.dificultadActual = FishMovement.Dificultad.Normal;
        else fish.dificultadActual = FishMovement.Dificultad.Dificil;

        switch (fish.dificultadActual)
        {
            case FishMovement.Dificultad.Facil:
                progress.hookPower = 0.09f;
                progress.hookProgressLossSpeed = 0.03f;
                ui.UpdateDificulty("Easy");
                break;

            case FishMovement.Dificultad.Normal:
                progress.hookPower = 0.07f;
                progress.hookProgressLossSpeed = 0.05f;
                ui.UpdateDificulty("Normal");
                break;

            case FishMovement.Dificultad.Dificil:
                progress.hookPower = 0.03f;
                progress.hookProgressLossSpeed = 0.007f;
                ui.UpdateDificulty("Hard");
                break;
        }
    }

    int PuntosPorDificultad()
    {
        switch (fish.dificultadActual)
        {
            case FishMovement.Dificultad.Facil: return 50;
            case FishMovement.Dificultad.Normal: return 100;
            case FishMovement.Dificultad.Dificil: return 200;
        }
        return 100;
    }

    public void FinalizarPorTiempo()
    {
        if (minijuegoTerminado) return;

        minijuegoTerminado = true;
        isFishing = false;

        ui.HideFishingUI();
        ui.HideResult();

        string mensaje;
        if (puntos >= 300) mensaje = "Wow!";
        else if (puntos >= 200) mensaje = "Incredible!";
        else if (puntos >= 100) mensaje = "Well done!";
        else mensaje = "Oops :(";

        ui.ShowFinal(mensaje);
        int medalla = (puntos >= 300) ? 3 :
                      (puntos >= 200) ? 2 :
                      (puntos >= 100) ? 1 : 0;
        InsigniaManager.Instance?.GuardarInsignia(medalla);
    }

    public void EmpezarJuego()
    {
        loadingUI.SetActive(false);
        Time.timeScale = 1f;

        temporizador?.ActivarTemporizador();
    }

}
