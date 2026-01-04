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

    void Start()
    {
        SpawnInitialBalloons();
    }

    void SpawnInitialBalloons()
    {
        int toSpawn = Mathf.Min(maxBalloons, balloonPaths.Count);

        for (int i = 0; i < toSpawn; i++)
            SpawnSingleBalloon();
    }

    void SpawnSingleBalloon()
    {
        BalloonPath path = GetRandomUnusedPath();
        if (path == null) return;

        GameObject balloon = Instantiate(balloonPrefab, path.spawnPoint.position, path.spawnPoint.rotation);

        BalloonMovement movement = balloon.GetComponent<BalloonMovement>();
        movement.Initialize(path, this);

        usedPaths.Add(path);
    }

    public void OnBalloonDestroyed(BalloonPath path)
    {
        if (usedPaths.Contains(path))
            usedPaths.Remove(path);

        Invoke(nameof(SpawnSingleBalloon), respawnDelay);
    }

    BalloonPath GetRandomUnusedPath()
    {
        List<BalloonPath> available = new List<BalloonPath>(balloonPaths);
        available.RemoveAll(p => usedPaths.Contains(p));

        if (available.Count == 0) return null;

        return available[Random.Range(0, available.Count)];
    }
}