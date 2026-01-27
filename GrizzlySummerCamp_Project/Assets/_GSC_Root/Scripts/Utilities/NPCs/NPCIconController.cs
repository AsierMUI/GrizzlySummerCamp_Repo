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

    private bool isInDialogue;

    private void Start()
    {
        SetIcon(exclamationIcon, false);
        SetIcon(interactionIcon, false);
    }

    private void Update()
    {
        if (npcController == null || player == null) return;

        if (isInDialogue)
        {
            SetIcon(exclamationIcon, false);
            SetIcon(interactionIcon, false);
            return;
        }

        bool inRange = Vector3.Distance(player.position, transform.position) <= showDistance;
        bool showExclamation = npcController.ShouldShowExclamation();

        if (!showExclamation)
        {
            SetIcon(exclamationIcon, false);
            SetIcon(interactionIcon, inRange);
            FaceCamera(interactionIcon);
            return;
        }

        if (inRange)
        {
            SetIcon(exclamationIcon, false);
            SetIcon(interactionIcon, true);
            return;
        }
        else
        {
            SetIcon(exclamationIcon, true);
            SetIcon(interactionIcon, false);
        }

        FaceCamera(exclamationIcon);
        FaceCamera(interactionIcon);
    }

    public void SetDialoueState(bool talking)
    {
        isInDialogue = talking;
    }

    void SetIcon(SpriteRenderer icon, bool show)
    {
        if (icon != null)
        icon.enabled = show;
    }

    void FaceCamera(SpriteRenderer icon)
    {
        if (icon == null || Camera.main == null) return;
        icon.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}