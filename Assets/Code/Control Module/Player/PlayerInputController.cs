using UnityEngine;
using UnityEngine.InputSystem;

namespace Control_Module.Player
{
    /// <summary>
    /// Handles player-specific movement input and behavior.
    /// </summary>
    /// <remarks>
    /// This class extends the EntityController to process player input
    /// and translate it into movement actions for the player entity.
    /// </remarks>
    public class PlayerInputController : EntityController
    {
        [Header("Player Controller")] 
        [Tooltip("Rotation offset to align the player sprite correctly.")]
        public float rotationOffset;

        [Tooltip("Speed at which the player rotates to face the mouse cursor."),
         Range(0f, 100f)]
        public float rotationSpeed = 20f;

        [Tooltip("Reference to the PlayerInput component.")]
        private PlayerInput _playerInput;

        [Tooltip("Stored movement input from the player.")]
        private Vector2 _moveInput;

        [Tooltip("Stored look input (mouse position) from the player.")]
        private Vector2 _lookInput;

        [Tooltip("Reference to the main camera in the scene.")]
        private Camera _mainCamera;

        /// <summary>
        /// Initializes the PlayerInputController component and sets up input bindings.
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
            if (_moveInput != Vector2.zero)
                EntityMovement.Move(_moveInput);
            else
                EntityMovement.Stop();
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

            // Calculate the direction from the player to the mouse cursor
            Vector2 direction = (mouseWorldPosition - transform.position);

            // Calculate the angle in degrees from the direction vector
            // Mathf.Atan2 returns the angle in radians, so we convert it to degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Create the target rotation by creating a Quaternion from our calculated angle plus the offset
            // We only care about rotation on the Z-axis for 2D
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);

            // Turn the rotation into a smooth transition
            var interpolationRatio = Time.deltaTime * rotationSpeed;
            targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, interpolationRatio);

            // Apply the final rotation to the player
            transform.rotation = targetRotation;
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
}