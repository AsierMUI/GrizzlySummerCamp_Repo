using UnityEngine;
public class MinigameManager : MonoBehaviour
{

    //GENERICO SIRVE PARA TODOS LOS MINIJUEGOS

    public static MinigameManager Instance;

    public enum MinigameState
    {
        WaitingToStart,
        Playing,
        Finished
    }

    [Header("State")]
    public MinigameState CurrentState { get; private set; }

    [Header("Player")]
    [SerializeField] private MonoBehaviour playerMovementBehaviour;
    private IMinigamePlayerMovement playerMovement;

    [Header("UI")]
    [SerializeField] private EndMinigameUI endUI;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CurrentState = MinigameState.WaitingToStart;

        playerMovement = playerMovementBehaviour as IMinigamePlayerMovement;

        if (playerMovement != null)
            playerMovement.DisableMovement();

    }

    public void SetPlayerMovement(MonoBehaviour movement)
    {
        playerMovementBehaviour = movement;
        playerMovement = movement as IMinigamePlayerMovement;
    }

    public void OnMinigameStarted()
    {
        CurrentState = MinigameState.Playing;

        playerMovement.EnableMovement();
        ScoreManager.Instance.ResetScore();
    }

    public void EndMinigame()
    {
        if (CurrentState == MinigameState.Finished) return;

        CurrentState = MinigameState.Finished;

        playerMovement.DisableMovement();

        int finalScore = ScoreManager.Instance.GetScore();
        int insignia = InsigniaManager.Instance.CalcularInsignia(finalScore);

        InsigniaManager.Instance.GuardarInsignia(insignia);
        endUI.ShowResult(insignia);
    }
}