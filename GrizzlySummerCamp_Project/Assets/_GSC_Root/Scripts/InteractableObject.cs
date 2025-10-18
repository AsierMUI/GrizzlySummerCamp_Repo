using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InteractableObject : MonoBehaviour
{
    public float interactionDistance = 2f; //Distancia para interactuar con el objeto
    public int sceneToLoad; //numero de la escena

    private GameObject player; //ref al player
    private bool isPlayerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            isPlayerInRange = distance < interactionDistance;

            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
            {
                LoadScene();
            }
        }
    }

    private void OnGUI()
    {
        if (isPlayerInRange)
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Presiona 'E' para interactuar"); //el texto estatico que se muestra

        }
    }
    
    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
