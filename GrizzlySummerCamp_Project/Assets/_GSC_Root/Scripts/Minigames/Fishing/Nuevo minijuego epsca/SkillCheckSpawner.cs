using UnityEngine;

public class SkillCheckSpawner : MonoBehaviour
{
    [Header("Skillcheck UI")]
    [SerializeField] private GameObject skillcheckUI;

    [Header("Target Element Inside UI")]
    [SerializeField] private RectTransform targetElement;

    [Header("Spawn Area")]
    [SerializeField] private RectTransform spawnArea;

    public void ShowSkillcheckUI(FishingSkillCheckManager manager)
    {
        skillcheckUI.SetActive(true);

        SkillCheckTarget target = targetElement.GetComponent<SkillCheckTarget>();
        target.Initialize(manager);

        Vector2 rnd = new Vector2(
            Random.Range(-spawnArea.rect.width / 2, spawnArea.rect.width / 2),
            0
        );

        targetElement.anchoredPosition = rnd;
    }

    public void HideSkillcheckUI()
    {
        skillcheckUI.SetActive(false);
    }

}
