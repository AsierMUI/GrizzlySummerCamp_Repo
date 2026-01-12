using UnityEngine;

public class BalloonExplosionFX : MonoBehaviour
{
    [Header("Explosion Effect")]
    [SerializeField] private GameObject explosionPrefab;

    public void PlayExplosion(Vector3 position)
    {
        if (explosionPrefab == null)
            return;

        GameObject fx = Instantiate(explosionPrefab, position, Quaternion.identity);
        Destroy(fx, 2f);
    }
}