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
        SaveHubPositionIfNeeded(sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void CambiarEscena(int index)
    {
        SaveHubPositionIfNeeded(SceneManager.GetSceneByBuildIndex(index).name);
        SceneManager.LoadScene(index);
    }

    private void SaveHubPositionIfNeeded(string nextScene)
    {
        if (HubPlayerSpawner.Instance == null) return;

        if (SceneManager.GetActiveScene().name == HubPlayerSpawner.Instance.hubSceneName
            && nextScene != HubPlayerSpawner.Instance.hubSceneName)
        {
            HubPlayerSpawner.Instance.SavePosition();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}