using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    public SceneLoader sceneLoader;

    public void CambiarEscena(int indice)
    {
        sceneLoader.CargarEscenaPorIndice(indice);
    }
}
