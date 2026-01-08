using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    [Header("Velocidad")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;

    [Header("Score")]
    [SerializeField] private int baseScore = 10;
    [SerializeField] private float speedScoreMultiplier = 1.5f;

    [Header("Floating Score")]
    [SerializeField] private GameObject floatingScorePrefab;
    [SerializeField] private Vector3 floatingOffset = Vector3.up * 0.5f;

    [Header("Balloon Type")]
    [SerializeField] private int scoreSign = 1; // Positivo = bueno, Negativo = malo

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float speed;

    private BalloonSpawner spawner;
    private bool isDespawning = false;

    public BalloonRouteInstance myRouteInstance;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    public void Initialize(BalloonPath path, Transform[] chosenWaypoints, BalloonSpawner balloonSpawner)
    {
        spawner = balloonSpawner;
        waypoints = chosenWaypoints;
        currentWaypointIndex = 0;

        transform.position = waypoints[0].position;
    }

    void Update()
    {
        if (waypoints == null || currentWaypointIndex >= waypoints.Length) return;

        Transform target = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
                Despawn(false);
        }
    }

    public void Despawn(bool giveScore = true)
    {
        if (isDespawning) return;
        isDespawning = true;

        if (giveScore && ScoreManager.Instance != null)
        {
            float speedNormalized = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
            float finalMultiplier = 1f + (speedNormalized * speedScoreMultiplier);

            int rawScore = Mathf.RoundToInt(baseScore * finalMultiplier);
            rawScore *= scoreSign;

            int finalScore = Mathf.RoundToInt(rawScore / 10f) * 10;

            ScoreManager.Instance.AddPoints(finalScore);

            if (floatingScorePrefab != null)
            {
                GameObject floating = Instantiate(
                    floatingScorePrefab,
                    transform.position + floatingOffset,
                    Quaternion.identity
                );

                floating.GetComponent<FloatingScoreText>().SetText(finalScore);
            }
        }

        spawner.OnBalloonDestroyed(myRouteInstance);
        Destroy(gameObject);
    }

    public void SetRouteInstance(BalloonRouteInstance routeInstance)
    {
        myRouteInstance = routeInstance;
    }
}