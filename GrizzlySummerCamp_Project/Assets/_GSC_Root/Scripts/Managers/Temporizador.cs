using UnityEngine;

public class Temporizador : MonoBehaviour
{
    [SerializeField] private float tiempoMax;
    [SerializeField] private GameObject[] sprites; //aqui ponemos los sprites de los numeros
    private float tiempoActual;
    private bool tiempoActivado = false;

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

    public void IniciarAnimacion()
    {
        foreach (GameObject sprite in sprites)
        {
            sprite.SetActive(true); //los sprites tienen q estar activos al principio
            sprite.transform.localScale = Vector3.zero; //tiene el tamaño en 0
            LeanTween.scale(sprite, Vector3.one, 1f).setEase(LeanTweenType.easeOutBounce);
        }
        LeanTween.delayedCall(0.5f, () => ActivarTemporizador());
    }

}
