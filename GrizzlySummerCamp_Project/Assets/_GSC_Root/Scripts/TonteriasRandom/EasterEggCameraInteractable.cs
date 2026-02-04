using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class EasterEggCameraInteractable : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] float interactionDistance = 4f;

    [Header("Camera")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Camera easterEggCamera;
    [SerializeField] float duration = 5f;

    [Header("Player")]
    [SerializeField] PlayerMovement playerMovement;

    private Transform player;
    private PlayerInput playerInput;
    private InputAction interactAction;

    private bool isPlayerInRange;
    private bool isPlaying;

    float interactionDistanceSqr;
    private void Awake()
    {
        interactionDistanceSqr = interactionDistance * interactionDistance;
    }

    private void Start()
    {
        StartCoroutine(FindPlayerWhenAvailable());

        if (easterEggCamera != null)
        {
            easterEggCamera.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!player || interactAction == null || isPlaying) return;

        CheckDistance();

        if (isPlayerInRange && interactAction.WasPressedThisFrame())
        {
            StartCoroutine(PlayEasterEgg());
        }
    }

    void CheckDistance()
    {
        float sqrDistance = (player.position - transform.position).sqrMagnitude;
        isPlayerInRange = sqrDistance <= interactionDistanceSqr;
    }

    IEnumerator PlayEasterEgg()
    {
        isPlaying = true;

        if (playerMovement != null) playerMovement.SetCanMove(false);

        if (mainCamera != null) mainCamera.gameObject.SetActive(false);

        if (easterEggCamera != null) easterEggCamera.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        if (easterEggCamera != null) easterEggCamera.gameObject.SetActive(false);

        if (mainCamera != null) mainCamera.gameObject.SetActive(true);

        if (playerMovement != null) playerMovement.SetCanMove(true);

        isPlaying = false;
    }

    IEnumerator FindPlayerWhenAvailable()
    {
        while (player == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");

            if (playerGO != null && playerGO.activeInHierarchy)
            {
                player = playerGO.transform;

                playerInput = playerGO.GetComponent<PlayerInput>();
                playerMovement = playerGO.GetComponent<PlayerMovement>();

                if (playerInput != null)
                {
                    interactAction = playerInput.actions.FindAction("Interact");
                }
                yield break;
            }

            yield return null;
        }
    }
}