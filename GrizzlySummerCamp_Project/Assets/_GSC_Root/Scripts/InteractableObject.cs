using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class InteractableObject : MonoBehaviour
{
    public float interactionDistance = 2f; //Distancia para interactuar con el objeto
    public int sceneToLoad; //numero de la escena
    public GameObject textObject; //ref al texto

    private GameObject player; //ref al player
    private bool isPlayerInRange = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        textObject.SetActive(false);
    }

    void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(player.transform.position, transform.position);
            isPlayerInRange = distance < interactionDistance;

            textObject.SetActive(isPlayerInRange);

            if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
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
