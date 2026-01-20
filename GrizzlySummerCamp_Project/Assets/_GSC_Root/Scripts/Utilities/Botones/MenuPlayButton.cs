using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPlayButton : MonoBehaviour
{
    [Header("Scenes")]
    public string introSceneName = "SCN_Intro";
    public string hubSceneName = "SCN_HUB";

    public void OnPlayPressed()
    {
        if (!GameSession.introAlreadyPlayed)
        {
            SceneManager.LoadScene(introSceneName);
        }
        else
        {
            SceneManager.LoadScene(hubSceneName);
        }
    }
}