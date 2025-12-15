using UnityEngine;
public class MinigameManager : MonoBehaviour
{

    //GENERICO SIRVE PARA TODOS LOS MINIJUEGOS

    public static MinigameManager instance;

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

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        CurrentState = MinigameState.WaitingToStart;

        playerMovement = playerMovementBehaviour as IMinigamePlayerMovement;

        if (playerMovement != null)
            playerMovement.DisableMovement();

        if (MinigameTimer.instance != null)
            MinigameTimer.instance.OnTimerFinished += OnTimeFinished;
    }

    public void SetPlayerMovement(MonoBehaviour movement)
    {
        playerMovementBehaviour = movement;
        playerMovement = movement as IMinigamePlayerMovement;
    }

    public void OnMinigameStarted()
    {
        if (CurrentState != MinigameState.WaitingToStart) return;

        CurrentState = MinigameState.Playing;

        if (playerMovement !=null)
            playerMovement.EnableMovement();

        if (ScoreManager.instance != null)
            ScoreManager.instance.ResetScore();
    }

    private void OnTimeFinished()
    {
        EndMinigame();
    }

    public void EndMinigame()
    {
        if (CurrentState == MinigameState.Finished) return;

        CurrentState = MinigameState.Finished;

        if (playerMovement != null)
            playerMovement.DisableMovement();

        if (InsigniaManager.Instance != null && ScoreManager.instance != null)
        {
            InsigniaManager.Instance.ShowInsignia(ScoreManager.instance.GetScore());
        }
    }

    private void OnDestroy()
    {
        if (MinigameTimer.instance != null)
            MinigameTimer.instance.OnTimerFinished -= OnTimeFinished;
    }
}