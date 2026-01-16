using UnityEngine;

[RequireComponent(typeof(InteractableObject))]
public class NPCDialogueController : MonoBehaviour
{
    public enum NPCDialogueMode
    {
        Simple,
        ConInsignias
    }
    //Para los npcs simples como por ejemplo el pirata, solo ponemos en dialogo normal y final, para el monitor todos

    [Header("ID Unico del NPC (Obligatorio)")]
    public string npcID;

    [Header("Modo de npc")]
    public NPCDialogueMode modo = NPCDialogueMode.Simple;

    [Header("Dialogues (Pon los que necesites)")]
    public DialogueSystem dialogoTutorial;
    public DialogueSystem dialogoNormal;
    public DialogueSystem dialogoProgreso;
    public DialogueSystem dialogoFinal;

    [Header("Minigame Keys (Deben coincidir con InsigniaManager)")]
    public string pescaKey = "Pesca";
    public string basurakey = "Basura";
    public string arcoKey = "Arco";
    public string contrarelojKey = "Contrareloj";

    private NPCState state;
    private InteractableObject interactable;

    private void Awake()
    {
        state = NPCStateManager.Instance.GetState(npcID);
        interactable = GetComponent<InteractableObject>();
        interactable.npcController = this;
    }
    private void OnEnable()
    {
        SubscribeDialogue(dialogoTutorial);
        SubscribeDialogue(dialogoNormal);
        SubscribeDialogue(dialogoProgreso);
        SubscribeDialogue(dialogoFinal);
    }

    private void SubscribeDialogue(DialogueSystem dialogue)
    {
        if (dialogue == null) return;

        dialogue.OnDialogueStarted += interactable.OnDialogueStart;
        dialogue.OnDialogueEnded += interactable.OnDialogueEnded;
    }

    public void Interact()
    {
        if (UIState.IsUIOpen) return;
       
        switch (modo)
        {
            case NPCDialogueMode.Simple:
                InteractSimple();
                break;

            case NPCDialogueMode.ConInsignias:
                InteractConInsignias();
                break;
        }
    }

    //NPC Simple
    void InteractSimple()
    {
        if (!state.dialogoNormalUsado && dialogoNormal != null)
        {
            state.dialogoNormalUsado = true;
            dialogoNormal.StartDialogue();
            return;
        }

        if (dialogoFinal != null)
        {
            dialogoFinal.StartDialogue();
        }
    }

    //NPC Completo
    void InteractConInsignias()
    {
        if (InsigniaManager.Instance == null) return;

        //Locura de ifs como el undertale

        // primero el tutoial(si existe y no se ha completao)
        if (dialogoTutorial != null && !state.tutorialCompletado)
        {
            state.tutorialCompletado = true;
            dialogoTutorial.StartDialogue();
            return;
        }

        //Todas las insignias = final (si existe)
        if (dialogoFinal != null && TieneTodasLasInsignias())
        {
            dialogoFinal.StartDialogue();
            return;
        }

        //Alguna insignia = progreso (si existe)
        if (dialogoProgreso != null && TieneAlgunaInsignia())
        {
            dialogoProgreso.StartDialogue();
            return;
        }

        //Normal (fallback seguro)
        if (dialogoNormal != null)
        {
            dialogoNormal.StartDialogue();
        }
    }

    bool TieneAlgunaInsignia()
    {
        return
            InsigniaManager.Instance.GetInsignia(pescaKey) > 0 ||
            InsigniaManager.Instance.GetInsignia(basurakey) > 0 ||
            InsigniaManager.Instance.GetInsignia(arcoKey) > 0 ||
            InsigniaManager.Instance.GetEstrella(contrarelojKey) > 0;
    }

    bool TieneTodasLasInsignias()
    {
        return
            InsigniaManager.Instance.GetInsignia(pescaKey) > 0 &&
            InsigniaManager.Instance.GetInsignia(basurakey) > 0 &&
            InsigniaManager.Instance.GetInsignia(arcoKey) > 0 &&
            InsigniaManager.Instance.GetEstrella(contrarelojKey) > 0;
    }
}