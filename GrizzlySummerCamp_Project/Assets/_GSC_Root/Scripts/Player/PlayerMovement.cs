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


    [SerializeField] float collisionSlowdown = 0.6f; //Entre 0.0-1.0 (0 a 100) //NUEVO

    [SerializeField] float drag = 5f; //NUEVO


    //Player Input = El input es una función que recive valores y los traduce (teclado&ratón,mando,etc. a dirección,cantidad,etc) se usa para efectuar acciones
    PlayerInput playerInput;
    InputAction moveAction;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Evita que vuelque/Congela la rotación

        rb.interpolation = RigidbodyInterpolation.Interpolate; //NUEVO
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; //NUEVO
        rb.linearDamping = drag; //NUEVO

    }
    void Start()
    {
        //moveAction = playerInput.actions.FindAction("Move");
        moveAction = playerInput.actions["Move"]; //NUEVO
    }

    void FixedUpdate()
    {
        MovePlayer();
    }


    void MovePlayer()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y).normalized;

        // Deseamos una velocidad en esa dirección
        Vector3 desiredVelocity = moveDir * Speed;

        // Aplicamos cambio instantáneo de velocidad (como velocity pero moderno)
        Vector3 velocityChange = desiredVelocity - rb.linearVelocity;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        // Rotación suave hacia la dirección de movimiento
        if (moveDir.sqrMagnitude > 0.01f)
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

    /*
    //Función que se llama para mover al player Antiguo.
    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(direction.x, 0, direction.y);

        //transform.position += moveDir * Speed * Time.deltaTime;
        Vector3 targetPosition = rb.position + moveDir * Speed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        //Este código sirve para rotar al personaje, si su movimiento es mayor de 0.01.
        if (moveDir.sqrMagnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, targetRotation, rotationSpeed * Time.deltaTime));
            //transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    */

}