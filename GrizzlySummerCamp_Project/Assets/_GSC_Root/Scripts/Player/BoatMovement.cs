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

    [SerializeField] private bool canMove = false;

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
        if (!canMove)
        {
            ResetVelocity();
            UpdateAnimation(false);
            return;
        }

        MoveBoat();

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

        if (velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        UpdateAnimation(velocity.sqrMagnitude > 0.01f);
    }

    void UpdateAnimation(bool isMoving)
    {
        if (childAnimator != null) childAnimator.SetBool("isMoving", isMoving);
    }

    public void ResetVelocity()
    {
        velocity = Vector3.zero; //para quitar la inercia
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!value)
            ResetVelocity();
    }
}
