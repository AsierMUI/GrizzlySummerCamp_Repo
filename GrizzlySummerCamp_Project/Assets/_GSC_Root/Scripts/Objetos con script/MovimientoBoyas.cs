using UnityEngine;

public class MovimientoBoyas : MonoBehaviour
{
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private bool volverPorElMismoCamino = false;

    private int currentWaypointIndex = 0;
    private int direction = 1;

    //Movimiento fluido
    private float t = 0f;           //Progreso entre el waypont actual y el siguiente  
    private float distance = 0f;    // distancia entre puntos
    private Vector3 startPos;       // punto inicial
    private Vector3 targetPos;      //punto final

    private void Start()
    {
        if (waypoints.Length == 0) return;

        startPos = transform.position = waypoints[0].position;
        SetNextTarget();
    }
    private void Update()
    {
        if (waypoints.Length <= 1) return;

        //Avanza con la velocidad base del 0 al 1 da igual la distancia
        t += (speed / distance) * Time.deltaTime;

        //suavizado(aceleracion y desaceleracion)
        float easedT = EaseInOut(t);

        //Movimiento suave
        transform.position = Vector3.Lerp(startPos, targetPos, easedT);

        //Cuando llega al final del waypoint pasa al siguiente

        if (t >= 1f)
            SetNextTarget();
    }

    //Suavizado tipo ease in out,  t = 0 (lento), 0.5 (rapido), 1 (vuelve a frenar)

    private float EaseInOut(float x)
    {
        return x * x * (3f - 2f * x);
        //es una funcion de suavizado (smoothstep)
    }

    void SetNextTarget()
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

        startPos = transform.position;
        targetPos = waypoints[currentWaypointIndex].position;

        distance = Vector3.Distance(startPos, targetPos);
        t = 0f;
    }
}