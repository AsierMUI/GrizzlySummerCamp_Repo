using UnityEngine;
using UnityEngine.InputSystem;

public class TrashInteractor : MonoBehaviour
{
    [Header("Interaction Field")]
    [SerializeField] float interactRange = 2f;
    [SerializeField] LayerMask interactLayer;

    TrashPlayerCarry carry;
    PlayerInput playerInput;
    InputAction interactAction;

    private void Awake()
    {
        carry = GetComponent<TrashPlayerCarry>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        interactAction = playerInput.actions["Interact"];
        interactAction.performed += _ = TryInteract();
    }

    void TryInteract() 
    {
        if (!MinigameManager.Instance) return;

        Ray ray = new Ray(Transform.position + Vector3.up, transform.forward);
        if (!Physics.Raycast(ray, out RaycastHit hit, interactRange, interactLayer))
            return;

        if (hit.collider.TryGetComponent(out TrashItem trash))
        {
            carry.PickTrash(trash);
            return;
        }

        if (hit.collider.TryGetComponent(out TrashContainter container))
        {
            container.TryDeposit(carry);
        }
    }
}
