using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public float bounceForce = 5f;
    public float stunDuration = 2f;
    private bool isStunned = false;

    private Rigidbody rb;
    private PlayerMovement movementScript;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementScript = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Vector3 bounceDir = (transform.position - collision.transform.position).normalized; //calcula la direccion opuesta

            rb.AddForce(bounceDir * bounceForce, ForceMode.Impulse); //aplica la fuerza del rebote

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
    }

}
