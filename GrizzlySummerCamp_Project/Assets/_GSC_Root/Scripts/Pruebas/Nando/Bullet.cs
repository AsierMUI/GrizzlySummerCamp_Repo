using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;

    void OnCollisionEnter(Collision collision)
    {
        // Aquí podrías aplicar daño a enemigos u objetivos
        // collision.gameObject.GetComponent<EnemyHealth>()?.TakeDamage(damage);

        Destroy(gameObject);
    }
}