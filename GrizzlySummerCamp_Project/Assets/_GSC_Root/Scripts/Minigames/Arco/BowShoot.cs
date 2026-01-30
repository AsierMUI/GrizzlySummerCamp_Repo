using UnityEngine;

public class BowShoot : MonoBehaviour
{
    [Header("Arrow Settings")]
    public GameObject arrowPrefab;
    public Transform shootPoint;
    public float shootForce = 20f;
    public float upwardForce = 3f;
    public LayerMask aimLayer;

    [Header("Cooldown")]
    public float shootCooldown = 1f;
    private bool canShoot = true;
    //Meter texto "Reloading!"

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
    public bool IsReloading => !canShoot;

    [Header("Audio")]
    [SerializeField] private AudioManager audioManager;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private void Start()
    {
        mainCam = Camera.main;

        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        SetupLineRenderer();

        if (audioManager == null)
            audioManager = FindFirstObjectByType<AudioManager>();

        if (animator == null)
            animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (!CanProcessInput()) 
        {
            DisableTrajectory();
            return;
        }

        HandleTrajectory();
        HandleShootInput();
    }

    bool CanProcessInput() //Bool de seguridad.
    {
        if (MinigameManager.Instance == null) return false;
        if (!MinigameManager.Instance.IsRunning) return false;
        if (UIState.IsUIOpen) return false;

        return true;
    }
    void HandleTrajectory() 
    {
        if (!showTrajectory) return;
        {
            UpdateTrajectory();
        }
    }
    void HandleShootInput() 
    {
        if (!canShoot) return;

        if (Input.GetMouseButtonDown(0))
            Shoot();
    }

    void Shoot()
    {
        if (!CanProcessInput()) return;
        
        //Deshabilitar el disparo+trayectoria.
        canShoot = false;
        showTrajectory = false;
        DisableTrajectory();

        animator.SetTrigger("shootBow");

        //Llamar al ruido de disparo
        audioManager?.PlaySFX("Bow_Shoot");

        //Deshabilitar el apuntar.
        if(TryGetComponent<PlayerAim>(out var aim))
            aim.enabled = false;

        Vector3 targetPoint = GetAimPoint();
        Vector3 direction = (targetPoint - shootPoint.position).normalized;

        GameObject arrow = Instantiate(
            arrowPrefab,
            shootPoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        Vector3 force = direction * shootForce + Vector3.up * upwardForce;
        rb.AddForce(force, ForceMode.Impulse);

        Invoke(nameof(ResetShot), shootCooldown);

    }

    void ResetShot()
    {
        canShoot = true;
        showTrajectory = true;

        audioManager?.PlaySFX("Bow_Reload");

        if (TryGetComponent<PlayerAim>(out var aim))
            aim.enabled = true;

    }

    void SetupLineRenderer() 
    {
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.widthMultiplier = lineWidth;
        lineRenderer.enabled = false;
    }

    void DisableTrajectory() 
    {
        if(lineRenderer.enabled)
            lineRenderer.enabled = false;
    }

    private void UpdateTrajectory()
    {
        Vector3 targetPoint = GetAimPoint();
        Vector3 direction = (targetPoint - shootPoint.position).normalized;
        Vector3 velocity = direction * shootForce + Vector3.up * upwardForce;

        int visiblePoints = Mathf.CeilToInt(lineSegmentCount * lineVisibleLength);
        lineRenderer.positionCount = visiblePoints;
        
        Vector3 previousPoint = shootPoint.position;

        for (int i = 0; i < visiblePoints; i++)
        {
            float t = i * timeStep;
            Vector3 point = shootPoint.position + velocity * t + 0.5f * Physics.gravity * t * t;

            if (Physics.Raycast(previousPoint, point - previousPoint,
                out RaycastHit hit,
                (point - previousPoint).magnitude,
                aimLayer)) 
            {
                lineRenderer.positionCount = i + 1;
                lineRenderer.SetPosition(i, hit.point);
                break;
            }

            lineRenderer.SetPosition(i, point);
            previousPoint = point;
        }

        lineRenderer.enabled = true;
    }

    Vector3 GetAimPoint() 
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayer))
        {
            return hit.point;
        }

        return ray.GetPoint(50f);
    }
}