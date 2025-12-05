using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Stats")]
    [SerializeField] float Speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Rigidbody rb;
    [SerializeField] float collisionSlowdown = 0.6f; //Entre 0.0-1.0 (0 a 100) 
    [SerializeField] float drag = 5f;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem walkingVFX;
    [SerializeField] float sprintMultiplier = 3f;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.linearDamping = drag;

        if (animator == null)
            animator = GetComponentInChildren<Animator>();

    }
    void Start()
    {
        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];
    }

    void FixedUpdate()
    {
        MovePlayer();
    }


    void MovePlayer()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(-input.x, 0, -input.y).normalized; //Código está en negativa "-input.x" para que mueva en dirección del mapa

        //animacion andar / idle
        bool isWalking = moveDir.sqrMagnitude > 0.01f;
        animator.SetBool("isWalking", isWalking);
        HandleWalkingVFX(isWalking);

        //Comprueba si se pulsa sprint
        float currentSpeed = Speed;
        if (sprintAction.ReadValue<float>()> 0.1f)
            currentSpeed *= sprintMultiplier;

        // Deseamos una velocidad en esa dirección
        Vector3 desiredVelocity = moveDir * currentSpeed;
        Vector3 velocityChange = desiredVelocity - rb.linearVelocity;   // Aplicamos cambio instantáneo de velocidad (como velocity pero moderno)
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (isWalking)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }
    }
    private void OnCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("Obstacle"))
        {
            rb.linearVelocity *= collisionSlowdown;
        }
    }

    void HandleWalkingVFX(bool isWalking)
    {
        if (walkingVFX == null) return;

        if (isWalking && !walkingVFX.isPlaying)
            walkingVFX.Play();
        else if (!isWalking && walkingVFX.isPlaying)
            walkingVFX.Stop();
    }
}