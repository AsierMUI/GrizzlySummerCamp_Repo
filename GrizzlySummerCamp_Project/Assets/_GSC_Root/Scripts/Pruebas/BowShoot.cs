using UnityEngine;

public class BowShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint; // punto desde donde se lanza la flecha
    public float shootForce = 20f;
    public float upwardForce = 3f; //control de parabola
    public LayerMask aimLayer; //Es nuevo, si no funciona borrar

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))//clic izquierdo
        {
            Shoot();
        } 
    }

    void Shoot()
    {
        //Calcular direccion hacia donde apunta el mouse
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 targetPoint = ray.GetPoint(50); //fallback
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, aimLayer))
            targetPoint = hit.point;

        Vector3 direction = (targetPoint - shootPoint.position).normalized;

        //Instanciar flecha
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, Quaternion.LookRotation(direction));
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        //Aplicar fuerza fisica (parabola)
        Vector3 force = direction * shootForce + Vector3.up * upwardForce;
        rb.AddForce(force, ForceMode.Impulse);



        /*SCRIPT ANTERIOR

        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        //Aplica fuerxa hacia adelante y un pequeño impulso hacia arriba
        Vector3 forceDirection = shootPoint.forward * shootForce + shootPoint.up * upwardForce;
        rb.AddForce(forceDirection, ForceMode.Impulse);
        */
    }
}
