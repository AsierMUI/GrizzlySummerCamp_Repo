using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    public float bounceForce = 5f;
    public float stunDuration = 2f;
    private bool isStunned = false;
    public ParticleSystem stunEffect;

    private Rigidbody rb;
    private BoatMovement movementScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementScript = GetComponent<BoatMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            //calcula la direccion opuesta
            Vector3 bounceDir = (transform.position - collision.transform.position).normalized;

            //elimina la inercia del barco
            if (movementScript != null)
                movementScript.ResetVelocity();

            //fuera de  reborte
            rb.AddForce(bounceDir * bounceForce, ForceMode.Impulse);

            if (stunEffect != null)
                stunEffect.Play();

            if (!isStunned)
                StartCoroutine(StunPlayer());
        }
    }

    private System.Collections.IEnumerator StunPlayer()
    {
        isStunned = true;

        if (movementScript != null)
            movementScript.enabled = false;

        yield return new WaitForSeconds(stunDuration);

        if (movementScript != null)
            movementScript.enabled = true;

        isStunned = false;
        if (stunEffect != null)
            stunEffect.Stop();
    }
}
