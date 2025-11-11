using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager instance;

    public int score = 0;
    public int maxScore = 4;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] GameObject goal;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject boat;

    private BoatMovement boatMovement;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (boat != null)
        {
            boatMovement = boat.GetComponent<BoatMovement>();
        }
    }

    private void Start()
    {
        UpdateScoreUI();
    }

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score:" + score);

        UpdateScoreUI();

        if (score >= maxScore)
            ActivateGoal();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = $"Points:{score}/{maxScore}";
    }

    void ActivateGoal()
    {
        goal.SetActive(true);
    }

    public void ReachedGoal()
    {
        if (winUI != null)
            winUI.SetActive(true);

        boatMovement.enabled = false;

        if (InsigniaManager.Instance != null)
        {
            InsigniaManager.Instance.GuardarEstrella(1);
        }
    }    
}
