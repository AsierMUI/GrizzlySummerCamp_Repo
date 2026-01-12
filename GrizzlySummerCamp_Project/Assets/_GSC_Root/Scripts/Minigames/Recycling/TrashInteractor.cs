using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class TrashInteractor : MonoBehaviour
{
    /*
    [Header("Interaction Field")]
    [SerializeField] float interactRange = 2f;
    [SerializeField] LayerMask interactLayer;
    */
    TrashPlayerCarry carry;
    PlayerInput playerInput;
    InputAction interactAction;

    private readonly List<MonoBehaviour> nearbyInteractables = new(); 

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

    void TryInteract() 
    {
        if (!MinigameManager.Instance) return;
        Debug.Log($"Carrying: {carry.IsCarryingTrash()}");

        nearbyInteractables.RemoveAll(obj => obj == null);

        foreach (var obj in nearbyInteractables) 
        {
            if (carry.IsCarryingTrash() && obj is TrashContainer container)
            {
                container.TryDeposit(carry);
                return;
            }

            if (!carry.IsCarryingTrash() && obj is TrashItem trash)
            {
                carry.PickTrash(trash);
                return;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Entered: {other.name}");
        if (other.TryGetComponent(out TrashItem trash))
            nearbyInteractables.Add(trash);

        if (other.TryGetComponent(out TrashContainer container))    
            nearbyInteractables.Add(container);
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.TryGetComponent(out TrashItem trash))
            nearbyInteractables.Remove(trash);

        if (other.TryGetComponent(out TrashContainer container))
            nearbyInteractables.Remove(container);
    }
}
