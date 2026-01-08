using UnityEngine;
using System.Collections.Generic;
public class BalloonSpawner : MonoBehaviour
{

    [Header("Prefab globo")]
    public GameObject balloonPrefab;

    [Header("Recorridos disponibles")]
    public List<BalloonPath> balloonPaths = new List<BalloonPath>();

    [Header("Cantidad maxima globos")]
    public int maxBalloons = 5;

    [Header("Respawn")]
    public float respawnDelay = 0.5f;

    private List<BalloonPath> usedPaths = new List<BalloonPath>();

    private int currentBalloons = 0;

    void Start()
    {
        SpawnInitialBalloons();
    }

    void SpawnInitialBalloons()
    {
        for (int i = 0; i < maxBalloons; i++)
            SpawnSingleBalloon();
    }

    void SpawnSingleBalloon()
    {
        if (currentBalloons >= maxBalloons) return;

        BalloonPath path = GetRandomPath();
        if (path == null) return;

        Transform[] route = path.GetRandomRoute();
        if (route == null || route.Length == 0) return;

        GameObject balloon = Instantiate(balloonPrefab, path.spawnPoint.position, path.spawnPoint.rotation);

        BalloonMovement movement = balloon.GetComponent<BalloonMovement>();
        movement.Initialize(path, route, this);

        usedPaths.Add(path);
        currentBalloons++;
    }

    public void OnBalloonDestroyed(BalloonPath path)
    {
        if (usedPaths.Contains(path))
            usedPaths.Remove(path);

        currentBalloons--;

        Invoke(nameof(SpawnSingleBalloon), respawnDelay);
    }

    BalloonPath GetRandomPath()
    {
        List<BalloonPath> available = new List<BalloonPath>(balloonPaths);
        available.RemoveAll(p => usedPaths.Contains(p));

        if (available.Count == 0)
            available = balloonPaths;

        return available[Random.Range(0, available.Count)];
    }
}