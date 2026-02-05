using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    [Header("Player Settings")]
    public float rotationSpeed = 5f;
    public float aimPlaneDistance = 10f;

    [Header("Bow Settings")]
    public BowShoot bowShoot;
    private Animator playerAnimator;

    [Header("Visual Arrow")]
    public GameObject arrowToHide;

    private bool wasReloading;

    private Camera mainCam;
    private Quaternion lockedRotation; //la rotacion en la que se queda margarita al disparar
    private Rigidbody rb;

    private void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        lockedRotation = transform.rotation;

        playerAnimator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (MinigameManager.Instance == null || !MinigameManager.Instance.IsRunning)
            return;

        if (bowShoot != null)
        {
            if (!bowShoot.IsReloading && wasReloading)
            {
                OnReloadFinished();
            }

            wasReloading = bowShoot .IsReloading;
        }

        if (bowShoot != null && bowShoot.IsReloading)
        {
            LockRotation();
            return;
        }

        HandleAiming();
        HandleInput();

    }

    void HandleAiming()
    {
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Vector3 planePoint = transform.position + transform.forward * aimPlaneDistance;
        Plane aimPlane = new Plane(-mainCam.transform.forward, planePoint);

        if (aimPlane.Raycast(ray, out float enter))
        {
            Vector3 aimPoint = ray.GetPoint(enter);
            RotateTowards(aimPoint);
        }
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && playerAnimator != null && playerAnimator.GetBool("inArco"))
        {
            ShootBow();
        }
    }

    void LockRotation()
    {
        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
            rb.rotation = lockedRotation;
        }
        else
        {
            transform.rotation = lockedRotation;
        }
    }

    void RotateTowards(Vector3 point)
    {
        Vector3 direction = point - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        lockedRotation = transform.rotation;
    }

    public void ShootBow()
    {
        if (bowShoot != null)
            bowShoot.Shoot();

        if (arrowToHide != null)
            arrowToHide.SetActive(false);

        if (playerAnimator != null)
        {
            playerAnimator.CrossFade("MG_Arrow_Reload&Shoot", 0.1f, 0, 0f);
        }
    }

    void OnReloadFinished()
    {
        if (arrowToHide != null)
            arrowToHide.SetActive(true);
    }
}