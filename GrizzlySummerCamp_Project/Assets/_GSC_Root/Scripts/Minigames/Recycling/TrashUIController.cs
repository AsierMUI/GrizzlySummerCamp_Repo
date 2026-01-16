using UnityEngine;
using TMPro;

public class TrashUIController : MonoBehaviour
{
    [Header("Carry UI Icons")]
    [SerializeField] private GameObject carryIcon;
    [SerializeField] private UnityEngine.UI.Image carryImage;
    [SerializeField] private Sprite organicaIcon;
    [SerializeField] private Sprite papelIcon;
    [SerializeField] private Sprite plasticoIcon;
    [SerializeField] private Sprite cristalIcon;


    [Header("Counter UI")]
    [SerializeField] private TMP_Text counterText;

    private int totalTrash;
    private int remainingTrash;

    private void OnEnable()
    {
        TrashSpawner.OnTrashSpawned += SetTotalTrash;
        TrashPlayerCarry.OnTrashDelivered += OnTrashRemoved;
        TrashPlayerCarry.OnCarryChanged += UpdateCarryUI;
    }

    private void OnDisable()
    {
        TrashSpawner.OnTrashSpawned -= SetTotalTrash;
        TrashPlayerCarry.OnTrashDelivered -= OnTrashRemoved;
        TrashPlayerCarry.OnCarryChanged -= UpdateCarryUI;
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
    void UpdateCarryUI(TrashType? type)
    {
        if (carryIcon != null)
            carryIcon.SetActive(type.HasValue);

        if (!type.HasValue || carryImage == null)
            return;

        carryImage.sprite = type.Value switch
        {
            TrashType.Organica => organicaIcon,
            TrashType.Papel => papelIcon,
            TrashType.Plastico => plasticoIcon,
            TrashType.Cristal => cristalIcon,
            _ => null
        };
    }

    void UpdateCounterUI()
    {
        if (counterText != null)
            counterText.text = $"Basura: {totalTrash - remainingTrash}/{totalTrash}";
    }
}