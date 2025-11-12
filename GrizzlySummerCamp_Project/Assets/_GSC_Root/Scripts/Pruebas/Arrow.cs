using UnityEngine;

public class Arrow : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            transform.forward = rb.linearVelocity.normalized;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //clavar la flecha
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero;
        transform.parent = collision.transform;
    }
}
