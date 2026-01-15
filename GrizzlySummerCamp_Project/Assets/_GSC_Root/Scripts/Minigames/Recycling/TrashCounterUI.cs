using UnityEngine;
using TMPro;

public class TrashCounterUI : MonoBehaviour
{
    [SerializeField] private TrashSpawner spawner;
    [SerializeField] private TMP_Text counterText;

    private void Update()
    {
        if (spawner == null || counterText == null) return;

        int remaining = spawner.GetRemainingTrash();
        int total = spawner.GetTotalSpawnedTrash();

        counterText.text = $"Basura: {total - remaining}/{total}";
    }
}
