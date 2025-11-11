using UnityEditor.Build.Content;
using UnityEngine;

public class PickUpLaberinto : MonoBehaviour
{
    [SerializeField] private int puntosC = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PickUpManager.instance.AddScore(puntosC);
            Destroy(gameObject);
        }
    }
}
