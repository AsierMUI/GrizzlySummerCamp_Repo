using UnityEngine;

public class UIButtonSceneLoader : MonoBehaviour
{
    public void LoadSceneByIndex(int index)
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadSceneByIndex(index);
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadSceneByName(sceneName);
        }
    }

    public void QuitGame()
    {
       if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.QuitGame();
        }
    }
}