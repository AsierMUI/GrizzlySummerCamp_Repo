using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FishingUIController : MonoBehaviour
{
    [Header("UIs")]
    public GameObject fishingUI;
    public GameObject miniResultUI;
    public GameObject finalUI;

    [Header("Labels")]
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI dificultadText;
    public TextMeshProUGUI mensajeFinalText;
    public TextMeshProUGUI pointsText;

    [Header("Insignias")]
    public Image insigniaImage;
    public Sprite insigniaBronce;
    public Sprite insigniaPlata;
    public Sprite insigniaOro;

    public void ShowFishingUI() => fishingUI.SetActive(true);
    public void HideFishingUI() => fishingUI.SetActive(false);

    public void ShowResult(string text)
    {
        miniResultUI.SetActive(true);
        resultText.text = text;
    }

    public void HideResult() => miniResultUI.SetActive(false);

    public void ShowFinal(string msg)
    {
        finalUI.SetActive(true);
        mensajeFinalText.text = msg;
    }

    public void UpdateDificulty(string text) => dificultadText.text = text;

    public void UpdatePoints(int points)
    {
        pointsText.text = "Points:" + points;

        if (points >= 300) insigniaImage.sprite = insigniaOro;
        else if (points >= 200) insigniaImage.sprite = insigniaPlata;
        else if (points >= 100) insigniaImage.sprite = insigniaBronce;
        else insigniaImage.sprite = null;
    }

}

