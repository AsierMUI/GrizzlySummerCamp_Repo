using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public float aimPlaneDistance = 10f;

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void Update()
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

    void RotateTowards(Vector3 point)
    {
        Vector3 direction = point - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}