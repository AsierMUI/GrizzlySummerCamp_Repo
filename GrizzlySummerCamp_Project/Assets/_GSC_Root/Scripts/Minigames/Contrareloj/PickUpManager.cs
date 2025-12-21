using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PickUpManager : MonoBehaviour
{
    #region Singleton
    public static PickUpManager instance;
    #endregion

    #region Score
    [Header("Score System")]
    public int score = 0;
    public int maxScore = 4;
    [SerializeField] private TMP_Text scoreText;
    #endregion

    #region Level Elements
    [Header("Lvl elements")]
    [SerializeField] private GameObject goal;
    #endregion

    #region UI
    [Header("UI")]
    [SerializeField] private GameObject notificationUI;
    [SerializeField] private GameObject winUI;
    #endregion

    #region Sounds
    [Header("Sounds")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pointSound;
    #endregion

    #region Arrow System
    [Header("Arrow System")]
    [SerializeField] private Image arrowUI;
    [SerializeField] private Transform player;
    #endregion

    #region Minigame Info
    [Header("Minigame Info")]
    [SerializeField] string minigameName = "Race";
    #endregion

    #region Pickups Data
    private List<Transform> pickups = new List<Transform>();
    private Transform lastPickup;
    #endregion

    #region Awake, Start y Update
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateScoreUI();

        //Busca los pickups por tag
        GameObject[] initialPickups = GameObject.FindGameObjectsWithTag("Pickup");
        foreach (var p in initialPickups)
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

    #region AddScore y ReachedGoal
    public void AddScore(Transform pickedObject)
    {
        score ++;
        Debug.Log($"[PickUpManager] Score: + {score}");

        PlayPointSound();
        UpdateScoreUI();

        pickups.Remove(pickedObject);

        UpdateArrowTarget();

        if (score >= maxScore)
            ActivateGoal();
    }

    public void ReachedGoal()
    {
        Debug.Log("[PickUpManager] Meta alcanzada");

        if (winUI != null)
            winUI.SetActive(true);

        MinigameManager.Instance?.EndMinigame();

        if (InsigniaManager.Instance != null)
        {
            InsigniaManager.Instance.GuardarEstrella(minigameName, 1);
            Debug.Log($"[PickUpManager] Estrella guardada para {minigameName}");
        }
        else
        {
            Debug.Log("[PickUpManager] InsigniaMaanger no encontrado");
        }
    }
    #endregion

    #region Arrow
    private void UpdateArrowTarget()
    {
        int remaining = pickups.Count;

        if (remaining == 1)
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
        if (arrowUI == null || !arrowUI.gameObject.activeSelf || lastPickup == null) return;

        Vector3 dir = lastPickup.position - player.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        arrowUI.rectTransform.rotation = Quaternion.Euler(0, 0, -angle);
    }
    #endregion

    #region UI
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Points:{score}/{maxScore}";
    }

    void ActivateGoal()
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

    #region Audio
    void PlayPointSound()
    {
        if (audioSource != null && pointSound != null)
            audioSource.PlayOneShot(pointSound);
    }
    #endregion
}