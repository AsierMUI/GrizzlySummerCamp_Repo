using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    // Generico para todos los minijuegos
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        ScoreManager.OnScoreChanged += UpdateScore;
    }

    private void OnDisable()
    {
        ScoreManager.OnScoreChanged -= UpdateScore;
    }

    void UpdateScore(int newScore)
    {
        scoreText.text = $"Puntos: {newScore}";
    }

}
