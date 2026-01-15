using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class TrashSpawner : MonoBehaviour
{
    [Header("Trash Prefabs")]
    [SerializeField] List<GameObject> trashPrefabs;

    [Header("Spawn Points (empites)")]
    [SerializeField] List<Transform> spawnpoints;

    [Tooltip("El número máximo de basura que se spawneara.")]
    [Header("Max Spawn Nº")]
    [SerializeField] int maxTrashToSpawn = 5;


    private List<GameObject> spawnedTrash = new();

    public void SpawnAllTrash() 
    {
        ClearTrash();

        if (trashPrefabs.Count == 0 || spawnpoints.Count == 0) return;

        int spawnCount = Mathf.Min(maxTrashToSpawn, spawnpoints.Count);
        int maxPerType = Mathf.FloorToInt(spawnCount * 0.5f);

        Dictionary<TrashType, List<GameObject>> prefabsByType = new();

        foreach (var prefab in trashPrefabs) 
        {
            if (prefab == null) continue;

            var item = prefab.GetComponent<TrashItem>();
            if (item == null) continue;

            if (!prefabsByType.ContainsKey(item.trashType))
                prefabsByType[item.trashType] = new List<GameObject>();

            prefabsByType[item.trashType].Add(prefab);
        }

        List<Transform> freePoints = new(spawnpoints);
        Dictionary<TrashType, int> spawnedPerType = new();

        foreach (var kvp in prefabsByType) 
        {
            if (freePoints.Count == 0 || spawnedTrash.Count >= spawnCount)
                break;

            var prefabList = kvp.Value;
            if (prefabList.Count == 0) continue;

            SpawnTrash(
                prefabList[Random.Range(0, prefabList.Count)],
                kvp.Key,
                freePoints,
                spawnedPerType
            );
        }

        while (spawnedTrash.Count < spawnCount && freePoints.Count > 0) 
        {
            var validTypes = prefabsByType.Keys
                .Where(t => !spawnedPerType.ContainsKey(t) || spawnedPerType[t] < maxPerType)
                .ToList();

            if (validTypes.Count == 0)
                break;

            TrashType chosenType = validTypes[Random.Range(0, validTypes.Count)];
            var prefabList = prefabsByType[chosenType];

            SpawnTrash(
                prefabList[Random.Range(0, prefabList.Count)],
                chosenType,
                freePoints,
                spawnedPerType
            );
        }
    }
    void SpawnTrash(
        GameObject prefab,
        TrashType type,
        List<Transform> freePoints,
        Dictionary<TrashType, int> spawnedPerType
    )
    {
        int pointIndex = Random.Range(0, freePoints.Count);
        Transform point = freePoints[pointIndex];
        freePoints.RemoveAt(pointIndex);

        GameObject trash = Instantiate(prefab, point.position, point.rotation);
        spawnedTrash.Add(trash);

        if (!spawnedPerType.ContainsKey(type))
            spawnedPerType[type] = 0;

        spawnedPerType[type]++;
    }
    void ClearTrash() 
    {
        foreach (var t in spawnedTrash)
            if (t != null) Destroy(t);

        spawnedTrash.Clear();
    }
}
