using UnityEngine;
using UnityEngine.InputSystem;

public class FishingMinigameInteractable : MonoBehaviour
{
    [Header("Conf interacción")]
    [SerializeField] float interactionDistance = 4f;
    [SerializeField] GameObject spriteObject;
    [SerializeField] Pesca_Prueba pescaScript;

    private GameObject player;
    private PlayerInput playerInput;
    private InputAction interactAction;

    private bool isPlayerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");

        spriteObject.SetActive(false);
        
    }
    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);
        isPlayerInRange = distance < interactionDistance;

        spriteObject.SetActive(isPlayerInRange);

        if (isPlayerInRange && interactAction.WasPressedThisFrame())
        {
            ShowInterface();
        }
    }

    void ShowInterface()
    {
        var pescaScript = Object.FindFirstObjectByType<Pesca_Prueba>();

        if (pescaScript != null)
            pescaScript.StartFishing();

        gameObject.SetActive(false);
    }
}
