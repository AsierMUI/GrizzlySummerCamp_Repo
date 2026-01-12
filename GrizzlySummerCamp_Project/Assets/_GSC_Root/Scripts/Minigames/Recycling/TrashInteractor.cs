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

        foreach (var obj in nearbyInteractables) 
        {
            if (obj is TrashItem trash && !carry.IsCarryingTrash()) 
            {
                carry.PickTrash(trash);
                return;
            }

            if (obj is TrashContainer container && carry.IsCarryingTrash())
            {
                container.TryDeposit(carry);
                return;
            }   
        
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
