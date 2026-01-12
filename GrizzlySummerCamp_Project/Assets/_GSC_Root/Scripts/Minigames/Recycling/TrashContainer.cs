using UnityEngine;

public class TrashContainer : MonoBehaviour
{
    public TrashType acceptedType;

    public void TryDeposit(TrashPlayerCarry carry) 
    {
        if (carry.IsCarryingTrash()) return;

        if (carry.GetCarriedType() == acceptedType)
        {
            ScoreManager.Instance?.AddPoints(carry.GetCarriedPoints());
            Debug.Log("[Trash] Correct container");
        }
        else 
        {
            Debug.Log("[Trash] Wrong container");
        }

        carry.DeliverTrash();

        if (FindObjectsByType<TrashItem>(FindObjectsSortMode.None).Length == 0)
        {
            MinigameManager.Instance?.EndMinigame(true);
        }
    }
}
