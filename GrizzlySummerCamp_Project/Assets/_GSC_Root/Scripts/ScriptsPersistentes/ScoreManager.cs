using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{

    //GENERICO PARA TODOS LOS MINIJUEGOS

    public static ScoreManager Instance;

    [Header("Score")]
    [SerializeField] private int puntosTotales = 0;

    [Header("UI")]
    [SerializeField] private TMP_Text scoreText;

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

    private void Start()
    {
        UpdateUI();
    }

    public void AddPoints(int amount)
    {
        puntosTotales += amount;
        UpdateUI();

        Debug.Log($"[ScoreManager] Puntos actuales: {puntosTotales}");
    }

    public int GetTotalPoints()
    {
        return puntosTotales;
    }

    public void ResetScore()
    {
        puntosTotales = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null )
            scoreText.text = puntosTotales.ToString();
    }

}