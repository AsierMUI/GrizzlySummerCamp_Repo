using UnityEngine;

public class Rapids : MonoBehaviour
{
    [Header("Boost Settings")]
    [SerializeField] float boostForce = 10f;

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(transform.forward * boostForce * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}
