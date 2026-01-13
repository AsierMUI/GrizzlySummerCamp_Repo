using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false;

    [Header("Comportamiento flecha")]
    public bool stickOnHit = true;
    public float destroyDelay = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!hasHit && rb != null && rb.linearVelocity.magnitude > 0.1f)
            transform.forward = rb.linearVelocity.normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        BalloonMovement balloon = collision.gameObject.GetComponent<BalloonMovement>();

        if (balloon != null)
        {
            balloon.Despawn(true);
            Destroy(gameObject);
            return;
        }

        if (stickOnHit)
        {
            ContactPoint contact = collision.contacts[0];

            //Posiciona la flecha
            transform.position = contact.point;
            transform.forward = -contact.normal;

            //Elimina rigidbody
            Destroy(rb);

            Destroy(gameObject, destroyDelay);
        }
        else
        {
            Destroy(gameObject, destroyDelay);
        }
    }
}