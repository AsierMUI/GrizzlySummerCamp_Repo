using UnityEngine;

public class NPCIconController : MonoBehaviour
{
    [Header("Iconos")]
    public SpriteRenderer exclamationIcon;
    public SpriteRenderer interactionIcon;

    [Header("Ref NPC")]
    public NPCDialogueController npcController;

    [Header("Jugador")]
    public Transform player;
    public float showDistance = 2f;

    private bool isPlayerInRange;


    private void Start()
    {
        SetIcon(exclamationIcon, false);
        SetIcon(interactionIcon, false);
    }

    private void Update()
    {
        if (npcController == null || player == null) return;

        isPlayerInRange = Vector3.Distance(player.position, transform.position) <= showDistance;
        bool hasImportantDialogue = npcController.HasImportantDialogue();

        if (hasImportantDialogue && !isPlayerInRange)
        {
            SetIcon(exclamationIcon, true);
            SetIcon(interactionIcon, false);
        }
        else if (hasImportantDialogue && isPlayerInRange)
        {
            SetIcon(exclamationIcon, false);
            SetIcon(interactionIcon, true);
        }
        else
        {
            SetIcon(exclamationIcon, false);
            SetIcon(interactionIcon, false);
        }

        FaceCamera(exclamationIcon);
        FaceCamera(interactionIcon);
    }

    public void OnPlayerInteract()
    {
        SetIcon(interactionIcon, false);
        SetIcon(exclamationIcon, false);
    }

    void SetIcon(SpriteRenderer icon, bool show)
    {
        if (icon == null) return;
        icon.enabled = show;
    }

    void FaceCamera(SpriteRenderer icon)
    {
        if (icon == null) return;
        icon.transform.rotation = Quaternion.LookRotation(icon.transform.position - Camera.main.transform.forward);
    }
}