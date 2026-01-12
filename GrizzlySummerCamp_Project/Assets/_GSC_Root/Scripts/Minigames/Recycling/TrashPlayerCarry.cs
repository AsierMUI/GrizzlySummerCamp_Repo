using UnityEngine;

public class TrashPlayerCarry : MonoBehaviour
{
    private TrashItem carriedTrash;
    private PlayerMovement playerMovement;

    [Header("Movement Penalty")]
    [Tooltip("This value changes how slow the player becomes after picking up waste.")]
    [SerializeField] float speedPenalty = -0.2f;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    public bool IsCarryingTrash()
    {
        return carriedTrash != null;
    
    }

    public void PickTrash(TrashItem trash) 
    {
        if (carriedTrash != null) return;

        carriedTrash = trash;   
        trash.gameObject.SetActive(false);

        playerMovement.SetSprintBlocked(true);
        playerMovement.SetSpeedModifier(speedPenalty);

        Debug.Log($"[Trash] Picked {trash.trashType}");
    }

    public void DeliverTrash() 
    {
        carriedTrash = null;

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
