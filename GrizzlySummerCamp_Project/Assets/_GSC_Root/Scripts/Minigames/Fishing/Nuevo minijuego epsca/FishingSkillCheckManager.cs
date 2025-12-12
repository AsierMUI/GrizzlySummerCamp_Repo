using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class FishingSkillCheckManager : MonoBehaviour
{
    [Header("Progress")]
    [SerializeField] private Slider progressSlider;
    [SerializeField] private float progressPerHit = 0.15f;
    [SerializeField] private float progressLossSpeed = 0.1f;

    [Header("Skill Check Settings")]
    [SerializeField] private SkillCheckSpawner spawner;
    [SerializeField] private int minChecks = 4;
    [SerializeField] private int maxChecks = 8;

    private int checksRemaining;
    private bool isActive;

    public System.Action OnWin;
    public System.Action OnLose;

    private void Update()
    {
        if (!isActive) return;

        progressSlider.value -= progressLossSpeed * Time.deltaTime;

        if (progressSlider.value <= 0f)
        {
            Lose();
        }
    }

    public void StartSkillCheck(int dificultad)
    {
        isActive = true;
        progressSlider.value = 0f;

        switch (dificultad)
        {
            case 0:
                checksRemaining = minChecks;
                break;
            case 1:
                checksRemaining = (minChecks + maxChecks) / 2; 
                break;
            case 2:
                checksRemaining = maxChecks;
                break;
        }

        SpawnNextTarget();
    }

    public void HitSuccess()
    {
        progressSlider.value += progressPerHit;
        checksRemaining--;

        if (progressSlider.value >=1f)
        {
            Win();
            return;
        }

        if (checksRemaining > 0)
            SpawnNextTarget();
    }

    private void SpawnNextTarget()
    {
        spawner.SpawnTarget(this);
    }

    private void Win()
    {
        isActive = false;
        OnWin?.Invoke();
    }

    private void Lose()
    {
        isActive = false;
        OnLose?.Invoke();
    }
}
