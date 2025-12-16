using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    //GENERICO PARA TODOS LOS MINIJUEGOS

    public static ScoreManager Instance;

    public static System.Action<int> OnScoreChanged;

    private int score;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        OnScoreChanged?.Invoke(score);
        Debug.Log($"[ScoreManager] Score actual: {score}");
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        OnScoreChanged?.Invoke(score);
        Debug.Log("[ScoreManager] Score reseteado");
    }

}
