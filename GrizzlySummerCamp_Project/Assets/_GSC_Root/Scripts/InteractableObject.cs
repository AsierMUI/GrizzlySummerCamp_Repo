using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
//using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public float interactionDistance = 4f; //Distancia para interactuar con el objeto
    public int sceneToLoad; //numero de la escena
    public GameObject spriteObject; //ref al sprite

    private GameObject player; //ref al player
    private bool isPlayerInRange = false;

    void Start()
    {
        //Asigna el jugador al iniciar.
        player = GameObject.FindGameObjectWithTag("Player");
        spriteObject.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            isPlayerInRange = distance < interactionDistance; //Booleano, se vuelve verdadero(true) sí "distancia" es menor a "interactionDistance";

            spriteObject.SetActive(isPlayerInRange); //Activa el objeto si el "isPlayerInRange" es verdadero

            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) //Sí se da ambos casos (boolean == "true" y Se presiona la tecla "E") llama a "LoadScene"
            {
                LoadScene();
            }
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
