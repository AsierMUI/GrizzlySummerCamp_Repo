using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHubSaver : MonoBehaviour
{
    //Este script lo tiene solo el player

    private void Start()
    {
        if (HubPlayerSpawner.Instance != null)
        {
            HubPlayerSpawner.Instance.RegisterPlayer(transform);
        }
    }
}