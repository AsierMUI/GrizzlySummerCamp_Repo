using UnityEngine;
using System.Collections.Generic;
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

    //int maxspawnlist;

    public void SpawnAllTrash() 
    {
        ClearTrash();

        if (trashPrefabs.Count == 0 || spawnpoints.Count == 0) return;

        List<Transform> freePoints = new(spawnpoints);

        int spawnCount = Mathf.Min(maxTrashToSpawn, freePoints.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            int pointIndex = Random.Range(0, freePoints.Count);
            Transform point = freePoints[pointIndex];
            freePoints.RemoveAt(pointIndex);

            //Aqui se elige el numero aleatorio de los objetos.
            int prefabIndex = Random.Range(0, trashPrefabs.Count);
            //Aqui se selecciona el objeto en base al numero.
            GameObject prefab = trashPrefabs[prefabIndex];
            if (prefab == null)
            {
                i--;
                //No termina esta instancia del bucle, y vuelve al inicio del for.
                continue;
            }

            //Aqui se genera el objeto.
            GameObject trash = Instantiate(prefab, point.position, point.rotation);
            //Se añade a la lista de objetos generados.
            spawnedTrash.Add(trash);
        }
        /*
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
        */
    }

    void ClearTrash() 
    {
        foreach (var t in spawnedTrash)
            if (t != null) Destroy(t);

        spawnedTrash.Clear();
    }
}
