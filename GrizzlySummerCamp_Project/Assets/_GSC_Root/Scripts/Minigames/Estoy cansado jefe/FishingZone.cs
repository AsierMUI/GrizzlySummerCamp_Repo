using UnityEngine;

public class FishingZone : MonoBehaviour
{
    //ESTE SCRIPT UTILIZA FISHINGZONESPAWNER PARA AVISARLE CUANDO DEBE VOLVER A SPAWNEAR OTRA ZONA
    // TAMBIEN SE ENCARGA DE QUE AL INTERACTUAR CON EL PREFAB DESAPAREZCA Y APAREZCA LA INTERFAZ EN FISHINGSKILLCHECK

    [SerializeField] GameObject interactIcon;
    [SerializeField] FishingSkillCheck skillCheck;

    bool playerInside;
    bool used;

    Transform spanwPoint;
    private void Start()
    {
        interactIcon.SetActive(false);

        skillCheck = FindObjectOfType<FishingSkillCheck>(true);

        if (skillCheck != null) Debug.Log("No se encuentra Fishingskillcheck en escena");
    }

    private void Update()
    {
        if (playerInside && !used && Input.GetKeyDown(KeyCode.E))
        {
            StartFishing();
        }
    }

    public void SetSpawnPoint(Transform point)
    {
        spanwPoint = point;
    }

    void StartFishing()
    {
        used = true;

        interactIcon.SetActive(false);

        FishingZoneSpawner.instance.RespawnSingleZone(spanwPoint);

        skillCheck.OnSkillCheckFinished += OnSkillCheckResult;
        skillCheck.StartSkillCheck();

        Destroy(gameObject);
    }

    void OnSkillCheckResult(bool success)
    {
        if (success)
        {
            Debug.Log("Capturao");
        }
        else
        {
            Debug.Log("Eres un pollo");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (used) return;

        if (other.CompareTag("Player"))
        {
            playerInside = true;
            interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (used) return;

        if (other.CompareTag("Player"))
        {
            playerInside = false;
            interactIcon.SetActive(false);
        }
    }
}
