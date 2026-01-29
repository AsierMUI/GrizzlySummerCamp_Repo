using UnityEngine;
using UnityEngine.Timeline;
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
        duracionCinematica = (float)cinematica.duration;
    }

    private void Update()
    {
        t += Time.deltaTime;

        if (t >= duracionCinematica)
        {
            cineObj.SetActive(false);
            gameplayObj.SetActive(true);
            Destroy(this);
        }
    }
}