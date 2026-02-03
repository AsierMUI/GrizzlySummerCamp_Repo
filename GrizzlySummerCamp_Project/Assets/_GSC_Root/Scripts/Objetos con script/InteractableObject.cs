using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] float interactionDistance = 4f;

    [Header("UI")]
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject InstructionsUI;
    [SerializeField] GameObject notebookUI;

    [Header("Player")]
    [SerializeField] PlayerMovement playerMovementScript;

    [Header("NPC")]
    public NPCDialogueController npcController;

    [Header("Rotation")]
    [SerializeField] bool rotateDurationDialogue = true;
    [SerializeField] float lookSpeed = 5f;

    private GameObject player;
    private Transform playerTransform;
    private Transform selfTransform;

    private PlayerInput playerInput;
    private InputAction interactAction;

    private bool isPlayerInRange = false;
    private bool isInDialogue = false;

    private float interactionDistanceSqr;

    private void Awake()
    {
        selfTransform = transform;
        interactionDistanceSqr = interactionDistance * interactionDistance;
    }

    void Start()
    {
        TryGetPlayer();

        if (spriteObject != null)
            spriteObject.SetActive(false);
    }
    private void OnEnable()
    {
        TryGetPlayer();
    }

    void TryGetPlayer()
    {
        if (playerTransform != null) return;

        player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        playerTransform = player.transform;
        playerInput = player.GetComponent<PlayerInput>();

        if (playerInput != null)
        {
            interactAction = playerInput.actions.FindAction("Interact");
        }

        playerMovementScript = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!playerTransform)
        {
            TryGetPlayer();
            return;
        }

        UpdateDistanceCheck();
        UpdateSprite();

        if (!isPlayerInRange) 
        {
            HandleAutoCloseUI();
            return;
        }

        HandleInteractionInput();
        HandleDialogueRotation();
    }
    //Nueva lógica
    void UpdateDistanceCheck() 
    {
        float sqrDistance = (playerTransform.position - selfTransform.position).sqrMagnitude;

        isPlayerInRange = sqrDistance < interactionDistanceSqr;
    }

    void UpdateSprite() 
    {
        if (spriteObject) 
        {
            spriteObject.SetActive(isPlayerInRange);
        }
    }

    void HandleAutoCloseUI() 
    {
        if (!isInDialogue && InstructionsUI && InstructionsUI.activeSelf)
        {
            CloseUI();
        }
    }

    void HandleInteractionInput() 
    {
        if (interactAction.WasPressedThisFrame() && !UIState.IsUIOpen) 
        {
            Interact();
        }
    }

    void HandleDialogueRotation() 
    {
        if (!isInDialogue) return;
        if (!rotateDurationDialogue) return;

        SmoothLookAt(playerTransform, selfTransform);
        SmoothLookAt(selfTransform, playerTransform);
    }


    void Interact()
    {
        if (npcController != null)
        {
            npcController.Interact();
            return;
        }

        OpenUI();
    }

    void OpenUI()
    {
        if (InstructionsUI == null || InstructionsUI.activeSelf) return;

        InstructionsUI.SetActive(true);
        UIState.SetUIOpen(true);
    }

    void CloseUI() 
    {
        if (isInDialogue || !InstructionsUI.activeSelf) return;

        InstructionsUI.SetActive(false);
        UIState.IsUIOpen = false;
    }

    public void CloseUIFromButton() 
    {
        CloseUI();
    }

    
    public void PlayUI() 
    {
        UIState.IsUIOpen = false;
    }
    


    void DisableNotebook()
    {
        if (notebookUI != null)
            notebookUI.SetActive(false);
    }

    void EnableNotebook()
    {
        if (notebookUI != null)
            notebookUI.SetActive(true);
    }

    //Cosas dialogo
    //Se llaman desde npcdialoguecontroller desde dialogue system
    public void OnDialogueStart()
    {
        isInDialogue = true;
        DisableNotebook();
        BlockPlayerMovement();
    }

    public void OnDialogueEnded()
    {
        isInDialogue = false;
        EnableNotebook();
        UnblockPlayerMovement();
    }
     //Movimiento personaje
    void BlockPlayerMovement()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.SetCanMove(false);
        }
    }

    void UnblockPlayerMovement()
    {
        if (playerMovementScript != null)
        {
            playerMovementScript.SetCanMove(true);
        }
    }

    //Giro
    void SmoothLookAt(Transform target, Transform self)
    {
        Vector3 dir = target.position - self.position;
        dir.y = 0;
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        self.rotation = Quaternion.Slerp(self.rotation, targetRot, lookSpeed * Time.deltaTime);
    }
}