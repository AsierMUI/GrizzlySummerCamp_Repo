using UnityEngine;

public class CinematicaHubVista : MonoBehaviour
{
    public static CinematicaHubVista Instance;

    [Header("Cinematica")]
    public bool cinematicaHubVista = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}