using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishingZoneSpawner : MonoBehaviour
{
    public static FishingZoneSpawner instance;

    [Header("Prefab zona pesca")]
    public GameObject fishingZonePrefab;

    [Header("Puntos de spawn")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Numero de zonas de pesca")]
    public int numberOfZones = 5;

    [Header("Respawn")]
    public float respawnDelay = 20f;

    private List<Transform> usedPoints = new List<Transform>();

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnFishingZones();
    }

    void SpawnFishingZones()
    {
        int zonesToSpawn = Mathf.Min(numberOfZones, spawnPoints.Count);

        usedPoints.Clear();

        for (int i = 0; i < zonesToSpawn; i++)
        {
            SpawnAtRandomPoint();
        }
    }

    public void RespawnSingleZone(Transform usedPoint)
    {
        StartCoroutine(RespawnCoroutine(usedPoint));
    }

    private IEnumerator RespawnCoroutine(Transform usedPoint)
    {
        yield return new WaitForSeconds(respawnDelay);
        usedPoints.Remove(usedPoint);
        SpawnAtRandomPoint();
    }

    private void SpawnAtRandomPoint()
    {
        Transform spawnPoint = GetRandomUnusedSpawnPoint();
        if (spawnPoint == null) return;

        GameObject zone = Instantiate(fishingZonePrefab, spawnPoint.position, spawnPoint.rotation);

        zone.GetComponent<FishingZone>().SetSpawnPoint(spawnPoint);

        usedPoints.Add(spawnPoint);
    }

    Transform GetRandomUnusedSpawnPoint()
    {
        List<Transform> avaliablePoints = new List<Transform>(spawnPoints);
        avaliablePoints.RemoveAll(point => usedPoints.Contains(point));

        if (avaliablePoints.Count == 0)
        {
            usedPoints.Clear();
            avaliablePoints = new List<Transform>(spawnPoints);
        }

        int randomIndex = Random.Range(0, avaliablePoints.Count);
        return avaliablePoints[randomIndex];
    }
}
