using UnityEngine;
using UnityEngine.Playables;

public class MovimientoBoyas : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 2f;

    [Header("Opciones de movimiento")]
    [SerializeField] private bool volverPorElMismoCamino = false;

    private int currentWaypointIndex = 0;
    private int direction = 1;

    private void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.05f)
        {
            if (volverPorElMismoCamino)
            {
                if (currentWaypointIndex == waypoints.Length - 1)
                    direction = -1;
                else if (currentWaypointIndex == 0)
                    direction = 1;

                currentWaypointIndex += direction;
            }
            else
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
