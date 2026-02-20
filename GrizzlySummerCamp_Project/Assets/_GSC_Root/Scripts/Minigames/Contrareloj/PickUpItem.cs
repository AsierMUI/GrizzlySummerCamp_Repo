using UnityEngine;
using System.Collections;

public class PickUpItem : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private string pickupSfxKey;

    public string PickupSFXKey => pickupSfxKey;

    [Header("Animator")]
    [SerializeField] private Animator childAnimator;

    [Header("Additional Time")]
    [SerializeField] private float extime;
    public float Extratime => extime;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        PickUpManager.Instance?.CollectPickup(this);

        if(childAnimator != null)
        {
            childAnimator.SetTrigger("Conseguida");

            StartCoroutine(DestroyAfterAnimation());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitUntil(() =>
        childAnimator.GetCurrentAnimatorStateInfo(0).IsName("ConseguirEstrella")
        );

        AnimatorStateInfo stateInfo = childAnimator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitForSeconds(stateInfo.length);

        Destroy(gameObject);
    }
}