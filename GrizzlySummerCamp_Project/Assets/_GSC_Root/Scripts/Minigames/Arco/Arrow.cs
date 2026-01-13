using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasHit = false;

    [Header("Comportamiento flecha")]
    public bool stickOnHit = true;
    public float destroyDelay = 5f;

    [Header("Punto de enganche")]
    [Tooltip("Punto que dice dodne se clava la flecha")]
    public Transform stickPoint;

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

        if (!stickOnHit)
        {
            Destroy(gameObject, destroyDelay); return;
        }

        ContactPoint contact = collision.contacts[0];

        Quaternion targetRotation = Quaternion.LookRotation(-contact.normal, Vector3.up);

        if (stickPoint != null)
        {
            transform.rotation = targetRotation;

            Vector3 offset = transform.position - stickPoint.position;
            transform.position = contact.point + offset;
        }
        else
        {
            transform.position = contact.point;
            transform.rotation = targetRotation;
        }

        rb.isKinematic = true;
        rb.detectCollisions = false;

        Destroy(gameObject, destroyDelay);
    }
}