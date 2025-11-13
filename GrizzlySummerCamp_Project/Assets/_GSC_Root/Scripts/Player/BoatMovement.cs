using UnityEngine;
using UnityEngine.InputSystem;

public class BoatMovement : MonoBehaviour
{
    [Header("Move Stats")]
    [SerializeField] float Speed = 5f;
    [SerializeField] float rotationSpeed = 10f;
    [SerializeField] float friction = 0.98f;
    [Space]
    [SerializeField] Rigidbody rb;

    private PlayerInput playerInput;
    private InputAction moveAction;

    private Vector3 velocity;

    public bool canMove = false;

    [Header("Child Player Animator")]
    [SerializeField] private Animator childAnimator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
        if(childAnimator == null)
            childAnimator = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        if (canMove == true)
        {
            MoveBoat();
        }
        else { ResetVelocity(); }
    }

    void MoveBoat()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(direction.x, 0, direction.y).normalized;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            velocity += moveDir * Speed * Time.fixedDeltaTime;
        }

        velocity *= friction;

        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

        if (velocity.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        //Animaciones

        if (childAnimator != null)
        {
            bool isMoving = velocity.sqrMagnitude > 0.01f;
            childAnimator.SetBool("isRowing", isMoving);
            childAnimator.SetBool("isIdleBoat", !isMoving);
        }
    }

    public void CanMove() 
    {
        canMove = true;
    }
    public void ResetVelocity()
    {
        velocity = Vector3.zero; //para quitar la inercia
    }
}
