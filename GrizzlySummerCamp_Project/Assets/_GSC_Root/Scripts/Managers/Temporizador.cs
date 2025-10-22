using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Temporizador : MonoBehaviour
{
    [SerializeField] private float tiempoMax;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject canvasResultado;
    [SerializeField] private TMP_Text resultadoTexto;
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
            MostrarResultados();
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

    public void MostrarResultados()
    {
        resultadoTexto.text = "Has conseguido:";
        canvasResultado.SetActive(true);
    }
}
