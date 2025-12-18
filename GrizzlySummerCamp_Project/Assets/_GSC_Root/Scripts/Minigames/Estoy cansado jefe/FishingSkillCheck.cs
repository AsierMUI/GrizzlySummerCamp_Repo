using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

public class FishingSkillCheck : MonoBehaviour
{
    //Este script se encarga SOLO del minijuego, su dificultad e interfaz

    public enum SkillDifficulty
    {
        Easy,
        Medium,
        Hard
    }

    [Header("Skillcheck UI")]
    public GameObject panel;
    public RectTransform needle;
    public RectTransform successZone;
    public RectTransform barBackground;
    public TMP_Text difficultyText;
    public TMP_Text chainText;

    [Header("Progress UI")]
    public RectTransform progressBarBG;
    public RectTransform fishMarker;

    [Header("Result UI")]
    public GameObject resultPanel;
    public TMP_Text resultText;
    public float resultDelay = 2f;

    [Header("Timer UI")]
    public RectTransform timerBarBG;
    public Image timerFill;

    [Header("Dificulty Chances (%)")]
    [Range(0, 100)] public int easyChance = 60;
    [Range(0, 100)] public int mediumChance = 30;
    [Range(0, 100)] public int hardChance = 10;

    [Header("Needle Speed")]
    public float easyNeedleSpeed = 300f;
    public float mediumNeedleSpeed = 450f;
    public float hardNeedleSpeed = 650f;

    [Header("Zone Width")]
    public float easyZoneWidth = 100f;
    public float mediumZoneWidth = 70f;
    public float hardZoneWidth = 40f;

    [Header("Max Time")]
    public float easyMaxTime = 10f;
    public float mediumMaxTime = 8f;
    public float hardMaxTime = 6f;

    [Header("Skillchecks Required")]
    public int easyRequired = 2;
    public int mediumRequired = 3;
    public int hardRequired = 4;

    [Header("Score")]
    [SerializeField] private FishingScoreHandler scoreHandler;

    float barWidth;
    float needlePosX;
    int direction = 1; //la derecha 1 y la izquierda -1
    float timer;
    bool active;
    bool ending;

    SkillDifficulty currentDifficulty;
    int requiredChecks;
    int currentSuccesses;

    float currentNeedleSpeed;
    float currentMaxTime;

    public Action<bool> OnSkillCheckFinished;

    private void Start()
    {
        panel.SetActive(false);
        if (resultPanel) resultPanel.SetActive(false);

        barWidth = barBackground.rect.width;

        if (difficultyText) difficultyText.gameObject.SetActive(false);
        if (chainText) chainText.gameObject.SetActive(false);

        if (scoreHandler == null)
        {
            scoreHandler = FindFirstObjectByType<FishingScoreHandler>(FindObjectsInactive.Include);
        }
        if (scoreHandler == null)
            Debug.LogError("No se encuentra fishingScoreHandler en la escena");

        ResetFishPosition();
    }

