using UnityEngine;
using System.Collections.Generic;

public class FishingVisualSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FishingSkillCheck fishingSkillCheck;

    [Header("Fish Visuals (Apagados al inicio)")]
    [SerializeField] private List<GameObject> fishVisuals = new List<GameObject>();

    [Header("Behaviour")]
    [SerializeField] private bool randomFish = true;

    private int currentFishIndex = 0;

    private void Awake()
    {
        foreach (var fish in fishVisuals)
        {
            if (fish != null)
                fish.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (fishingSkillCheck != null)
            fishingSkillCheck.OnSkillCheckFinished += OnFishingFinished;
    }

    private void OnDisable()
    {
        if (fishingSkillCheck != null)
            fishingSkillCheck.OnSkillCheckFinished -= OnFishingFinished;
    }

    private void OnFishingFinished(bool success)
    {
        if (!success)
            return;

        ActivateFish();
    }

    private void ActivateFish()
    {
        if (fishVisuals.Count == 0)
            return;

        if (randomFish)
        {
            int index = Random.Range(0, fishVisuals.Count);
            fishVisuals[index].SetActive(true);
        }
        else
        {
            if (currentFishIndex >= fishVisuals.Count)
                return;

            fishVisuals[currentFishIndex].SetActive(true);
            currentFishIndex++;
        }
    }
}