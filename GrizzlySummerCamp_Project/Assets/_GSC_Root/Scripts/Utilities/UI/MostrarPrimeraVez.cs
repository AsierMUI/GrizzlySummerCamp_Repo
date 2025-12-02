using UnityEngine;

public class MostrarPrimeraVez : MonoBehaviour
{
    [SerializeField] private GameObject contenedorObjeto; //objeto que se quiere ocultar despues de la primera vez en pantalla

    private static bool yaMostradoEnSesion = false;

    private void Awake()
    {
        bool debeMostrarse = !yaMostradoEnSesion;

        if (contenedorObjeto != null)
            contenedorObjeto.SetActive(debeMostrarse);

        if (debeMostrarse)
            yaMostradoEnSesion = true;

    }
}
