using UnityEngine;

public class BowShoot : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public Transform shootPoint; // punto desde donde se lanza la flecha
    public float shootForce = 20f;
    public float upwardForce = 3f;
    public LayerMask aimLayer;

    [Header("Trajectory Preview")]
    public LineRenderer lineRenderer;
    public int lineSegmentCount = 30; // más puntos = curva más suave
    public float timeStep = 0.1f;     // intervalo entre puntos
    public Color lineColor = Color.yellow;

    [Tooltip("Porcentaje visible de la línea de trayectoria (0 = nada, 1 = completa)")]
    [Range(0f, 1f)]
    public float lineVisibleLength = 0.4f; // la trayectoria q se ve

    [Tooltip("Grosor de la línea")]
    public float lineWidth = 0.05f;

    private Camera mainCam;
    private bool showTrajectory = true;

    private void Start()
    {
        mainCam = Camera.main;

        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.enabled = false;
    }

    private void Update()
    {
        if (showTrajectory)
            UpdateTrajectory();

        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {
        // Calcular dirección del disparo
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(50); // fallback
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayer))
            targetPoint = hit.point;

        Vector3 direction = (targetPoint - shootPoint.position).normalized;

        // Instanciar flecha
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        // Aplicar fuerza (parabólica)
        Vector3 force = direction * shootForce + Vector3.up * upwardForce;
        rb.AddForce(force, ForceMode.Impulse);

        // Ocultar línea tras disparar
        lineRenderer.enabled = false;
        showTrajectory = false;

        // Opcional: mostrar línea otra vez después de recarga
        Invoke(nameof(ShowLineAgain), 1.2f);
    }

    void ShowLineAgain()
    {
        showTrajectory = true;
        lineRenderer.enabled = true;
    }

    private void UpdateTrajectory()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(50); // fallback
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayer))
            targetPoint = hit.point;

        Vector3 direction = (targetPoint - shootPoint.position).normalized;
        Vector3 velocity = direction * shootForce + Vector3.up * upwardForce;

        int visiblePoints = Mathf.CeilToInt(lineSegmentCount * lineVisibleLength);
        lineRenderer.positionCount = visiblePoints;

        Vector3 previousPoint = shootPoint.position;

        for (int i = 0; i < visiblePoints; i++)
        {
            float t = i * timeStep;
            Vector3 point = shootPoint.position + velocity * t + 0.5f * Physics.gravity * t * t;

            // Raycast para colisiones
            if (Physics.Raycast(previousPoint, point - previousPoint, out RaycastHit segmentHit,
                                (point - previousPoint).magnitude, aimLayer))
            {
                lineRenderer.positionCount = i + 1;
                lineRenderer.SetPosition(i, segmentHit.point);
                break;
            }

            lineRenderer.SetPosition(i, point);
            previousPoint = point;
        }

        lineRenderer.enabled = true;
    }
}
