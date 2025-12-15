using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    //GENERICO PARA TODOS LOS MINIJUEGOS

    public static ScoreManager instance;

    private int score = 0;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"[ScoreManager] Score actual: {score}");
    }

    public int GetScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        Debug.Log("[ScoreManager] Score reseteado");
    }

}
