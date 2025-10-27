using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    [SerializeField] float interactionDistance = 4f;
    [SerializeField] int sceneToLoad;
    [SerializeField] GameObject spriteObject;

    private GameObject player;
    private PlayerInput playerInput;
    private InputAction interactAction;

    private bool isPlayerInRange = false;
    
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
        isPlayerInRange = distance < interactionDistance; //Booleano, se vuelve verdadero(true) s� "distancia" es menor a "interactionDistance";

        spriteObject.SetActive(isPlayerInRange); //Activa el objeto si el "isPlayerInRange" es verdadero

        if (isPlayerInRange && interactAction.WasPressedThisFrame()) //S� se da ambos casos (boolean == "true" y Se presiona la tecla "E") llama a "LoadScene"
        {
            LoadScene();
        }
    
    }

    void LoadScene()
    {
        SavePlayerPosition();
        SceneManager.LoadScene(sceneToLoad);
    }
    void SavePlayerPosition()
    {
        PlayerData.lastPosition = player.transform.position;
        PlayerData.hasSavedPosition = true;
    }
}
