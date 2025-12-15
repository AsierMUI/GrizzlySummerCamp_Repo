using UnityEngine;
using System;
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

    [Header("UI")]
    public GameObject panel;
    public RectTransform needle;
    public RectTransform successZone;
    public RectTransform barBackground;
    public TMP_Text difficultyText;
    public TMP_Text chainText;

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

    float barWidth;
    float needlePosX;
    int direction = 1; //la derecha 1 y la izquierda -1
    float timer;
    bool active;

    SkillDifficulty currentDifficulty;
    int requiredChecks;
    int currentSuccesses;

    float currentNeedleSpeed;
    float currentMaxTime;

    public Action<bool> OnSkillCheckFinished;

    private void Start()
    {
        panel.SetActive(false);
        barWidth = barBackground.rect.width;

        if (difficultyText) difficultyText.gameObject.SetActive(false);
        if (chainText) chainText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!active) return;

        timer += Time.deltaTime;
        if (timer >= currentMaxTime)
        {
            FailFishing();
            return;
        }

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

        needle.anchoredPosition = new Vector2 (needlePosX, needle.anchoredPosition.y);

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            CheckResult();
        }
    }

    public void StartSkillCheck()
    {
        panel.SetActive(true);
        active = true;

        RollDifficultyByChance();
        ApplyDifficultySettings();

        currentSuccesses = 0;
        UpdateDifficultyUI();
        UpdateChainUI();

        StartSingleSkillCheck();
    }

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

    void StartSingleSkillCheck()
    {
        timer = 0f;
        direction = 1;
        needlePosX = 0;

        needle.anchoredPosition = new Vector2(needlePosX, needle.anchoredPosition.y);

        RandomizeSuccessZone();
    }

    void CheckResult()
    {
        bool success = 
            needlePosX >= successZone.anchoredPosition.x &&
            needlePosX <= successZone.anchoredPosition.x + successZone.rect.width;

        if (success )
        {
            currentSuccesses++;
            UpdateChainUI();

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
        EndSkillCheck(true);
    }

    void FailFishing()
    {
        EndSkillCheck(false);
    }

    void EndSkillCheck(bool success)
    {
        active = false;
        panel.SetActive(false);

        if (difficultyText) difficultyText.gameObject.SetActive(false);
        if (chainText) chainText.gameObject.SetActive(false);

        OnSkillCheckFinished?.Invoke(success);
    }

    void RandomizeSuccessZone()
    {
        float maxX = barWidth - successZone.rect.width;
        float randomX = UnityEngine.Random.Range(0, maxX);
        successZone.anchoredPosition = new Vector2(randomX, successZone.anchoredPosition.y);
    }

    void UpdateDifficultyUI()
    {
        if (!difficultyText) return;

        difficultyText.gameObject.SetActive(true);

        difficultyText.text = currentDifficulty switch
        {
            SkillDifficulty.Easy => "Easy",
            SkillDifficulty.Medium => "Medium",
            SkillDifficulty.Hard => "Hard",
            _ => ""
        };
    }

    void UpdateChainUI()
    {
        if (!chainText) return;

        chainText.gameObject.SetActive(true);
        chainText.text = $"{currentSuccesses}/{requiredChecks}";
    }
}
