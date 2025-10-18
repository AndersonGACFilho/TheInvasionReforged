using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player-specific movement input and behavior.
/// </summary>
/// <remarks>
/// This class extends the EntityMovementHandler to process player input
/// and translate it into movement actions for the player entity.
/// </remarks>
public class PlayerMovementHandler : EntityMovementHandler
{
    [Header("Player Movement Handler" )]
    [Tooltip("Reference to the PlayerInput component.")]
    private PlayerInput _playerInput;
    [Tooltip("Stored movement input from the player.")]
    private Vector2 _moveInput; 
    [Tooltip("Stored look input (mouse position) from the player.")]
    private Vector2 _lookInput;
    [Tooltip("Reference to the main camera in the scene.")]
    private Camera _mainCamera;
    
    /// <summary>
    /// Initializes the PlayerMovementHandler component and sets up input bindings.
    /// </summary>
    public override void Awake()
    {
        base.Awake();

        _mainCamera = Camera.main;

        _playerInput = GetComponent<PlayerInput>();
        
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMove;
        
        _playerInput.actions["Look"].performed += OnLook;
    }
    
    /// <summary>
    /// Handles movement input and moves the player entity accordingly.
    /// </summary>
    void FixedUpdate()
    {
        if(_moveInput != Vector2.zero)
            entityMovement.Move(_moveInput);
    }

    /// <summary>
    /// Handles rotation input and rotates the player entity to face the mouse cursor.
    /// </summary>
    void Update()
    {
        HandleRotation();
    }

    /// <summary>
    /// Handles the rotation of the player to face the mouse cursor.
    /// </summary>
    private void HandleRotation()
    {
        // Convert the stored mouse screen position to a world position
        Vector3 mouseWorldPosition = _mainCamera.ScreenToWorldPoint(_lookInput);
        
        Vector2 direction = (mouseWorldPosition - transform.position);
        transform.right = direction;
    }

    // --- Input Callbacks ---
    /// <summary>
    /// Callback for movement input action.
    /// </summary>
    /// <param name="context"> The context of the input action. </param>
    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    /// <summary>
    /// Callback for look input action.
    /// </summary>
    /// <param name="context"> The context of the input action. </param>
    public void OnLook(InputAction.CallbackContext context)
    {
        _lookInput = context.ReadValue<Vector2>();
    }

}
