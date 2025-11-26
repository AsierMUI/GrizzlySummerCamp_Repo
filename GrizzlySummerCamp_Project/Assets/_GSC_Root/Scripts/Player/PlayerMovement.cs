using UnityEngine;
//Importamos el InputSystem.
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Header = Titulo
    [Header("Move Stats")]
    //SerializeField = Nombre de "x" valor, se mantiene privado pero se puede editar en el registro.
    //float speed = Valor con decimales. [Tipo valor] [Nombre valor] [Cantidad valor(Sí se deja vació equivale a 0)]
    [SerializeField] float Speed;
    [SerializeField] float rotationSpeed;
    [SerializeField] Rigidbody rb;


    [SerializeField] float collisionSlowdown = 0.6f; //Entre 0.0-1.0 (0 a 100) 
    [SerializeField] float drag = 5f;

    [SerializeField] Animator animator;

    //Player Input = El input es una función que recive valores y los traduce (teclado&ratón,mando,etc. a dirección,cantidad,etc) se usa para efectuar acciones
    PlayerInput playerInput;
    InputAction moveAction;

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
        bool isWalking = moveDir.sqrMagnitude > 0.01;
        animator.SetBool("isWalking", isWalking);

        // Deseamos una velocidad en esa dirección
        Vector3 desiredVelocity = moveDir * Speed;
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
}