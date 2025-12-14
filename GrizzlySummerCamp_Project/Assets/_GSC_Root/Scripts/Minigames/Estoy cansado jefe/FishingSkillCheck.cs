using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class FishingSkillCheck : MonoBehaviour
{
    [Header("UI")]
    public GameObject panel;
    public RectTransform needle;
    public RectTransform successZone;
    public RectTransform barBackground;

    [Header("Setting")]
    public float needleSpeed = 400f;
    public float maxTime = 10f;

    float barWidth;
    float needlePosX;
    int direction = 1; //la derecha 1 y la izquierda -1
    float timer;
    bool active;

    public Action<bool> OnSkillCheckFinished;

    private void Start()
    {
        panel.SetActive(false);
        barWidth = barBackground.rect.width;
    }

    private void Update()
    {
        if (!active) return;

        timer += Time.deltaTime;
        if(timer >= maxTime)
        {
            EndSkillCheck(false);
            return;
        }

        needlePosX += needleSpeed * direction * Time.deltaTime;

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

        timer = 0f;
        direction = 1;

        needlePosX = 0;
        needle.anchoredPosition = new Vector2(0, needle.anchoredPosition.y);

        RandomizeSuccessZone();
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
        OnSkillCheckFinished?.Invoke(success);
    }

}
