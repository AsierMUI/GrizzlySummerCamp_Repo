using UnityEditor.Build.Content;
using UnityEngine;

public class PickUpLaberinto : MonoBehaviour
{
    public int value = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUpManager.instance.AddScore(value);

            Destroy(gameObject);
        }
    }
}
