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

    private List<BalloonPath> usedPaths = new List<BalloonPath>();

    void Start()
    {
        SpawnBalloons();
    }

    void SpawnBalloons()
    {
        int toSpawn = Mathf.Min(maxBalloons, balloonPaths.Count);
        usedPaths.Clear();

        for (int i = 0; i < toSpawn; i++)
        {
            SpawnSingleBalloon();
        }
    }

    void SpawnSingleBalloon()
    {
        BalloonPath path = GetRandomUnusedPath();
        if (path == null) return;

        GameObject balloon = Instantiate(balloonPrefab, path.spawnPoint.position, path.spawnPoint.rotation);

        BalloonMovement movement = balloon.GetComponent<BalloonMovement>();
        movement.SetWaypoints(path.waypoints);

        usedPaths.Add(path);
    }

    BalloonPath GetRandomUnusedPath()
    {
        List<BalloonPath> available = new List<BalloonPath>(balloonPaths);
        available.RemoveAll(p => usedPaths.Contains(p));

        if (available.Count == 0) return null;

        return available[Random.Range(0, available.Count)];
    }
}