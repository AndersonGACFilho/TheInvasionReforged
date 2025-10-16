using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : EntityMovement
{
    [Header("Player Settings")]
    protected PlayerInput playerInput;
    protected Vector2 moveInput;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        
        playerInput.actions["Move"].performed += OnMove;
        playerInput.actions["Move"].canceled += OnMove;
    }
    
    void FixedUpdate()
    {
        if(moveInput != Vector2.zero)
            Move(moveInput);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}