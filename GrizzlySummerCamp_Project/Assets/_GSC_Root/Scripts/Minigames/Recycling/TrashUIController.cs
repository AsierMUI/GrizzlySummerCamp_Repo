using UnityEngine;
using TMPro;

public class TrashUIController : MonoBehaviour
{
    [Header("Carry UI")]
    [SerializeField] private TMP_Text carryText;
    [SerializeField] private GameObject carryIcon;

    [Header("Counter UI")]
    [SerializeField] private TMP_Text counterText;

    private int totalTrash;
    private int remainingTrash;

    private void OnEnable()
    {
        TrashSpawner.OnTrashSpawned += SetTotalTrash;
        TrashPlayerCarry.OnTrashDelivered += OnTrashRemoved;
        TrashPlayerCarry.OnCarryStateChanged += UpdateCarryUI;
    }

    private void OnDisable()
    {
        TrashSpawner.OnTrashSpawned -= SetTotalTrash;
        TrashPlayerCarry.OnTrashDelivered -= OnTrashRemoved;
        TrashPlayerCarry.OnCarryStateChanged -= UpdateCarryUI;
    }

    void SetTotalTrash(int total)
    {
        totalTrash = total;
        remainingTrash = total;
        UpdateCounterUI();
    }

    void OnTrashRemoved()
    {
        remainingTrash = Mathf.Max(0, remainingTrash - 1);
        UpdateCounterUI();
    }

    void UpdateCarryUI(bool carrying)
    {
        if (carryIcon != null)
            carryIcon.SetActive(carrying);

        if (carryText != null)
            carryText.text = carrying ? "1" : "0";
    }

    void UpdateCounterUI()
    {
        if (counterText != null)
            counterText.text = $"Basura: {totalTrash - remainingTrash}/{totalTrash}";
    }
}