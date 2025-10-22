using UnityEngine;
using UnityEngine.UI;

public class Temporizador : MonoBehaviour
{
    [SerializeField] private float tiempoMax;
   // [SerializeField] private GameObject[] sprites; //aqui ponemos los sprites de los numeros
    [SerializeField] private Slider slider;
    private float tiempoActual;
    private bool tiempoActivado = false;

    private void Start()
    {
        slider.maxValue = tiempoMax;
        slider.value = 0;
    }


    private void Update()
    {
        if (tiempoActivado)
        {
            CambiarContador();
        }
    }

    private void CambiarContador()
    {
        tiempoActual -= Time.deltaTime;

        if (tiempoActual >= 0)
        {
            slider.value = tiempoActual; //el slider cambia su valor conforme pasa el tiempo
        }

        if (tiempoActual <= 0)
        {
            CambiarTemporizador(false);
        }
    }

    private void CambiarTemporizador(bool estado)
    {
        tiempoActivado = estado;
    }
    public void ActivarTemporizador()
    {
        tiempoActual = tiempoMax;
        CambiarTemporizador(true);
    }

    public void DesactivarTemporizador()
    {
        CambiarTemporizador(false);
    }

    public void IniciarTemporizador()
    {
        if (!tiempoActivado)
        {
            ActivarTemporizador();
        }
    }

    //public void IniciarAnimacion()
   // {
       // foreach (GameObject sprite in sprites)
        //{
          //  sprite.SetActive(true); //los sprites tienen q estar inactivos al principio
          //  sprite.transform.localScale = Vector3.zero; //tiene el tamaño en 0
         //   LeanTween.scale(sprite, Vector3.one, 1f).setEase(LeanTweenType.easeOutBounce);
      // }
      //  LeanTween.delayedCall(0.5f, () => ActivarTemporizador());
    //}

}
