using UnityEngine;
using System.Collections;

public class PickUpItem : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private string pickupSfxKey;

    private AudioManager audioManager;
    private Animator childAnimator;

    private void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();

        childAnimator = GetComponentInChildren<Animator>(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!string.IsNullOrEmpty(pickupSfxKey))
            audioManager?.PlaySFX(pickupSfxKey);


        PickUpManager.Instance?.CollectPickup(transform);
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