using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueSystem : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    [TextArea(2, 5)]
    public string[] dialogues;

    private int index;
    private bool isTalking;
    private bool canClick = true;

    private void Update()
    {
        if (!isTalking) return;

        if (canClick && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            NextDialogue();
        }
    }

    public void StartDialogue()
    {
        isTalking = true;
        index = 0;
        dialogueText.text = dialogues[index];
        dialogueText.gameObject.SetActive(true);
    }

    void NextDialogue()
    {
        index++;
        if (index < dialogues.Length)
        {
            dialogueText.text = dialogues[index];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueText.gameObject.SetActive(false);
    }

    IEnumerator ClickCooldown()
    {
        canClick = false;
        yield return new WaitForSeconds(0.2f);
        canClick = true;
    }
}