using UnityEngine;
using TMPro;
using System;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class DialogueSystem : MonoBehaviour
{
    public GameObject dialogueUI;
    public TextMeshProUGUI dialogueText;

    [TextArea(2, 5)] // Sirve para el minimo y el maximo de lineas que se ven en el inspector.
    public string[] dialogues;

    public float letterDelay = 0.03f; //el tiempo entre letras

    public event Action OnDialogueStarted;
    public event Action OnDialogueEnded;

    private int index;
    private bool isTalking;
    private bool canClick = true;
    private Coroutine typingCoroutine;

    public bool IsTalking => isTalking;

    private void Update()
    {
        if (!isTalking) return;

        if (canClick && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            NextDialogue();
            StartCoroutine(ClickCooldown());
        }
    }

    public void StartDialogue()
    {
        if (isTalking || dialogues.Length == 0) return;

        index = 0;
        isTalking = true;
        
        dialogueUI.SetActive(true);

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(dialogues[index]));

        UIState.IsUIOpen = true;
        OnDialogueStarted?.Invoke();
    }

    void NextDialogue()
    {
        index++;

        if (index < dialogues.Length)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            typingCoroutine = StartCoroutine(TypeSentence(dialogueText.text = dialogues[index]));
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);

        UIState.IsUIOpen = false;
        OnDialogueEnded?.Invoke();
    }

    IEnumerator ClickCooldown()
    {
        canClick = false;
        yield return new WaitForSeconds(0.3f);
        canClick = true;
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }
}