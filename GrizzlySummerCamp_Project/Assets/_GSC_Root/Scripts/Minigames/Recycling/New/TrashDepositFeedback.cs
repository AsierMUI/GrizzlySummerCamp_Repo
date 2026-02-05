using UnityEngine;
using TMPro;
using System.Collections;

public class TrashDepositFeedback : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject feedbackSprite;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Settings")]
    [SerializeField] private float showTime = 1.2f;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;

    Coroutine routine;

    private void OnEnable()
    {
        TrashContainer.OnTrashDeposited += ShowFeedback;
    }

    private void OnDisable()
    {
        TrashContainer.OnTrashDeposited -= ShowFeedback;

    }

    void ShowFeedback(bool correct) 
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(FeedbackRoutine(correct));
    }

    IEnumerator FeedbackRoutine(bool correct) 
    {
        if(feedbackSprite)
            feedbackSprite.gameObject.SetActive(true);

        if (feedbackText)
        {
            feedbackText.gameObject.SetActive(true);
            feedbackText.text = correct ? "Well done!" : "That's not right...";
            feedbackText.color = correct ? correctColor : wrongColor;
        }

        yield return new WaitForSeconds(showTime);

        if (feedbackSprite)
            feedbackSprite.gameObject.SetActive(false);

        if (feedbackText)
            feedbackText.gameObject.SetActive(false);
    }

}
