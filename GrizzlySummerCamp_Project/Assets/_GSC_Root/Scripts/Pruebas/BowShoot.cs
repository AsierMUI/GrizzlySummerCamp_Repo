using UnityEngine;

public class BowShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public Transform shootPoint; // punto desde donde se lanza la flecha
    public float shootForce = 20f;
    public float upwardForce = 3f; //control de parabola

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))//clic izquierdo
        {
            Shoot();
        } 
    }

    void Shoot()
    {
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        //Aplica fuerxa hacia adelante y un pequeño impulso hacia arriba
        Vector3 forceDirection = shootPoint.forward * shootForce + shootPoint.up * upwardForce;
        rb.AddForce(forceDirection, ForceMode.Impulse);
    }
}
