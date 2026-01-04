using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    [Header("Velocidad")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float speed;

    private BalloonPath myPath;
    private BalloonSpawner spawner;
    private bool isDespawning = false;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    public void Initialize(BalloonPath path, BalloonSpawner balloonSpawner)
    {
        myPath = path;
        spawner = balloonSpawner;

        waypoints = path.waypoints;
        currentWaypointIndex = 0;

        transform.position = waypoints[0].position;
    }

    void Update()
    {
        if (waypoints == null || currentWaypointIndex >= waypoints.Length) return;

        Transform target = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
                Despawn();
        }
    }

    public void Despawn()
    {
        if (isDespawning) return;
        isDespawning = true;

        spawner.OnBalloonDestroyed(myPath);
        Destroy(gameObject);
    }
}