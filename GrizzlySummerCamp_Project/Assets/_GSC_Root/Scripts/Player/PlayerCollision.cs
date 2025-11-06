using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour
{
    [Header("Stun Stats")]
    [SerializeField] public float bounceForce = 5f;
    [SerializeField] public float stunDuration = 2f;
    private bool isStunned = false;

    [Header("Effect Prefab")]
    [SerializeField] public ParticleSystem stunEffect;

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

            //Nuevo:
            ContactPoint contact = collision.contacts[0];
            Vector3 hitPoint = contact.point;
            Vector3 hitNormal = contact.normal;

            if (stunEffect != null) 
            {
                ParticleSystem effect = Instantiate(stunEffect, hitPoint, Quaternion.LookRotation(hitNormal));
                effect.Play();
                Destroy(effect.gameObject, effect.main.duration); //No es la forma más eficiente, tal vez probar con apagar uno y moverlo de algun modo sería mejor.
            }

            /*  Código Viejo
            if (stunEffect != null)
                stunEffect.Play();
            */
            if (!isStunned)
                StartCoroutine(StunPlayer());
        }
    }

    /* Stun Player nuevo*/
    private IEnumerator StunPlayer() 
    {
        isStunned = true;

        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        yield return new WaitForSeconds(stunDuration);

        if (movementScript != null)
        {
            movementScript.enabled = true;
        }
        isStunned = false;
    }

    /* Stun Player Antiguo
     * private System.Collections.IEnumerator StunPlayer()
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
    }*/
}
