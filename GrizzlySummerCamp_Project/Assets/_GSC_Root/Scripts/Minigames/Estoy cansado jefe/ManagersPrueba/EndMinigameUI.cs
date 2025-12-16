using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndMinigameUI : MonoBehaviour
{
    //Generico para todos los minijuegos

    [Header("UI")]
    [SerializeField] private Image insigniaImage;
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Sprites")]
    [SerializeField] private Sprite bronce;
    [SerializeField] private Sprite plata;
    [SerializeField] private Sprite oro;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowResult(int insignia)
    {
        gameObject.SetActive(true);

        switch (insignia)
        {
            case 1:
                insigniaImage.sprite = bronce;
                resultText.text = "Nice!";
                break;

            case 2:
                insigniaImage.sprite = plata;
                resultText.text = "Well done!";
                break;

            case 3:
                insigniaImage.sprite = oro;
                resultText.text = "Wow!";
                break;

            default:
                resultText.text = "Oops :(";
                break;
        }
    }

}
