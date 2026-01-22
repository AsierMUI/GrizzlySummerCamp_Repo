using Unity.VisualScripting;
using UnityEngine;

public class TrashContainerFrontCheck : MonoBehaviour
{
    public bool PlayerInFront { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInFront = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        { 
            PlayerInFront = false;
        }
    }

}
