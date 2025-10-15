using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move Stats")]
    [SerializeField] float Speed;

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

    void MovePlayer() 
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * Speed * Time.deltaTime;
    }
}
