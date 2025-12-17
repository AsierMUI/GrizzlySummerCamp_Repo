using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class MinigameManager : MonoBehaviour
{

    //GENERICO SIRVE PARA TODOS LOS MINIJUEGOS

    public static MinigameManager Instance;

    [Header("Minigame Settings")]
    [SerializeField] public string minigameName;
    [SerializeField] private float minigameDuration = 90f;
    private float currentTime;
    private bool isRunning = false;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject endMinigameUI;
    [SerializeField] private TMP_Text finalScoreText;

    [Header("End Game Message")]
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private Image insigniaImage;

    [Header("Insignia Sprites")]
    [SerializeField] private Sprite bronzeSprite;
    [SerializeField] private Sprite silverSprite;
    [SerializeField] private Sprite goldSprite;
    
    private IMinigamePlayerMovement playerMovement;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindPlayerMovementInScene();
    }

    void FindPlayerMovementInScene()
    {
        playerMovement = FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
            )
            .OfType<IMinigamePlayerMovement>()
            .FirstOrDefault();

        if (playerMovement != null)
        {
            Debug.Log($"[MinigameManager] PlayerMovement detectado {playerMovement}");
            playerMovement.SetCanMove(false);
        }
        else
        {
            Debug.LogWarning("[MinigameManager] no se encontro IMinigamePlayerMovement en la escena");
        }
    }

    private void Update()
    {
        if (!isRunning) return;

        currentTime -=Time.deltaTime;
        UpdateTimerUI();

        if (currentTime <=0f)
        {
            EndMinigame();
        }
    }

    public void OnMinigameStarted()
    {
        currentTime = minigameDuration;
        isRunning = true;

        SetPlayerMovement(true);

        if (ScoreManager.Instance != null)
            ScoreManager.Instance.ResetScore();
    }

    public void StartMinigame()
    {
        OnMinigameStarted(); //lo dejo pq estoy cansao de cambiar scripts
    }

    public void SetPlayerMovement(bool canMove)
    {
        if (playerMovement != null) playerMovement.SetCanMove(canMove);
    }

    public void EndMinigame()
    {
        isRunning = false;

        SetPlayerMovement(false);

        int puntos = ScoreManager.Instance != null ? ScoreManager.Instance.GetTotalPoints() : 0;
        int insignia = GetInsigniaByScore(puntos);

        if (minigameName == "Race")
        {
            int estrella = puntos > 0 ? 1 : 0;
            InsigniaManager.Instance.GuardarEstrella(minigameName, estrella);
        }
        else
        {
            InsigniaManager.Instance.GuardarInsignia(minigameName, insignia);
        }

        if (finalScoreText)
            finalScoreText.text = $"You got {puntos} points!";

        UpdateEndGameUI(insignia);

        if (endMinigameUI)
            endMinigameUI.SetActive(true);
    }

    void UpdateEndGameUI(int insignia)
    {
        if (messageText)
            messageText.text = GetMessageByInsignia(insignia);

        if (insigniaImage)
        {
            insigniaImage.sprite = GetSpriteByInsignia(insignia);
            insigniaImage.enabled = insignia > 0;
        }
    }

    string GetMessageByInsignia(int insignia)
    {
        switch (insignia)
        {
            case 3: return "Wow!";
            case 2: return "Great job!";
            case 1: return "Nice try;";
            default: return "Better luck next time!";
        }
    }

    Sprite GetSpriteByInsignia(int insignia)
    {
        switch (insignia)
        {
            case 3: return goldSprite;
            case 2: return silverSprite;
            case 1: return bronzeSprite;
            default: return null;
        }
    }

    int GetInsigniaByScore(int score)
    {
        if (score >= 200) return 3;
        if (score >= 150) return 2;
        if (score >= 50) return 1;
        return 0;
    }

    void UpdateTimerUI()
    {
        if (!timerText) return;

        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = seconds.ToString();
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public float GetMaxTime()
    {
        return minigameDuration;
    }
}