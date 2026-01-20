using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private string pickupSfxKey;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!string.IsNullOrEmpty(pickupSfxKey))
            audioManager?.PlaySFX(pickupSfxKey);


        PickUpManager.Instance?.CollectPickup(transform);
        Destroy(gameObject);
    }
}