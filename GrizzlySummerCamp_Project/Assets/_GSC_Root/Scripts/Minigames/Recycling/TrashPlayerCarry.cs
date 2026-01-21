using UnityEngine;
using System;
public class TrashPlayerCarry : MonoBehaviour
{
    private TrashItem carriedTrash;
    private PlayerMovement playerMovement;
    private AudioManager audioManager;

    [Header("Movement Penalty")]
    [Tooltip("Este valor altera quan despacio vas tras recoger basura")]
    [SerializeField] float speedPenalty = -0.2f;

    [Header("Audio")]
    [SerializeField] private string pickupTrashSfxKey;
    [SerializeField] private string dropTrashSfxKey;

    public static event Action OnTrashDelivered;
    public static event Action<TrashType?> OnCarryChanged;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    public bool IsCarryingTrash()
    {
        return carriedTrash != null;
    }

    public void PickTrash(TrashItem trash) 
    {
        if (carriedTrash != null) return;

        carriedTrash = trash;

        if (!string.IsNullOrEmpty(pickupTrashSfxKey))
            audioManager?.PlaySFX(pickupTrashSfxKey);

        OnCarryChanged?.Invoke(trash.trashType);

        trash.gameObject.SetActive(false);

        playerMovement.SetSprintBlocked(true);
        playerMovement.SetSpeedModifier(speedPenalty);
    }

    public void DeliverTrash() 
    {
        if (carriedTrash != null)
            Destroy(carriedTrash.gameObject);

        carriedTrash = null;

        if (!string.IsNullOrEmpty(dropTrashSfxKey))
            audioManager?.PlaySFX(dropTrashSfxKey);

        OnCarryChanged?.Invoke(null);
        OnTrashDelivered?.Invoke();

        //carriedTrash = null;

        //restaurar movimiento
        playerMovement.SetSprintBlocked(false);
        playerMovement.SetSpeedModifier(0f);
    }

    public TrashType GetCarriedType() 
    {
        return carriedTrash != null ? carriedTrash.trashType : default;
    }

    public int GetCarriedPoints() 
    {
        return carriedTrash.points;
    }
}
