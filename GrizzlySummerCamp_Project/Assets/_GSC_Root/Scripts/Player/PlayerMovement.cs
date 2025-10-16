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
    [Space]
    //float rotationSpeed = 0.2f;
    [SerializeField] Rigidbody rb;

    //Player Input = El input es una función que recive valores y los traduce (teclado&ratón,mando,etc. a dirección,cantidad,etc) se usa para efectuar acciones
    PlayerInput playerInput;
    InputAction moveAction;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

    }
    void Start()
    {
        moveAction = playerInput.actions.FindAction("Move");
    }

    void Update()
    {
        MovePlayer();
    }

    //Función que se llama para mover al player.
    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(direction.x, 0, direction.y);

        transform.position += moveDir * Speed * Time.deltaTime;

        //Este código sirve para rotar al personaje, si su movimiento es mayor de 0.01.
        if (moveDir.sqrMagnitude > 0.01f) 
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Lerp(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
