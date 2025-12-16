using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class MinigameManager : MonoBehaviour
{

    //GENERICO SIRVE PARA TODOS LOS MINIJUEGOS

    public static MinigameManager Instance;

    [Header("Minigame Time")]
    [SerializeField] private float minigameDuration = 90f;
    private float currentTime;
    private bool isRunning = false;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private GameObject endMinigameUI;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text finalInsigniaText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentTime = minigameDuration;
        UpdateTimerUI();
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

    public void StartMinigame()
    {
        currentTime = minigameDuration;
        isRunning = true;

        ScoreManager.Instance.ResetScore();
    }

    public void EndMinigame()
    {
        isRunning = false;

        int puntos = ScoreManager.Instance.GetTotalPoints();

        if (finalScoreText)
            finalScoreText.text = puntos.ToString();

        int insignia = GetInsigniaByScore(puntos);

        InsigniaManager.Instance.GuardarInsignia(insignia);

        if (finalInsigniaText)
            finalInsigniaText.text = GetInsigniaText(insignia);

        if (endMinigameUI)
            endMinigameUI.SetActive(true);
    }

    int GetInsigniaByScore(int score)
    {
        if (score >= 300) return 3;
        if (score >= 150) return 2;
        if (score >= 50) return 1;
        return 0;
    }

    string GetInsigniaText(int insignia)
    {
        switch (insignia)
        {
            case 3: return "Gold Badge!";
            case 2: return "Silver Badge!";
            case 1: return "Bronce Badge!";
            default: return "Oops :(";
        }
    }

    void UpdateTimerUI()
    {
        if (!timerText) return;

        int seconds = Mathf.CeilToInt(currentTime);
        timerText.text = seconds.ToString();
    }
}