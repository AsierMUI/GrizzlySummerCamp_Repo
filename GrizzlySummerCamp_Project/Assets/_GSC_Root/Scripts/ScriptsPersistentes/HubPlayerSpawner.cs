using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HubPlayerSpawner : MonoBehaviour
{
    //Script persistente en el persistent root

    public static HubPlayerSpawner Instance;

    [Header("Hub Settings")]
    public string hubSceneName = "SCN_HUB";

    private Vector3 savedPosition;
    private bool hasSavedPosition = false;

    private Transform player;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void RegisterPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void SavePosition()
    {
        if (player == null) return;

        savedPosition = player.position;
        hasSavedPosition = true;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        player = null;

        if (scene.name == hubSceneName && hasSavedPosition)
        {
            StartCoroutine(WaitForPlayerAndRestore());
        }
    }

    private IEnumerator WaitForPlayerAndRestore()
    {
        while (player == null)
        {
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.Sleep();
        }

        player.position = savedPosition;
    }
}