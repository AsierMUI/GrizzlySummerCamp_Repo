using UnityEngine;
using System.Collections;

public class UIAnimations : MonoBehaviour
{
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject inicio;
    [SerializeField] private GameObject libreta;
    [SerializeField] string key;

    [Header("Manecilla tiempo")]
    [SerializeField] private GameObject loadingUI;
    [SerializeField] private Animator animManecilla;
    [SerializeField] private string tiempoAnim;
    private bool animacionTiempoIniciada = false;

    //Nuevo bool
    private static bool libretaAbierta = true;
    
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
        //Nuevo
        if (libreta !=null)
        {
            AplicarEstadoLibreta();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (UIState.IsUIOpen) return;
            
            ToggleLibreta();
        }   
    }

    //Funciones libreta

    public void ToggleLibreta() 
    {
        if (AnimaciónActiva) return;
        if (UIState.IsUIOpen) return;

        AnimaciónActiva=true;

        if (!string.IsNullOrEmpty(key))
            audioManager?.PlaySFX(key);

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
        libretaAbierta = isLibretaActive;
    }

    public void ActivarLibreta()
    {
        LeanTween.moveY(libreta.GetComponent<RectTransform>(), 139, 1f).setEase(LeanTweenType.easeOutSine).setOnComplete(() => AnimaciónActiva = false);
    }

    public void DesactivarLibreta()
    {
        LeanTween.moveY(libreta.GetComponent<RectTransform>(), -121, 1f).setEase(LeanTweenType.easeOutSine).setOnComplete(() => AnimaciónActiva = false);
    }

    //Funciones tiempo

    public void IniciarAnimacionManecilla()
    {
        if (animacionTiempoIniciada) return;

        StartCoroutine(EsperarYAnimarManecilla());
    }
    private IEnumerator EsperarYAnimarManecilla()
    {
        while (loadingUI != null && loadingUI.activeSelf)
            yield return null;

        animacionTiempoIniciada = true;

        if (animManecilla != null)
        {
            animManecilla.enabled = true;

            yield return null;

            animManecilla.Play(tiempoAnim, 0, 0f);
        }
    }

    //Funcion nueva
    private void AplicarEstadoLibreta() 
    {
        RectTransform rt = libreta.GetComponent<RectTransform>();

        if (libretaAbierta)
        {
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, 139);
        }
        else
            rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, -121);

        isLibretaActive = libretaAbierta;
    }
}