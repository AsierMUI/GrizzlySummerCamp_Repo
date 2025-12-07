using UnityEngine;
using UnityEngine.InputSystem;

public class FishingMinigameInteractable : MonoBehaviour
{
    [Header("Conf interacción")]
    [SerializeField] float interactionDistance = 4f;
    [SerializeField] GameObject spriteObject;

    [Header("Script minijuego")]
    [SerializeField] FMControler fishingController;

    [SerializeField] GameObject player;
    PlayerInput playerInput;
    InputAction interactAction;

    bool isPlayerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");
        spriteObject.SetActive(false);

        if (fishingController == null)
        {
            fishingController = FindFirstObjectByType<FMControler>();

            if (fishingController == null)
                Debug.LogError("no hay fmcontroler en la escena");
        }
    }

    void Update()
    {
        if (player == null || interactAction == null) return;

        float distance = Vector3.Distance(player.transform.position, transform.position);
        isPlayerInRange = distance < interactionDistance;

        spriteObject.SetActive(isPlayerInRange);

        if (isPlayerInRange && interactAction.WasPressedThisFrame())
            ShowInterface();
    }

    void ShowInterface()
    {
        if (fishingController != null)
        {
            fishingController.StartFishing();
        }
        else
        {
            Debug.LogError("el prefab no tiene fmcoltroler");
            return;
        }
        gameObject.SetActive(false);
        FishingZoneSpawner.instance.RespawnSingleZone(10f);
    }
}
