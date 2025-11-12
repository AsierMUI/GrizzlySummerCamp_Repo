using UnityEngine;
using UnityEngine.Rendering;

public class PlayerAim : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public LayerMask aimLayer; //El suelo y las dianas tienen q tener la layer AimTarget //Es nuevo, si no funciona borrar

    private void Update()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimLayer))
        {
            RotateTowards(hit.point);
        }
        else
        {
            //Si no golpea nada, proyecta un plano invisible a nivel del suelo
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 fallbackPoint = ray.GetPoint(enter);
                RotateTowards(fallbackPoint);
            }
        }

        /* SCRIPT ANTERIOR 
          
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
        }*/
    }

    void RotateTowards(Vector3 point)
    {
        Vector3 direction = (point - transform.position);
        direction.y = 0f; //solo rota en el eje y
        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation =Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}
