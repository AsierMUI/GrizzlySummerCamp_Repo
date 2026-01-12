using UnityEngine;
using System.Collections.Generic;
public class TrashSpawner : MonoBehaviour
{
    [Header("Trash Prefabs")]
    [SerializeField] List<GameObject> trashPrefabs;

    [Header("Spawn Points (empites)")]
    [SerializeField] List<Transform> spawnpoints;

    private List<GameObject> spawnedTrash = new();

    public void SpawnAllTrash() 
    {
        ClearTrash();

        List<Transform> freePoints = new(spawnpoints);

        foreach (GameObject prefab in trashPrefabs) 
        {
            if (freePoints.Count == 0) break;

            int index = Random.Range(0, freePoints.Count);
            Transform point = freePoints[index];
            freePoints.RemoveAt(index);

            GameObject trash = Instantiate(prefab, point.position, point.rotation);
            spawnedTrash.Add (trash);
        }
    }

    void ClearTrash() 
    {
        foreach (var t in spawnedTrash)
            if (t != null) Destroy(t);

        spawnedTrash.Clear();
    }
}
