using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverColorTMP : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Color hoverColor = Color.red;
    private Color originalColor;

    void Start()
    {
            originalColor = buttonText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
            buttonText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
            buttonText.color = originalColor;
    }
}
