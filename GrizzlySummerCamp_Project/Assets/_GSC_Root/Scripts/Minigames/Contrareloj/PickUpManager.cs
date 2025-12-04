using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager instance;

    public int score = 0;
    public int maxScore = 4;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject goal;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject boat;

    [SerializeField] GameObject Notification;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pointSound;

    private BoatMovement boatMovement;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (boat != null)
        {
            boatMovement = boat.GetComponent<BoatMovement>();
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score:" + score);

        PlayPointSound();

        UpdateScoreUI();

        if (score >= maxScore)
            ActivateGoal();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Points:{score}/{maxScore}";
    }

    void ActivateGoal()
    {
        StartCoroutine("Notice");
        goal.SetActive(true);
    }
    IEnumerator Notice() 
    {
        if (Notification != null)
        {
            Notification.SetActive(true);
            yield return new WaitForSeconds(3f);
            Notification.SetActive(false);
        }
    }

    public void ReachedGoal()
    {
        if (winUI != null)
            winUI.SetActive(true);

        boatMovement.enabled = false;

        if (InsigniaManager.Instance != null)
        {
            InsigniaManager.Instance.GuardarEstrella(1);
        }
    }

    void PlayPointSound()
    {
        if (audioSource != null && pointSound != null)
            audioSource.PlayOneShot(pointSound);
    }
}
