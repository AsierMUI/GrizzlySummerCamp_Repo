using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Temporizador : MonoBehaviour
{
    [SerializeField] private float tiempoMax;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject canvasResultado;
    [SerializeField] private TMP_Text resultadoTexto;
    //[SerializeField] private GameObject canvasInstrucciones;

    private float tiempoActual;
    private bool tiempoActivado = false;
    private bool tiempoFinalizado = false;

    [SerializeField] private Pesca_Prueba pescaScript;

    private void Start()
    {
        tiempoActual = tiempoMax;
        slider.maxValue = tiempoMax;
        slider.value = tiempoActual;

        if (pescaScript == null)
            pescaScript = FindFirstObjectByType<Pesca_Prueba>();

        //if (canvasInstrucciones !=null && canvasInstrucciones.activeSelf)
        //    tiempoActivado=false;
        //else
        tiempoActivado = true;
    }
    private void Update()
    {
        if (!tiempoActivado &&  !tiempoFinalizado /*&& canvasInstrucciones != null*/)
        {
            //if (!canvasInstrucciones.activeSelf)
            //{
            ActivarTemporizador();
            //}
        }

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
            slider.value = tiempoActual;
        }
        else 
        {
            tiempoActual = 0;
            slider.value = 0;

            tiempoActivado = false;
            tiempoFinalizado = true;
            MostrarResultados();
        }
    }
    public void ActivarTemporizador()
    {
        if (!tiempoFinalizado)
        {
            tiempoActual = tiempoMax;
            tiempoActivado = true;
        }
    }
    public void DesactivarTemporizador()
    {
        tiempoActivado = false;
    }
    public void MostrarResultados()
    {
        resultadoTexto.text = "You got:";
        canvasResultado.SetActive(true);

        if (pescaScript != null)
            pescaScript.FinalizarPorTiempo();

    }

    public GameObject GetCanvasResultado()
    {
        return canvasResultado;
    }
}
