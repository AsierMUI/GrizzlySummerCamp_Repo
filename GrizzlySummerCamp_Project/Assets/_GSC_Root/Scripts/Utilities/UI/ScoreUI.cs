using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    // Generico para todos los minijuegos
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        if (ScoreManager.Instance != null)
        {
            scoreText.text = $"Points: {ScoreManager.Instance.GetTotalPoints()}";
        }
    }
}