using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;
    bool hasHit = false;

    public bool stickOnHit = true; //true = se clava, false = rebota y desaparece //Nuevo, si no funciona quitar
    public float destroyDelay = 5f; //Nuevo, si no funciona quitar
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!hasHit && rb.linearVelocity.magnitude > 0.1f)
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
            //Clavar flecha
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;

            //posicionar en el punto deimpacto
            transform.position = collision.contacts[0].point;

            //apuntar en la direccion contraria al normal del impacto
            transform.forward = -collision.contacts[0].normal;
        }
        else
        {
            rb.linearDamping = 2f;
            Destroy(gameObject, destroyDelay);
        }
    }
}