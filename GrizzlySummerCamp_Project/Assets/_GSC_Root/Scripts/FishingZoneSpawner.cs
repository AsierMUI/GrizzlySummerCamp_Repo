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

    }
}
