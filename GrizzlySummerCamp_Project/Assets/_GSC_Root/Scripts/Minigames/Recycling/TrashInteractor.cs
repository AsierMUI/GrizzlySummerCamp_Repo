using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TrashInteractor : MonoBehaviour
{
    TrashPlayerCarry carry;
    PlayerInput playerInput;
    InputAction interactAction;


    private readonly List<ITrashInteractable> interactables = new();

    private void Awake()
    {
        carry = GetComponent<TrashPlayerCarry>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        interactAction = playerInput.actions["Interact"];
        //Sí se realiza la acción, se le suma el contexto "_" y se intenta "TryInteract()"
        interactAction.performed += _ => TryInteract();
    }

    private void Update()
    {
        UpdateInteractablesList();
    }

    void UpdateInteractablesList() 
    {
        interactables.Clear();
        foreach (var item in FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)) 
        {
            if (item is ITrashInteractable interactable)
            {
                float dist = Vector3.Distance(transform.position, interactable.Transform.position);

                if (dist <= interactable.InteractionDistance) 
                {
                    interactables.Add(interactable);
                }
            }
        }
    }

    void TryInteract()
    {
        if (!MinigameManager.Instance) return;
        if (interactables.Count == 0) return;

        ITrashInteractable closest = null;
        float minDist = float.MaxValue;

        foreach (var i in interactables)
        {
            float d = Vector3.Distance(transform.position, i.Transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = i;
            }
        }

        if (closest is TrashItem trash && !carry.IsCarryingTrash())
        {
            carry.PickTrash(trash);
            return;
        }

        if (closest is TrashContainer container && carry.IsCarryingTrash()) 
        {
            container.TryDeposit(carry);
            return;
        }
    }
}
