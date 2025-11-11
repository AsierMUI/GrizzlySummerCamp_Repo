using UnityEngine;

public class GoalLaberinto : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            PickUpManager.instance.ReachedGoal();
    }
}
