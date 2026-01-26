using UnityEngine;

public class TrashItem : MonoBehaviour, ITrashInteractable
{
    public TrashType trashType;
    public int points = 50;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 2f;
    [SerializeField] private float outlineDistance = 0f;

    public float InteractionDistance => interactionDistance;
    public float OutlineDistance => outlineDistance;
    public Transform Transform => transform;
}
