using UnityEngine;

public class SkillCheckSpawner : MonoBehaviour
{
    [Header("Target Prefab")]
    [SerializeField] private GameObject targetPrefab;

    [Header("Spawn Area")]
    [SerializeField] private RectTransform spawnArea;

    public void SpawnTarget(FishingSkillCheckManager manager)
    {
        GameObject newTarget = Instantiate(targetPrefab, spawnArea);
        SkillCheckTarget t = newTarget.GetComponent<SkillCheckTarget>();
        t.Initialize(manager);

        Vector2 rnd = new Vector2(
            Random.Range(-spawnArea.rect.width / 2, spawnArea.rect.width / 2),
            0
        );

        newTarget.GetComponent<RectTransform>().anchoredPosition = rnd;
    }
}
