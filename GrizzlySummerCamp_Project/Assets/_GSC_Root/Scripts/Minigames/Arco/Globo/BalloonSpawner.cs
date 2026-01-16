using UnityEngine;
using System.Collections.Generic;

public class BalloonSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject goodBalloonPrefab;
    public GameObject badBalloonPrefab;
    public GameObject rareBalloonPrefab;

    [Header("Probabilidades (%)")]
    [Range(0, 100)] public int goodBalloonChance = 70;
    [Range(0, 100)] public int badBalloonChance = 25;
    [Range(0, 100)] public int rareBalloonChance = 5;

    [Header("Recorridos disponibles")]
    public List<BalloonPath> balloonPaths = new List<BalloonPath>();

    [Header("Cantidad maxima globos")]
    public int maxBalloons = 5;

    [Header("Respawn")]
    public float respawnDelay = 0.5f;

    private List<BalloonRouteInstance> usedRoutes = new List<BalloonRouteInstance>();
    private int currentBalloons = 0;

    void Start()
    {
        /*
        for (int i = 0; i < maxBalloons; i++)
            SpawnSingleBalloon();
        */
    }

    void SpawnSingleBalloon()
    {
        if (currentBalloons >= maxBalloons) return;

        BalloonRouteInstance routeInstance = GetRandomFreeRoute();
        if (routeInstance == null) return;

        GameObject prefab = GetBalloonPrefab();

        GameObject balloon = Instantiate(
            prefab,
            routeInstance.path.spawnPoint.position,
            routeInstance.path.spawnPoint.rotation
        );

        BalloonMovement movement = balloon.GetComponent<BalloonMovement>();
        movement.Initialize(routeInstance.path, routeInstance.route.waypoints, this);
        movement.SetRouteInstance(routeInstance);

        usedRoutes.Add(routeInstance);
        currentBalloons++;
    }

    GameObject GetBalloonPrefab()
    {
        int roll = Random.Range(0, 100);

        if (roll < rareBalloonChance)
            return rareBalloonPrefab;

        if (roll < rareBalloonChance + goodBalloonChance)
            return goodBalloonPrefab;

        return badBalloonPrefab;
    }

    BalloonRouteInstance GetRandomFreeRoute()
    {
        List<BalloonRouteInstance> available = new List<BalloonRouteInstance>();

        foreach (BalloonPath path in balloonPaths)
        {
            foreach (BalloonRoute route in path.routes)
            {
                bool inUse = usedRoutes.Exists(r => r.route == route);

                if (!inUse)
                    available.Add(new BalloonRouteInstance(path, route));
            }
        }

        if (available.Count == 0)
            return null;

        return available[Random.Range(0, available.Count)];
    }

    public void OnBalloonDestroyed(BalloonRouteInstance routeInstance)
    {
        usedRoutes.Remove(routeInstance);
        currentBalloons--;

        Invoke(nameof(SpawnSingleBalloon), respawnDelay);
    }
    private void OnEnable()
    {
        MinigameManager.OnMinigameStarted += StartSpawning;
    }

    private void OnDisable()
    {
        MinigameManager.OnMinigameStarted -= StartSpawning;
    }

    void StartSpawning()
    {
        usedRoutes.Clear();
        currentBalloons = 0;

        for (int i = 0; i < maxBalloons; i++)
            SpawnSingleBalloon();
    }
}