    private void Update()
    {
        if (!active || ending) return;

        timer += Time.deltaTime;

        UpdateTimerUI();

        if (timer >= currentMaxTime)
        {
            FailFishing();
            return;
        }

        //Movimiento aguja
        needlePosX += currentNeedleSpeed * direction * Time.deltaTime;

        if (needlePosX <= 0)
        {
            needlePosX = 0;
            direction = 1;
        }
        else if (needlePosX >= barWidth)
        {
            needlePosX = barWidth;
            direction = -1;
        }

        needle.anchoredPosition = new Vector2(needlePosX, needle.anchoredPosition.y);

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            CheckResult();
        }
    }

    //Entrada al minijuego
    public void StartSkillCheck()
    {
        panel.SetActive(true);
        if (resultPanel) resultPanel.SetActive(false);

        active = true;
        ending = false;

        if (MinigameManager.Instance != null)
            MinigameManager.Instance.SetPlayerMovement(false);

        RollDifficultyByChance();
        ApplyDifficultySettings();

        currentSuccesses = 0;
        UpdateProgressBar();
        UpdateDifficultyUI();
        UpdateChainUI();     

        StartSingleSkillCheck();
    }

    //Dificultad por %
    void RollDifficultyByChance()
    {
        int roll = UnityEngine.Random.Range(1, 101);

        if (roll <= easyChance)
            currentDifficulty = SkillDifficulty.Easy;
        else if (roll <= easyChance + mediumChance)
            currentDifficulty = SkillDifficulty.Medium;
        else
            currentDifficulty = SkillDifficulty.Hard;
    }

    void ApplyDifficultySettings()
    {
        switch (currentDifficulty)
        {
            case SkillDifficulty.Easy:
                currentNeedleSpeed = easyNeedleSpeed;
                currentMaxTime = easyMaxTime;
                requiredChecks = easyRequired;
                successZone.sizeDelta = new Vector2(easyZoneWidth, successZone.sizeDelta.y);
                break;

            case SkillDifficulty.Medium:
                currentNeedleSpeed = mediumNeedleSpeed;
                currentMaxTime = mediumMaxTime;
                requiredChecks = mediumRequired;
                successZone.sizeDelta = new Vector2(mediumZoneWidth, successZone.sizeDelta.y);
                break;

            case SkillDifficulty.Hard:
                currentNeedleSpeed = hardNeedleSpeed;
                currentMaxTime = hardMaxTime;
                requiredChecks = hardRequired;
                successZone.sizeDelta = new Vector2(hardZoneWidth, successZone.sizeDelta.y);
                break;
        }
    }
    //Skillcheck individual
    void StartSingleSkillCheck()
    {
        timer = 0f;
        direction = 1;
        needlePosX = 0;

        needle.anchoredPosition = new Vector2(needlePosX, needle.anchoredPosition.y);

        if (timerFill)
            timerFill.fillAmount = 1f;

        RandomizeSuccessZone();
    }

    void CheckResult()
    {
        bool success = 
            needlePosX >= successZone.anchoredPosition.x &&
            needlePosX <= successZone.anchoredPosition.x + successZone.rect.width;

        if (success)
        {
            currentSuccesses++;
            UpdateChainUI();
            UpdateProgressBar();

            if (currentSuccesses >= requiredChecks)
            {
                CompleteFishing();
            }
            else
            {
                StartSingleSkillCheck();
            }
        }
        else
        {
            FailFishing();
        }
    }

    void CompleteFishing()
    {
        if (scoreHandler != null)
        {
            scoreHandler.OnFishCaptured(currentDifficulty);
        }
        else
        {
            Debug.LogWarning("[FishingSkillCheck] FishingScoreHandler no asignado");
        }

        StartCoroutine(EndWithResult(true));
    }

    void FailFishing()
    {
        StartCoroutine(EndWithResult(false));
    }

    IEnumerator EndWithResult(bool success)
    {
        ending = true;
        active = false;
        
        if (resultPanel && resultText)
        {
            resultPanel.SetActive(true);
            resultText.text = success ? "Fish caught!" : "It escaped!";
            resultText.color = success ? Color.green : Color.red;
        }

        yield return new WaitForSeconds(resultDelay);

        panel.SetActive(false);
        if (resultPanel) resultPanel.SetActive(false);

        ResetFishPosition();
        ending = false;

        if (MinigameManager.Instance != null) MinigameManager.Instance.SetPlayerMovement(true);

        OnSkillCheckFinished?.Invoke(success);
    }

    void RandomizeSuccessZone()
    {
        float maxX = barWidth - successZone.rect.width;
        float randomX = UnityEngine.Random.Range(0, maxX);
        successZone.anchoredPosition = new Vector2(randomX, successZone.anchoredPosition.y);
    }

    //Progreso pez
    void UpdateProgressBar()
    {
        if (!progressBarBG || !fishMarker) return;

        float progress = (float)currentSuccesses / requiredChecks;
        float barHeight = progressBarBG.rect.height;
        float yPos = progress * barHeight;

        fishMarker.anchoredPosition = new Vector2(fishMarker.anchoredPosition.x, yPos);
    }

    void ResetFishPosition()
    {
        if (!fishMarker) return;

        fishMarker.anchoredPosition = new Vector2(fishMarker.anchoredPosition.x, 0f);
    }

    void UpdateDifficultyUI()
    {
        if (!difficultyText) return;

        difficultyText.gameObject.SetActive(true);
        difficultyText.text = currentDifficulty.ToString();
    }

    void UpdateChainUI()
    {
        if (!chainText) return;

        chainText.gameObject.SetActive(true);
        chainText.text = $"{currentSuccesses}/{requiredChecks}";
    }

    void UpdateTimerUI()
    {
        if (!timerFill) return;

        float normalizedTime = 1f - (timer / currentMaxTime);
        timerFill.fillAmount = normalizedTime;
    }
}
