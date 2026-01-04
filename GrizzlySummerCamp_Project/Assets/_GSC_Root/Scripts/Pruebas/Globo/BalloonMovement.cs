using UnityEngine;

public class BalloonMovement : MonoBehaviour
{
    [Header("Velocidad")]
    [SerializeField] private float minSpeed = 1f;
    [SerializeField] private float maxSpeed = 3f;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private float speed;

    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        if (newWaypoints == null || newWaypoints.Length == 0)
        {
            Debug.LogWarning("Globo sin waypoints asignados");
            Destroy(gameObject);
            return;
        }

        waypoints = newWaypoints;
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
            {
                Destroy(gameObject);
            }
        }
    }
}