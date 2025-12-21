using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PickUpManager.Instance?.CollectPickup(transform);
        Destroy(gameObject);
    }
}