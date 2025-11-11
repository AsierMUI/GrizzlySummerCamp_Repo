using UnityEngine;
using UnityEngine.UI;

public class PickUpManager : MonoBehaviour
{
    public static PickUpManager instance;

    public int score = 0;
    public int maxScore = 4;

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

    public void AddScore(int value)
    {
        score += value;
        Debug.Log("Score:" + score);

        if (score >= maxScore)
            ActivateGoal();
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
