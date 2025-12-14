using UnityEngine;
using System;
using TMPro;

public class FishingSkillCheck : MonoBehaviour
{
    //ESTE SCRIPT SE ENCARGA DE LAS DIFICULTADES E INTERFAZ DEL MINIJUEGO

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

    [Header("Dificulty Chances (%)")]
    [Range(0, 100)] public int easyChance = 60;
    [Range(0, 100)] public int mediumChance = 30;
    [Range(0, 100)] public int hardChance = 10;

    [Header("Difficulty Setting")]
    public float easyNeedleSpeed = 300f;
    public float mediumNeedleSpeed = 450f;
    public float hardNeedleSpeed = 650f;

    public float easyZoneWidth = 100f;
    public float mediumZoneWidth = 70f;
    public float hardZoneWidth = 40f;

    public float easyMaxTime = 10f;
    public float mediumMaxTime = 8f;
    public float hardMaxTime = 6f;

    float barWidth;
    float needlePosX;
    int direction = 1; //la derecha 1 y la izquierda -1
    float timer;
    bool active;

    SkillDifficulty currentDifficulty = SkillDifficulty.Easy;

    float currentNeedleSpeed;
    float currentMaxTime;

    public Action<bool> OnSkillCheckFinished;

    private void Start()
    {
        panel.SetActive(false);
        barWidth = barBackground.rect.width;

        if (difficultyText != null )
            difficultyText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!active) return;

        timer += Time.deltaTime;
        if (timer >= currentMaxTime)
        {
            EndSkillCheck(false);
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
        UpdateDifficultyUI();

        timer = 0f;
        direction = 1;

        needlePosX = 0;
        needle.anchoredPosition = new Vector2(0, needle.anchoredPosition.y);

        RandomizeSuccessZone();
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
                successZone.sizeDelta = new Vector2(easyZoneWidth, successZone.sizeDelta.y);
                break;

            case SkillDifficulty.Medium:
                currentNeedleSpeed = mediumNeedleSpeed;
                currentMaxTime = mediumMaxTime;
                successZone.sizeDelta = new Vector2(mediumZoneWidth, successZone.sizeDelta.y);
                break;

            case SkillDifficulty.Hard:
                currentNeedleSpeed = hardNeedleSpeed;
                currentMaxTime = hardMaxTime;
                successZone.sizeDelta = new Vector2(hardZoneWidth, successZone.sizeDelta.y);
                break;
        }
    }

    void UpdateDifficultyUI()
    {
        if (difficultyText == null) return;

        difficultyText.gameObject.SetActive(true);

        switch (currentDifficulty)
        {
            case SkillDifficulty.Easy:
                difficultyText.text = "Easy";
                difficultyText.color = Color.green;
                break;

            case SkillDifficulty.Medium:
                difficultyText.text = "Medium";
                difficultyText.color = Color.yellow;
                break;

            case SkillDifficulty.Hard:
                difficultyText.text = "Hard";
                difficultyText.color = Color.red;
                break;
        }
    }

    void RandomizeSuccessZone()
    {
        float maxX = barWidth - successZone.rect.width;
        float randomX = UnityEngine.Random.Range(0, maxX);
        successZone.anchoredPosition = new Vector2 (randomX, successZone.anchoredPosition.y);
    }

    public void CheckResult()
    {
        bool success =
            needlePosX >= successZone.anchoredPosition.x &&
            needlePosX <= successZone.anchoredPosition.x + successZone.rect.width;

        EndSkillCheck(success);
    }

    void EndSkillCheck(bool success)
    {
        active = false;
        panel.SetActive(false);

        if (difficultyText != null)
            difficultyText.gameObject.SetActive(false);

        OnSkillCheckFinished?.Invoke(success);
    }

}
