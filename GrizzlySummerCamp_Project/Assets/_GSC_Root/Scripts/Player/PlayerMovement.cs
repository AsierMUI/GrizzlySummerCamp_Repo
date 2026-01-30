using UnityEngine;
using UnityEngine.InputSystem;

//Editado para que en el hub funcione normalmente y sea compatible con el minigamemanager

public class PlayerMovement : MonoBehaviour, IMinigamePlayerMovement
{

    [Header("Move Stats")]
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float collisionSlowdown = 0.6f; //Entre 0.0-1.0 (0 a 100) 
    [SerializeField] float drag = 5f;
    [SerializeField] float sprintMultiplier = 3f;

    [Header("Refeences")]
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;
    [SerializeField] ParticleSystem walkingVFX;

    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;
    
    private bool canMove = true;
    private bool sprintBlocked = false;
    private float speedModifier = 0f;


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
        if (!canMove)
        {
            StopMovement();
            return;
        }

        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(-input.x, 0, -input.y).normalized; //Código está en negativa "-input.x" para que mueva en dirección del mapa

        //animacion andar / idle
        bool isWalking = moveDir.sqrMagnitude > 0.01f;
        bool sprintPressed = !sprintBlocked && sprintAction.ReadValue<float>() > 0.1f;
        bool isRunning = isWalking && sprintPressed;

        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning);
        HandleWalkingVFX(isWalking);

        //Comprueba si se pulsa sprint
        float currentSpeed = speed + speedModifier;

        if (!sprintPressed && isWalking)
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

    void StopMovement()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);

        if (walkingVFX != null && walkingVFX.isPlaying) {walkingVFX.Stop();}
    }

    //Para otros scripts
    public void SetCanMove(bool value)
    {
        canMove = value;
        if (!value)
            StopMovement();
    }

    public bool GetCanMove()
    {
        return canMove;
    }

    public void SetSprintBlocked(bool value) 
    {
        sprintBlocked = value;
    }

    public void SetSpeedModifier(float modifier) 
    {
        speedModifier = modifier;
    }
}