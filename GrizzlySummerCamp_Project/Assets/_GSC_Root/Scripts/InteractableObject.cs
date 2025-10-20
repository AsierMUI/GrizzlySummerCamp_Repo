using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
//using UnityEngine.InputSystem;

public class InteractableObject : MonoBehaviour
{
    public float interactionDistance = 4f; //Distancia para interactuar con el objeto
    public int sceneToLoad; //numero de la escena
    public GameObject textObject; //ref al texto

    private GameObject player; //ref al player
    private bool isPlayerInRange = false;

    void Start()
    {
        //Asigna el jugador al iniciar.
        player = GameObject.FindGameObjectWithTag("Player");
        textObject.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            isPlayerInRange = distance < interactionDistance; //Booleano, se vuelve verdadero(true) sí "distancia" es menor a "interactionDistance";

            textObject.SetActive(isPlayerInRange); //Activa el objeto si el "isPlayerInRange" es verdadero

            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E)) //Sí se da ambos casos (boolean == "true" y Se presiona la tecla "E") llama a "LoadScene"
            {
                LoadScene();
            }
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
