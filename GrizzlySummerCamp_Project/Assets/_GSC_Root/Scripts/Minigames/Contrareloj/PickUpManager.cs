using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PickUpManager : MonoBehaviour
{
    // =====================================================
    // SINGLETON
    // =====================================================
    public static PickUpManager instance;

    // =====================================================
    // SISTEMA DE PUNTOS
    // =====================================================
    [Header("ScoreSystem")]
    public int score = 0;
    public int maxScore = 4;
    [SerializeField] TMP_Text scoreText; //UI que muestra los puntos (puede que se borre)

    // =====================================================
    // ELEMENTOS NIVEL
    // =====================================================
    [Header("Lvl elements")]
    [SerializeField] GameObject goal;
    [SerializeField] GameObject boat;
    private BoatMovement boatMovement;

    // =====================================================
    // UI
    // =====================================================
    [Header("UI")]
    [SerializeField] GameObject Notification;
    [SerializeField] GameObject winUI;
 
    // =====================================================
    // AUDIO
    // =====================================================
    [Header("Sounds")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pointSound;

    // =====================================================
    // SISTEMA FLECHA
    // =====================================================
    [Header("Arrow System")]
    [SerializeField] Image arrowUI;
    [SerializeField] Transform player;

    //Lista de pickups
    private List<Transform> pickups = new List<Transform>();
    private Transform lastPickup;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (boat != null)
            boatMovement = boat.GetComponent<BoatMovement>();

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
    }

    private void Update()
    {
        if (arrowUI != null && arrowUI.gameObject.activeSelf && lastPickup != null)
        {
            // La direccion del jugador hacia el pickup
            Vector3 dir = lastPickup.position - player.position;

            // Angulo calculado en gradios usando Atan2 
            float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

            //Gira la flecha
            arrowUI.rectTransform.rotation = Quaternion.Euler(0, 0, -angle);
        }
    }

    public void AddScore(Transform pickedObject)
    {
        score ++;
        Debug.Log("Score:" + score);

        PlayPointSound();
        UpdateScoreUI();

        //quitamos pickup de la lista
        pickups.Remove(pickedObject);

        int remaining = pickups.Count;

        if (remaining == 1)
        {
            lastPickup = pickups[0];
            arrowUI.gameObject.SetActive(true);
        }
        else if (remaining == 0)
        {
            arrowUI.gameObject.SetActive(false);
        }

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
        StartCoroutine(Notice());
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
            InsigniaManager.Instance.GuardarEstrella(1);
    }

    void PlayPointSound()
    {
        if (audioSource != null && pointSound != null)
            audioSource.PlayOneShot(pointSound);
    }
}
