using UnityEngine;

public interface ITrashInteractable
{
    float InteractionDistance { get; }
    Transform Transform { get; }
}
