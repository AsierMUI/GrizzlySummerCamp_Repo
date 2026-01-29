using UnityEngine;
using UnityEngine.Playables;

public class CinematicaHub : MonoBehaviour
{
    public PlayableDirector cinematica;
    public float duracionCinematica;
    float t;

    public GameObject cineObj;
    public GameObject gameplayObj;

    private void Start()
    {
        if (CinematicaHubVista.Instance.cinematicaHubVista)
        {
            cineObj.SetActive(false);
            gameplayObj.SetActive(true);
            Destroy(this);
            return;
        }

        duracionCinematica = (float)cinematica.duration;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t >= duracionCinematica)
        {
            FinalizarCinematica();
        }
    }

    void FinalizarCinematica()
    {
        CinematicaHubVista.Instance.cinematicaHubVista = true;

        cineObj.SetActive(false);
        gameplayObj.SetActive(true);
        Destroy(this);
    }
}