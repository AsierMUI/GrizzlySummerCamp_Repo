using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawnManager : MonoBehaviour
{
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "SCN_HUB")
            return;

        if (PlayerData.hasSavedPosition)
        {
            transform.position = PlayerData.lastPosition;
        }
    }
}