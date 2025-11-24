using UnityEngine;

public class BotonBorraImagen : MonoBehaviour
{
    public GameObject imageToDestroy;

    //Static hace que solo pase mientras este el juego abierto incluso entre las diferentes escenas, y al cerrar y volver a abrir el juego vuelve a aparecer
    private static bool imageDestroyedInSession = false;

    void Start()
    {
        if (imageDestroyedInSession)
            imageToDestroy.SetActive(false);
        else
            imageToDestroy.SetActive(true);
    }

    public void DestroyImage()
    {
        imageToDestroy.SetActive(false);
        imageDestroyedInSession = true;
    }
}
