using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour, IMinigamePlayerMovement
{
    [Header("Move Stats")]
    [SerializeField] float speed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float friction = 0.98f;
    [SerializeField] Rigidbody rb;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector3 velocity;

    [SerializeField] private bool canMove = true;

    [Header("Child Player Animator")]
    [SerializeField] private Animator childAnimator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        if (childAnimator == null)
            childAnimator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
    }

    void FixedUpdate()
    {
        if (!canMove || MinigameManager.Instance == null || !MinigameManager.Instance.IsRunning)
        {
            ResetVelocity();
            return;
        }

        if (childAnimator.GetBool("inContrareloj"))
        {
            MoveBoat();
        }
        else
        {
            ResetVelocity();
        }
    }

    private void Update()
    {
        float normalizedSpeed = Mathf.Clamp(velocity.magnitude / speed, 0f, 1f);
        UpdateAnimation(normalizedSpeed);
    }

    void MoveBoat()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(direction.x, 0, direction.y).normalized;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            velocity += moveDir * speed * Time.fixedDeltaTime;
        }

        velocity *= friction;

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        if (velocity.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        float normalizedSpeed = Mathf.Clamp(velocity.magnitude / speed, 0f, 1f);
        Debug.Log($"[BoatMovement] MoveBoat - velocity magnitude: {velocity.magnitude}");

        UpdateAnimation(velocity.sqrMagnitude);
    }

    void UpdateAnimation(float currentSpeed)
    {
        if (childAnimator == null) return;

        bool inContrareloj = childAnimator.GetBool("inContrareloj");
        bool inPesca = childAnimator.GetBool("inPesca");

        if (inContrareloj || inPesca)
        {
           childAnimator.SetFloat("speed", currentSpeed);
           Debug.Log($"[BoatMovement] UpdateAnimation - speed set to: {currentSpeed}");
        }
        else
        {
           childAnimator.SetFloat("speed", 0f);
           Debug.Log("[BoatMovement] UpdateAnimation - not inContrareloj, speed set to 0");
        }
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero; //para quitar la inercia
        Debug.Log("[BoatMovement] ResetVelocity called");
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        Debug.Log("[BoatMovement] SetCanMove: " + value);
        if (!value)
            ResetVelocity();
    }
}