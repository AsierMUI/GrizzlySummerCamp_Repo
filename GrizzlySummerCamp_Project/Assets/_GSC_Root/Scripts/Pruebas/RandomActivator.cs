/*using NUnit.Framework;
using UnityEngine;

public class RandomActivator : MonoBehaviour
{
    //Este script sirve tanto para pesca como para basura
    [Header("Zona(Empty Padre)")]
    public Transform zoneParent;

    [Header("Cantidad a activar")]
    public int minToActivate = 3;
    public int maxToActivate = 3;

    private List<GameObject> children = new List<GameObject>();

    void Start()
    {
        CacheChildren();
        ActivateRandomChildren();
    }

    private void CacheChildren()
    {
        children.Clear();

        foreach(Transform child in zoneParent)
        {
            children.Add(child.gameObject);
        }
    }

    public void ActivateRandomChildren()
    {
        if (children.Count == 0) return;

        foreach (GameObject obj in children)
        {
            obj.SetActive(false);
        }

        int amount = Random.Range(minToActivate, maxToActivate + 1);
        amount = Mathf.Clamp(amount, 0, children.Count);

    }
}
*/