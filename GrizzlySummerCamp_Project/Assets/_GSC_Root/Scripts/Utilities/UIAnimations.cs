using UnityEngine;

public class UIAnimations : MonoBehaviour
{
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject inicio;
    [SerializeField] private GameObject libreta;
    [SerializeField] private GameObject instruccionesLibreta;

    private bool isLibretaActive = true;
    private static bool instruccionesDestruidasEnSesion = false;

    private void Start()
    {
        if (instruccionesLibreta !=null)
            instruccionesLibreta.SetActive(!instruccionesDestruidasEnSesion);
        if (logo!=null)
        {
            LeanTween.moveY(logo.GetComponent<RectTransform>(), 0, 1.5f).setDelay(1f) //animacion del logo, con delay al empezar y su duracion
                .setEase(LeanTweenType.easeOutBounce).setOnComplete(BajarAlpha); //set on complete llama a la funcion bajaralpha al acabar la animacion del logo
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (instruccionesLibreta !=null && instruccionesLibreta.activeSelf)
            {
                InstruccionesEscape();
            }

            CambiarLibreta();
        }
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
        LeanTween.moveY(libreta.GetComponent<RectTransform>(), 0, 1f).setEase(LeanTweenType.easeOutSine);
    }

    public void DesactivarLibreta()
    {
        LeanTween.moveY(libreta.GetComponent<RectTransform>(), -609, 1f).setEase(LeanTweenType.easeOutSine);
    }

    private void InstruccionesEscape()
    {
        instruccionesLibreta.SetActive(false);
        instruccionesDestruidasEnSesion = true;
    }
}