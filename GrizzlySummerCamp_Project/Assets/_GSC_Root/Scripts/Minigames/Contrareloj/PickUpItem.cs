using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUpManager.instance.AddScore(transform);
            Destroy(gameObject);
        }
    }
}
