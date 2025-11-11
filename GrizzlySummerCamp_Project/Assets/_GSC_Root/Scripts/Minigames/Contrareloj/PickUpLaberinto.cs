using UnityEditor.Build.Content;
using UnityEngine;

public class PickUpLaberinto : MonoBehaviour
{
    [SerializeField] private int puntos = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUpManager.instance.AddScore(puntos);
            Destroy(gameObject);
        }
    }
}
