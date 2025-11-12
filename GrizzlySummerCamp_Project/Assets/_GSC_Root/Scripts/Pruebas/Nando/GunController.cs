using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class GunController : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Camera mainCamera;
    public LineRenderer lineRenderer;

    [Header("Settings")]
    [Tooltip("Velocidad de la bala al ser disparada")]
    public float bulletSpeed = 60f;
    [Tooltip("Duraci�n de la recarga despu�s de cada disparo")]
    public float reloadDuration = 1.5f;
    [Tooltip("Distancia m�xima del raycast de apuntado")]
    public float maxShootDistance = 100f;
    [Tooltip("Porcentaje visible de la l�nea predictiva (0 = nada, 1 = completa)")]
    [Range(0f, 1f)] public float lineVisibleLength = 0.3f;

    private PlayerInput playerInput;
    private InputAction shootAction;
    public GameObject aimPointerPrefab;
    private GameObject aimPointerInstance;
    private bool canShoot = true;
    private bool showLine = true;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        shootAction = playerInput.actions["Shoot"];
        if (aimPointerPrefab != null)
            aimPointerInstance = Instantiate(aimPointerPrefab);
    }

    void OnEnable()
    {
        shootAction.performed += Shoot;
    }

    void OnDisable()
    {
        shootAction.performed -= Shoot;
    }

    void Update()
    {
        // Mostrar la linea solo cuando se puede disparar
        if (showLine && canShoot)
        {
            UpdateAimLine();
        }
        else
        {
            if (lineRenderer.enabled)
                lineRenderer.enabled = false;
        }
    }

    void Shoot(InputAction.CallbackContext context)
    {
        if (!canShoot) return;
        StartCoroutine(ShootRoutine());
    }

    private IEnumerator ShootRoutine()
    {
        canShoot = false;
        showLine = false;
        if (aimPointerInstance != null)
            aimPointerInstance.SetActive(false);
        // ocultar linea durante disparo y recarga
        lineRenderer.enabled = false;

        // Crear y lanzar la bala
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = firePoint.forward * bulletSpeed;
        Destroy(bullet, 3f); //Destruye la bala pasado unos segundos

        // Esperar el tiempo de recarga antes de volver a mostrar la linea
        yield return new WaitForSeconds(reloadDuration);


        canShoot = true;
        if (aimPointerInstance != null)
            aimPointerInstance.SetActive(true);
        showLine = true;
    }

    private void UpdateAimLine()
    {
        // Crear un rayo desde la c�mara hasta la posici�n del rat�n
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, maxShootDistance))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * maxShootDistance;
        }
        if (aimPointerInstance != null)
        {
            aimPointerInstance.SetActive(true);
            aimPointerInstance.transform.position = targetPoint;
        }
        // Orientar el firePoint hacia el punto de impacto
        Vector3 aimDir = (targetPoint - firePoint.position).normalized;
        firePoint.forward = aimDir;

        // Activar el LineRenderer si est� desactivado
        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;

        // Dibujar una parte de la trayectoria (parcial)
        Vector3 endPoint = Vector3.Lerp(firePoint.position, targetPoint, lineVisibleLength);
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, endPoint);
    }
}