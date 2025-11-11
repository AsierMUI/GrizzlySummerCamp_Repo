using UnityEngine;
using TMPro;
public class MostrarTexto : MonoBehaviour
{
    [Header("Referencia al jugador")]
    [SerializeField] Transform jugador;

    [Header("Texto a mostrar")]
    [SerializeField] GameObject textoUI;           // Este debería ser un objeto UI que contiene el texto (puede estar desactivado al inicio)
    [SerializeField] float distanciaActivacion = 3f; // distancia desde el jugador para activar el texto

    bool textoActivo = false;

    void Start()
    {
        if (textoUI != null)
        {
            textoUI.SetActive(false);
        }
    }

    void Update()
    {
        if (jugador == null || textoUI == null) return;

        float dist = Vector3.Distance(jugador.position, transform.position);
        if (dist <= distanciaActivacion)
        {
            if (!textoActivo)
            {
                ActivarTexto();
            }
        }
        else
        {
            if (textoActivo)
            {
                DesactivarTexto();
            }
        }
    }

    void ActivarTexto()
    {
        textoUI.SetActive(true);
        textoActivo = true;
    }

    void DesactivarTexto()
    {
        textoUI.SetActive(false);
        textoActivo = false;
    }
}
