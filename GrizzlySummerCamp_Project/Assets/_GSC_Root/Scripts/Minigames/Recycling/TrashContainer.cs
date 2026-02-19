using UnityEngine;
using System;

public class TrashContainer : MonoBehaviour, ITrashInteractable
{
    public TrashType acceptedType;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;

    [Header("Front Check")]
    [SerializeField] private TrashContainerFrontCheck frontCheck;

    public float InteractionDistance => interactionDistance;
    public Transform Transform => transform;

    public static event Action<bool> OnTrashDeposited;

    public void TryDeposit(TrashPlayerCarry carry) 
    {
        if (!carry.IsCarryingTrash()) return;

        if (frontCheck != null && !frontCheck.PlayerInFront)
            return;

        if (UIState.IsUIOpen)
            return; 

        bool correct = carry.GetCarriedType() == acceptedType;

        if (correct)
        {
            ScoreManager.Instance?.AddPoints(carry.GetCarriedPoints());
            Debug.Log("[Trash] Correct container");
        }
        else
        {
            Debug.Log("[Trash] Wrong container");
        }

        OnTrashDeposited?.Invoke(correct);

        carry.DeliverTrash();

        if (FindObjectsByType<TrashItem>(FindObjectsSortMode.None).Length == 0)
        {
            MinigameManager.Instance?.EndMinigame(true);
        }
    }
}
