using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    public float rotationSpeed = 5f;

    private void Update()
    {
        //obtener posicion del mouse en pantalla
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Vector3.zero); //plano del suelo
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);
            Vector3 direction = (hitPoint - transform.position).normalized;
            direction.y = 0; //no puede rotar en el eje vertical
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
