using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrashProximityInteract : MonoBehaviour
{
    [Tooltip("Distancia a la que el jugador puede interactuar con la basura.")]
    [SerializeField] float interactionDistance = 2f;

    Transform player;
    TrashPlayerCarry carry;

    PlayerInput playerInput;
    InputAction interactAction;

    bool playerInRange;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        carry = player?.GetComponent<PlayerInput>();


    }
}
