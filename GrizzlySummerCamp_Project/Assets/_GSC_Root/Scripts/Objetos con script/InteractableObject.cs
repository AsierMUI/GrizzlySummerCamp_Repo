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

    [Header("Dialogue")]
    [SerializeField] bool hasDialogue = false;
    [SerializeField] DialogueSystem dialogueSystem;

    [Header("Rotation")]
    [SerializeField] float lookSpeed = 5f;

    private GameObject player;
    private PlayerInput playerInput;
    private InputAction interactAction;

    private bool isPlayerInRange = false;
    private bool isInDialogue = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            interactAction = playerInput.actions.FindAction("Interact");
        }

        if (spriteObject != null)
            spriteObject.SetActive(false);

        if (dialogueSystem != null)
        {
            dialogueSystem.OnDialogueStarted += OnDialogueStart;
            dialogueSystem.OnDialogueEnded += OnDialogueEnded;
        }
    }

    private void OnDestroy()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.OnDialogueStarted -= OnDialogueStart;
            dialogueSystem.OnDialogueEnded -= OnDialogueEnded;
        }
    }

    void Update()
    {
        if (player == null) return;
       
        float distance = Vector3.Distance(player.transform.position, transform.position);
        isPlayerInRange = distance < interactionDistance; //Booleano, se vuelve verdadero(true) sí "distancia" es menor a "interactionDistance";

        if (spriteObject!=null)
            spriteObject.SetActive(isPlayerInRange); //Activa el objeto si el "isPlayerInRange" es verdadero

        if (!isPlayerInRange) return;

        //Hemos quitado el cierre automatico por distancia
        if (interactAction.WasPressedThisFrame() && !UIState.IsUIOpen) 
        {
            Interact();
        }

        if (isInDialogue)
        {
            //SmoothLookAt(player, transform);
           // SmoothLookAt(transform, player);
        }
    }

    void Interact()
    {
        if (hasDialogue && dialogueSystem != null)
        {
            dialogueSystem.StartDialogue();
            return;
        }

        OpenUI();
    }

    void OpenUI()
    {
        if (InstructionsUI == null || InstructionsUI.activeSelf) return;

        InstructionsUI.SetActive(true);
        UIState.IsUIOpen = true;
    }

    void CloseUI() 
    {
        if (!InstructionsUI.activeSelf) return;

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

    void OnDialogueStart()
    {
        DisableNotebook();
        BlockPlayerMovement();
        LookAtEachOther();
    }

    void OnDialogueEnded()
    {
        EnableNotebook();
        UnblockPlayerMovement();
    }

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

    void LookAtEachOther()
    {
        if (player == null) return;

        Vector3 playerDir = transform.position - player.transform.position;
        playerDir.y = 0;
        if(playerDir != Vector3.zero)
            player.transform.rotation = Quaternion.LookRotation(playerDir);


        Vector3 npcDir = player.transform.position - transform.position;
        npcDir.y = 0;
        if(npcDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(npcDir);

    }
}