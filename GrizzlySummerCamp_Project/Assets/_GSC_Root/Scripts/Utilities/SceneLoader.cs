using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Lo ponemos en todo lo que cambie de escena(creoqyaesta)

    public void CambiarEscena(int index)
    {
        if (HubPlayerSpawner.Instance != null)
        {
            HubPlayerSpawner.Instance.SavePosition();
        }

        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}