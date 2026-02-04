using UnityEngine;

public class TrashContainerIcon : MonoBehaviour
{
    [Header("Distance")]
    [SerializeField] private float showDistance;
    [SerializeField] private float interactDistance;

    [Header("Icons")]
    [SerializeField] private GameObject appleIcon;
    [SerializeField] private GameObject interactIcon;

    private Transform player;
    private Transform self;

    private float showDistanceSqr;
    private float InteractDistanceSqr;
    private void Awake()
    {
        self = transform;
    }

}
