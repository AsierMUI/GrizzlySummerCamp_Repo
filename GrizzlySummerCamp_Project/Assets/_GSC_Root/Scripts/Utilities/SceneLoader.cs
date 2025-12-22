using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    //Persistente en persistent root

    public static SceneLoader Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void LoadSceneByIndex(int index)
    {
        CambiarEscena(index);
    }

    public void LoadSceneByName(string sceneName)
    {
        if (HubPlayerSpawner.Instance != null)
        {
            HubPlayerSpawner.Instance.SavePosition();
        }

        SceneManager.LoadScene(sceneName);
    }

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