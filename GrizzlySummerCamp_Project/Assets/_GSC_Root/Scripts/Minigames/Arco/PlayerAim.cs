using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float aimPlaneDistance = 10f;
    public BowShoot bowShoot;

    private Camera mainCam;
    private Quaternion lockedRotation; //la rotacion en la que se queda margarita al disparar
    private Rigidbody rb;

    private void Start()
    {
        mainCam = Camera.main;
        rb = GetComponent<Rigidbody>();
        lockedRotation = transform.rotation;
    }

    private void Update()
    {
        if (MinigameManager.Instance == null || !MinigameManager.Instance.IsRunning)
            return;

        if (bowShoot != null && bowShoot.IsReloading)
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
            return;
        }

        /*
        if (Time.timeScale == 0f) return;
        if (bowShoot != null && bowShoot.IsReloading)
        {
            if(rb != null)
            {
                rb.angularVelocity = Vector3.zero;
                rb.rotation = lockedRotation;
            }
            else
            {
                transform.rotation = lockedRotation;
            }     
            return;
        }
        */

        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        Vector3 planePoint = transform.position + transform.forward * aimPlaneDistance;
        Plane aimPlane = new Plane(-mainCam.transform.forward, planePoint);

        if (aimPlane.Raycast(ray, out float enter))
        {
            Vector3 aimPoint = ray.GetPoint(enter);
            RotateTowards(aimPoint);
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
}