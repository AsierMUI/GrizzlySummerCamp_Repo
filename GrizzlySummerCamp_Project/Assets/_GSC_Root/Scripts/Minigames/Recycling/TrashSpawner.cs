using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class TrashSpawner : MonoBehaviour
{
    [Header("Trash Prefabs")]
    [SerializeField] List<GameObject> trashPrefabs;

    [Header("Spawn Points (empites)")]
    [SerializeField] List<Transform> spawnpoints;

    //Test de generación en base a la posición del suelo con raycast.
    [Header("Ground Detectión.")]
    [Tooltip("Se asignar el mismo layer que el asignado al suelo.")]
    [SerializeField] LayerMask groundLayer; //LayerMask Referenciando el Suelo
    [SerializeField] float raycastHeight = 5f;  //

    [Tooltip("El número máximo de basura que se generara.")]
    [Header("Max Spawn Nº")]
    [SerializeField] int maxTrashToSpawn = 5;

    public static event Action<int> OnTrashSpawned;

    private List<GameObject> spawnedTrash = new();

    #region MyRegion

    #endregion
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
                prefabList[UnityEngine.Random.Range(0, prefabList.Count)],
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

            TrashType chosenType = validTypes[UnityEngine.Random.Range(0, validTypes.Count)];
            var prefabList = prefabsByType[chosenType];

            SpawnTrash(
                prefabList[UnityEngine.Random.Range(0, prefabList.Count)],
                chosenType,
                freePoints,
                spawnedPerType
            );
        }
        OnTrashSpawned?.Invoke(spawnedTrash.Count);
    }
    void SpawnTrash(
        GameObject prefab,
        TrashType type,
        List<Transform> freePoints,
        Dictionary<TrashType, int> spawnedPerType
    )
    {
        int pointIndex = UnityEngine.Random.Range(0, freePoints.Count);
        Transform point = freePoints[pointIndex];
        freePoints.RemoveAt(pointIndex);

        Vector3 spawnPosition = GetGroundedPosition(prefab, point.position);//Linea nueva para garantizar la posición del prefab de basura.

        //GameObject trash = Instantiate(prefab, point.position, point.rotation);
        GameObject trash = Instantiate(prefab, spawnPosition, point.rotation);
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
    #region Contar Basura
    public int GetTotalSpawnedTrash() 
    {
        return spawnedTrash.Count;
    }

    public int GetRemainingTrash() 
    {
        int count = 0;

        foreach (var t in spawnedTrash) 
        {
            if (t != null)
            {
                count++;
            }
        }
        return count;
    }

    #endregion


    Vector3 GetGroundedPosition(GameObject prefab, Vector3 basePosition) 
    {
        Vector3 rayOrigin = basePosition + Vector3.up * raycastHeight;

        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, raycastHeight * 2f, groundLayer))
        {
            float yOffset = 0f;

            Collider col = prefab.GetComponent<Collider>();
            if (col != null)
            {
                yOffset = col.bounds.extents.y;
            }

            return hit.point + Vector3.up * yOffset;
        }

        return basePosition;
    }
}
