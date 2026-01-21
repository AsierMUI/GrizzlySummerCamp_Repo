using UnityEngine;

public class TrashItem : MonoBehaviour, ITrashInteractable
{
    public TrashType trashType;
    public int points = 50;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 2f;

    public float InteractionDistance => interactionDistance;
    public Transform Transform => transform;
}
