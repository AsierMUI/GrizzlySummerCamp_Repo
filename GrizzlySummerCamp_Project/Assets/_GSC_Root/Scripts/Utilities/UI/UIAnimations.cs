using UnityEngine;
using System.Collections;

public class UIAnimations : MonoBehaviour
{
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject inicio;
    [SerializeField] private GameObject libreta;
    [SerializeField] string key;

    [Header("Manecilla tiempo")]
    [SerializeField] private GameObject LoadingUI;
    [SerializeField] private Animator animManecilla;
    [SerializeField] private string tiempoAnim;
    private bool animacionTiempoIniciada = false;

    private bool isLibretaActive = true;
    private bool AnimaciónActiva = false;

    public AudioManager audioManager;

    private void Awake()
    {
        if (audioManager == null)
            audioManager = FindFirstObjectByType<AudioManager>();
        if(animManecilla != null)
            animManecilla.enabled = false;
    }

    private void Start()
    {
        if (logo !=null)
        {
            LeanTween.moveY(logo.GetComponent<RectTransform>(), 0, 1.5f).setDelay(1f) //animacion del logo, con delay al empezar y su duracion
                .setEase(LeanTweenType.easeOutBounce).setOnComplete(BajarAlpha); //set on complete llama a la funcion bajaralpha al acabar la animacion del logo
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            ToggleLibreta();
        }
    }

    //Funciones libreta

    public void ToggleLibreta() 
    {
        if (AnimaciónActiva) return;

        AnimaciónActiva=true;

        if (!string.IsNullOrEmpty(key))
            audioManager?.PlaySFX(key);

        OcultaInstrucciones.Instance.OcultarInstrucciones();
        CambiarLibreta();
    }

    private void BajarAlpha()
    {
        LeanTween.alpha(inicio.GetComponent<RectTransform>(), 0f, 1f).setDelay(0.5f);
        inicio.GetComponent<CanvasGroup>().blocksRaycasts = false; //blockea la interaccion con los demas elementos del canva
    }

    public void CambiarLibreta()
    {
        if (isLibretaActive)
        {
            DesactivarLibreta();
        }
        else
        {
            ActivarLibreta();
        }
        isLibretaActive = !isLibretaActive; //cambia el estado al inverso
    }

    public void ActivarLibreta()
    {
        LeanTween.moveY(libreta.GetComponent<RectTransform>(), 0, 1f).setEase(LeanTweenType.easeOutSine).setOnComplete(() => AnimaciónActiva = false);
    }

    public void DesactivarLibreta()
    {
        LeanTween.moveY(libreta.GetComponent<RectTransform>(), -609, 1f).setEase(LeanTweenType.easeOutSine).setOnComplete(() => AnimaciónActiva = false);
    }

    //Funciones tiempo

    public void IniciarAnimacionManecilla()
    {
        if (!animacionTiempoIniciada)
            Debug.Log("Entra coroutine");
            StartCoroutine(EsperarYAnimarManecilla());
    }
    private IEnumerator EsperarYAnimarManecilla()
    {
        while (LoadingUI != null && LoadingUI.activeSelf)
            yield return null;

        animacionTiempoIniciada = true;
        if(animManecilla !=null)
        {
            Debug.Log("entra tiempo");
            animManecilla.enabled = true;

            yield return null;

            animManecilla.Play(tiempoAnim, 0, 0f);
        }
    }
}