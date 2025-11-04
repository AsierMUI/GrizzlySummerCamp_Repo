using UnityEngine;
using System.Collections.Generic;

public class FishingZoneSpawner : MonoBehaviour
{
    [Header("Prefab zona pesca")]
    public GameObject fishingZonePrefab;

    [Header("puntos de spawn")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Header("Numero de zonas de pesca")]
    public int numberOfZones = 5;

    private List<Transform> usedPoints = new List<Transform>(); 

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
            Transform spawnPoint = GetRandomUnusedSpawnPoint();
            if (spawnPoint == null)
                return;

            GameObject zone = Instantiate(fishingZonePrefab, spawnPoint.position, spawnPoint.rotation);
            zone.name = "FishingZone_" + (i + 1);

            usedPoints.Add(spawnPoint);
        }
    }

    Transform GetRandomUnusedSpawnPoint()
    {
        List<Transform> avaliablePoints = new List<Transform>(spawnPoints);
        avaliablePoints.RemoveAll(point => usedPoints.Contains(point));

        if (avaliablePoints.Count == 0)
            return null;

        int randomIndex = Random.Range(0, avaliablePoints.Count);
        return avaliablePoints[randomIndex];
    }
}
