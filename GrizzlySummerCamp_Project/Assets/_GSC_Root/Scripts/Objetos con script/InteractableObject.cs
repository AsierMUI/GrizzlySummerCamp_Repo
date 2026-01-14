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
    //[SerializeField] MonoBehaviour playerMovementScript

    [Header("Dialogue")]
    [SerializeField] bool hasDialogue = false;
    [SerializeField] DialogueSystem dialogueSystem;

    private GameObject player;
    private PlayerInput playerInput;
    private InputAction interactAction;

    private bool isPlayerInRange = false;
    
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
            dialogueSystem.OnDialogueStarted += DisableNotebook;
            dialogueSystem.OnDialogueEnded += EnableNotebook;
        }
    }

    private void OnDestroy()
    {
        if (dialogueSystem != null)
        {
            dialogueSystem.OnDialogueStarted -= DisableNotebook;
            dialogueSystem.OnDialogueEnded -= EnableNotebook;
        }
    }

    void Update()
    {
        if (player == null) return;
       
        float distance = Vector3.Distance(player.transform.position, transform.position);
        isPlayerInRange = distance < interactionDistance; //Booleano, se vuelve verdadero(true) sí "distancia" es menor a "interactionDistance";

        if(spriteObject!=null)
            spriteObject.SetActive(isPlayerInRange); //Activa el objeto si el "isPlayerInRange" es verdadero

        if (!isPlayerInRange) return;

        //if (InstructionsUI == null) return;
        //Hemos quitado el cierre automatico por distancia
        if (interactAction.WasPressedThisFrame() && !UIState.IsUIOpen) 
        {
            Interact();
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
        if (InstructionsUI.activeSelf) return;

        InstructionsUI.SetActive(isPlayerInRange);
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
}