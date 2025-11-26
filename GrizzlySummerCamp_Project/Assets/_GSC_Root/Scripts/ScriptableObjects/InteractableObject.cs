using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] float interactionDistance = 4f;
    [SerializeField] int sceneToLoad;
    [SerializeField] GameObject spriteObject;
    [SerializeField] GameObject InstructionsUI;

    private GameObject player;
    private PlayerInput playerInput;
    private InputAction interactAction;

    private bool isPlayerInRange = false;
    bool UIactive;
    
    void Start()
    {
        //Asigna el jugador al iniciar.
        player = GameObject.FindGameObjectWithTag("Player");
        playerInput = player.GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");
        
        spriteObject.SetActive(false);

    }

    void Update()
    {
        if (player == null) return;
        float distance = Vector3.Distance(player.transform.position, transform.position);
        isPlayerInRange = distance < interactionDistance; //Booleano, se vuelve verdadero(true) sí "distancia" es menor a "interactionDistance";

        spriteObject.SetActive(isPlayerInRange); //Activa el objeto si el "isPlayerInRange" es verdadero
        if (InstructionsUI != null) 
        {
            if (!InstructionsUI.activeInHierarchy)
            {
                var playersc = FindFirstObjectByType<PlayerMovement>();
                playersc.canMove = true;
            }
            else
            {
                var playersc = FindFirstObjectByType<PlayerMovement>();
                playersc.canMove = false;
            }

            if (isPlayerInRange && interactAction.WasPressedThisFrame()) //Sí se da ambos casos (boolean == "true" y Se presiona la tecla "E") llama a "LoadScene"
            {
                LoadScene();
            }
        }
    }

    void LoadScene()
    {
        SavePlayerPosition();
        InstructionsUI.SetActive(true);
    }
    void SavePlayerPosition()
    {
        PlayerData.lastPosition = player.transform.position;
        PlayerData.hasSavedPosition = true;
    }
}
