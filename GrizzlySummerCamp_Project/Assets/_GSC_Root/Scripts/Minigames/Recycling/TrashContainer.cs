using UnityEngine;

public class TrashContainer : MonoBehaviour, ITrashInteractable
{
    public TrashType acceptedType;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;

    public float InteractionDistance => interactionDistance;
    public Transform Transform => transform;

    public void TryDeposit(TrashPlayerCarry carry) 
    {
        if (!carry.IsCarryingTrash()) return;

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
