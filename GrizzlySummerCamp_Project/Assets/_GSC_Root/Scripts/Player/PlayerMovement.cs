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

    public static System.Action<bool> OnRunningChanged;
    float lastVelocity = 0f;

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
        float currentSpeed = speed + speedModifier;
        bool sprintPressed = !sprintBlocked && sprintAction.ReadValue<float>() > 0.1f;
        if (sprintPressed) currentSpeed *= sprintMultiplier;

        // Deseamos una velocidad en esa dirección
        Vector3 desiredVelocity = moveDir * currentSpeed;
        Vector3 currentVelocity = rb.GetPointVelocity(transform.position);
        Vector3 velocityChange = desiredVelocity - currentVelocity;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
        }

        //Animacion

        float velocityMagnitude = rb.GetPointVelocity(transform.position).magnitude;
        animator.SetFloat("speed", velocityMagnitude);

        bool isSprinting =
            sprintAction.ReadValue<float>() > 0.1f &&
            !sprintBlocked &&
            velocityMagnitude > 0.1f;

        if (isSprinting != (lastVelocity > speed * 1.1f))
        {
            OnRunningChanged?.Invoke(isSprinting);
        }

        lastVelocity = velocityMagnitude;

        HandleWalkingVFX(velocityMagnitude > 0.1f);
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.collider.CompareTag("Obstacle"))
        {
            Vector3 vel = rb.GetPointVelocity(transform.position) * collisionSlowdown;
            rb.AddForce(vel - rb.GetPointVelocity(transform.position), ForceMode.VelocityChange);
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
        Vector3 vel = rb.GetPointVelocity(transform.position);
        rb.AddForce(-vel, ForceMode.VelocityChange);

        animator.SetFloat("speed", 0f);

        if (walkingVFX != null && walkingVFX.isPlaying)
            walkingVFX.Stop();
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