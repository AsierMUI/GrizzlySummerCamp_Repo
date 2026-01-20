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
    [SerializeField] float lookSpeed = 5f;

    private GameObject player;
    Transform playerTransform;
    Transform selfTransform;

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
        player = GameObject.FindGameObjectWithTag("Player");
        if (!player) return;

        playerTransform = player.transform;
        playerInput = player.GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");

        /*
        if (player != null)
        {
            playerInput = player.GetComponent<PlayerInput>();
            interactAction = playerInput.actions.FindAction("Interact");
        }
        */
        if (spriteObject != null)
            spriteObject.SetActive(false);
    }

    void Update()
    {
        if (!playerTransform) return;

        UpdateDistanceCheck();
        UpdateSprite();

        if (!isPlayerInRange) 
        {
            HandleAutoCloseUI();
            return;
        }

        HandleInteractionInput();
        HandleDialogueRotation();
        /*
        if (player == null) return;
       
        float distance = Vector3.Distance(player.transform.position, transform.position);
        isPlayerInRange = distance < interactionDistance; //Booleano, se vuelve verdadero(true) sí "distancia" es menor a "interactionDistance";

        if (spriteObject != null)
            spriteObject.SetActive(isPlayerInRange);

        if (!isPlayerInRange) //return;
        {
            //Volvemos a meter que se cierra por distancia (debería funcionar)
            if (!isInDialogue && InstructionsUI != null && InstructionsUI.activeSelf)
            {
                CloseUI();
            }
            return;
        }

        //Hemos quitado el cierre automatico por distancia
        if (interactAction.WasPressedThisFrame() && !UIState.IsUIOpen) 
        {
            Interact();
        }

        if (isInDialogue)
        {
           SmoothLookAt(player.transform, transform);
           SmoothLookAt(transform, player.transform);
        }
        */
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
        UIState.IsUIOpen = true;
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
        //Vector3 dir = target.transform.position - self.position;
        dir.y = 0;
        //if (dir == Vector3.zero) return;
        if (dir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        self.rotation = Quaternion.Slerp(self.rotation, targetRot, lookSpeed * Time.deltaTime);
    }
}