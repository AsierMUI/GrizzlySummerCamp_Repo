using UnityEngine;
//Importamos el InputSystem.
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //Header = Titulo
    [Header("Move Stats")]
    //SerializeField = Nombre de "x" valor, se mantiene privado pero se puede editar en el registro.
    //float speed = Valor con decimales. [Tipo valor] [Nombre valor] [Cantidad valor(S� se deja vaci� equivale a 0)]
    [SerializeField] float Speed;

    //Player Input = El input es una funci�n que recive valores y los traduce (teclado&rat�n,mando,etc. a direcci�n,cantidad,etc) se usa para efectuar acciones
    PlayerInput  playerInput;
    InputAction moveAction;
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        
    }

    void Update()
    {
        MovePlayer();
    }

    //Funci�n que se llama para mover al player.
    void MovePlayer() 
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        //
        transform.position += new Vector3(direction.x, 0, direction.y) * Speed * Time.deltaTime;
    }
}
