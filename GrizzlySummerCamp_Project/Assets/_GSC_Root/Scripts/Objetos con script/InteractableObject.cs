using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{

    [Header("Interaction")]
    [SerializeField] float interactionDistance = 4f;

    [Header("UI")]
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject InstructionsUI;

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
    }

    void Update()
    {
        if (player == null) return;
       
        float distance = Vector3.Distance(player.transform.position, transform.position);
        isPlayerInRange = distance < interactionDistance; //Booleano, se vuelve verdadero(true) sí "distancia" es menor a "interactionDistance";

        if(spriteObject!=null)
            spriteObject.SetActive(isPlayerInRange); //Activa el objeto si el "isPlayerInRange" es verdadero

        if (InstructionsUI != null)
        {
            //Sí se da ambos casos (boolean == "true" y Se presiona la tecla "E") llama a "LoadScene"
            if (isPlayerInRange && interactAction.WasPressedThisFrame())
            {
                OpenUI();
            }
            else if (!isPlayerInRange)
            {
                InstructionsUI.SetActive(false);
            }
        }
    }

    void OpenUI()
    {
        InstructionsUI.SetActive(isPlayerInRange);
    }
}