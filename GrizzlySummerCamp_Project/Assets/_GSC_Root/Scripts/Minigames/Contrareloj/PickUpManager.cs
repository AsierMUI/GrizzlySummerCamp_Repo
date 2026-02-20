using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PickUpManager : MonoBehaviour
{
    #region Singleton
    public static PickUpManager Instance;
    #endregion

    #region Configuracion
    [Header("Score")]
    [SerializeField] private int maxScore = 4;
    [SerializeField] private TMP_Text scoreText;

    [Header("Lvl elements")]
    [SerializeField] private GameObject goal;

    [Header("UI")]
    [SerializeField] private GameObject notificationUI;
    [SerializeField] private GameObject winUI;

    [Header("Arrow System")]
    [SerializeField] private Image arrowUI;
    [SerializeField] private Transform player;

    /*
    [Header("Sounds")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip pointSound;
    */
    private AudioManager audioManager;
    #endregion

    #region Pickups
    private List<Transform> pickups = new List<Transform>();
    private Transform lastPickup;
    #endregion

    #region Unity Callbacks
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        /*
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        */
        audioManager = FindFirstObjectByType<AudioManager>();

    }

    private void Start()
    {
        ScoreManager.Instance?.ResetScore();
        UpdateScoreUI();

        //Busca los pickups por tag
        GameObject[] found = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (var p in found)
            pickups.Add(p.transform);

        if (arrowUI != null)
            arrowUI.gameObject.SetActive(false);

        if (goal != null)
            goal.SetActive(false);
    }

    private void Update()
    {
        UpdateArrow();
    }
    #endregion

    #region Pickups
    public void CollectPickup(PickUpItem pickup)
    {
        if (!pickups.Contains(pickup.transform)) return;

        pickups.Remove(pickup.transform);

        MinigameManager.Instance?.AddTime(pickup.Extratime);

        ScoreManager.Instance?.AddPoints(1);

        if (!string.IsNullOrEmpty(pickup.PickupSFXKey))
            audioManager?.PlaySFX(pickup.PickupSFXKey);


        //PlayPointSound();
        UpdateScoreUI();
        UpdateArrowTarget();

        if (ScoreManager.Instance.GetTotalPoints() >= maxScore)
            ActivateGoal();
    }
    #endregion

    #region Goal
    public void ReachedGoal()
    {
        if (winUI != null)
            winUI.SetActive(true);

        MinigameManager.Instance?.EndMinigame();

        if (MinigameManager.Instance != null)
        {
            InsigniaManager.Instance?.GuardarEstrella(MinigameManager.Instance.MinigameName, 1);
        }
    }
    #endregion

    #region Arrow
    private void UpdateArrowTarget()
    {
        if (ScoreManager.Instance == null) return;

        if (ScoreManager.Instance.GetTotalPoints() == maxScore - 1 && pickups.Count > 0)
        {
            lastPickup = pickups[0];
            arrowUI.gameObject.SetActive(true);
        }
        else
        {
            lastPickup = null;
            arrowUI.gameObject.SetActive(false);
        }
    }

    private void UpdateArrow()
    {
        if (!arrowUI || !arrowUI.gameObject.activeSelf || lastPickup == null) return;

        Vector3 dir = lastPickup.position - player.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        arrowUI.rectTransform.rotation = Quaternion.Euler(0, 0, -angle);
    }
    #endregion

    #region UI
    private void UpdateScoreUI()
    {
        if (scoreText != null && ScoreManager.Instance != null)
            scoreText.text = $"Points:{ScoreManager.Instance.GetTotalPoints()}/{maxScore}";
    }

    private void ActivateGoal()
    {
        if (goal != null)
            goal.SetActive(true);

        StartCoroutine(ShowNotification());
    }

    IEnumerator ShowNotification() 
    {
        if (notificationUI != null)
        {
            notificationUI.SetActive(true);
            yield return new WaitForSeconds(3f);
            notificationUI.SetActive(false);
        }
    }
    #endregion

    /*
    #region Audio
    void PlayPointSound()
    {
        if (audioSource != null && pointSound != null)
            audioSource.PlayOneShot(pointSound);
    }
    #endregion
    */
}