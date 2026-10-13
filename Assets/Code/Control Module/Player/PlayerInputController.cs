using UnityEngine;
using UnityEngine.InputSystem;

namespace Control_Module.Player
{
    /// <summary>
    /// Controls player movement and aiming using the Unity Input System.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The player rotates toward the mouse cursor and moves relative
    /// to its current facing direction.
    /// </para>
    /// <para>
    /// Movement controls:
    /// W/S move forward and backward, while A/D strafe left and right.
    /// The mouse controls aiming independently of movement input.
    /// </para>
    /// <para>
    /// Input is processed during Update, while movement and rotation
    /// are applied during FixedUpdate through Rigidbody2D.
    /// </para>
    /// </remarks>
    [RequireComponent(typeof(PlayerInput), typeof(Rigidbody2D))]
    public class PlayerInputController : EntityController
    {
        [Header("Player Controller")]

        [Tooltip(
            "Rotation offset in degrees used to align the sprite's " +
            "local forward axis with the mouse direction."
        )]
        public float rotationOffset;

        [Tooltip(
            "Controls how quickly the player rotates toward the cursor."
        )]
        [Range(0f, 100f)]
        public float rotationSpeed = 20f;

        /// <summary>
        /// Current cursor position in screen coordinates.
        /// </summary>
        private Vector2 _lookInput;

        /// <summary>
        /// Current movement input in local ship coordinates.
        /// X controls strafing and Y controls forward movement.
        /// </summary>
        private Vector2 _moveInput;

        /// <summary>
        /// Main camera used to convert cursor coordinates
        /// from screen space to world space.
        /// </summary>
        private Camera _mainCamera;

        /// <summary>
        /// Input component responsible for the player's actions.
        /// </summary>
        private PlayerInput _playerInput;

        /// <summary>
        /// Rigidbody2D responsible for physical rotation
        /// and movement of the player.
        /// </summary>
        private Rigidbody2D _rb;

        /// <summary>
        /// Target rotation in degrees, including the sprite
        /// alignment offset.
        /// </summary>
        private float _targetAngle;

        /// <summary>
        /// Initializes required components and registers
        /// the player's input callbacks.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            _mainCamera = Camera.main;
            _rb = GetComponent<Rigidbody2D>();
            _playerInput = GetComponent<PlayerInput>();

            // Prevent unexpected rotation before the first aim update.
            _targetAngle = _rb.rotation;

            _playerInput.actions["Move"].performed += OnMove;
            _playerInput.actions["Move"].canceled += OnMove;

            _playerInput.actions["Look"].performed += OnLook;
            _playerInput.actions["Look"].canceled += OnLook;
        }

        /// <summary>
        /// Updates the desired player orientation based on
        /// the current mouse position.
        /// </summary>
        private void Update()
        {
            HandleRotation();
        }

        /// <summary>
        /// Applies player rotation and movement at the
        /// fixed physics timestep.
        /// </summary>
        /// <remarks>
        /// Both movement and rotation are applied through
        /// Rigidbody2D to avoid direct Transform modifications
        /// conflicting with the physics simulation.
        /// Movement follows the ship's facing direction, so it
        /// depends on the rotation computed in the same step.
        /// </remarks>
        private void FixedUpdate()
        {
            float angle = Mathf.LerpAngle(
                _rb.rotation,
                _targetAngle,
                Mathf.Clamp01(rotationSpeed * Time.fixedDeltaTime)
            );

            _rb.MoveRotation(angle);

            // Ship-relative movement: W/S along the nose, A/D strafe.
            if (_moveInput.sqrMagnitude > 0.0001f)
                EntityMovement.Move(ToWorldDirection(_moveInput, angle));
            else
                EntityMovement.Stop();
        }

        /// <summary>
        /// Converts local ship input into a world-space direction.
        /// </summary>
        /// <param name="localInput">
        /// Movement input where X strafes right and Y moves forward.
        /// </param>
        /// <param name="rotation">
        /// Rigidbody2D rotation in degrees, including the sprite
        /// alignment offset.
        /// </param>
        /// <remarks>
        /// Uses the rotation applied in this physics step rather than
        /// <c>_rb.rotation</c>, which only reflects MoveRotation after
        /// the simulation runs.
        /// </remarks>
        private Vector2 ToWorldDirection(Vector2 localInput, float rotation)
        {
            float headingRadians =
                (rotation - rotationOffset) * Mathf.Deg2Rad;

            Vector2 forward = new Vector2(
                Mathf.Cos(headingRadians),
                Mathf.Sin(headingRadians)
            );

            // Forward rotated 90 degrees clockwise.
            Vector2 right = new Vector2(forward.y, -forward.x);

            return forward * localInput.y + right * localInput.x;
        }

        /// <summary>
        /// Calculates the target rotation required to face
        /// the current mouse cursor.
        /// </summary>
        /// <remarks>
        /// The cursor position is converted into world space
        /// using the main camera. The calculated direction is
        /// then transformed into a Z-axis rotation suitable
        /// for a top-down 2D game.
        /// </remarks>
        private void HandleRotation()
        {
            if (_mainCamera == null)
                return;

            float cameraDistance = Mathf.Abs(
                transform.position.z -
                _mainCamera.transform.position.z
            );

            Vector3 mouseWorldPosition =
                _mainCamera.ScreenToWorldPoint(
                    new Vector3(
                        _lookInput.x,
                        _lookInput.y,
                        cameraDistance
                    )
                );

            Vector2 direction =
                (Vector2)mouseWorldPosition - _rb.position;

            // Avoid unstable angles when the cursor
            // overlaps the player's position.
            if (direction.sqrMagnitude < 0.0001f)
                return;

            float headingAngle =
                Mathf.Atan2(
                    direction.y,
                    direction.x
                ) * Mathf.Rad2Deg;

            _targetAngle =
                headingAngle + rotationOffset;
        }

        /// <summary>
        /// Receives movement input from the Unity Input System.
        /// </summary>
        /// <param name="context">
        /// Input action context containing a Vector2 movement value.
        /// </param>
        /// <remarks>
        /// The input vector represents local movement:
        /// X for strafing and Y for forward/backward movement.
        /// </remarks>
        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// Receives cursor position updates from the Unity Input System.
        /// </summary>
        /// <param name="context">
        /// Input action context containing the cursor's
        /// screen-space position.
        /// </param>
        /// <remarks>
        /// The Look action must be configured as a pointer
        /// position input rather than a relative mouse delta.
        /// </remarks>
        public void OnLook(InputAction.CallbackContext context)
        {
            _lookInput = context.ReadValue<Vector2>();
        }

        /// <summary>
        /// Unregisters input callbacks when the controller
        /// is destroyed.
        /// </summary>
        /// <remarks>
        /// Prevents callbacks from remaining subscribed
        /// after the component's lifetime ends.
        /// </remarks>
        private void OnDestroy()
        {
            if (_playerInput == null)
                return;

            InputAction move =
                _playerInput.actions["Move"];

            InputAction look =
                _playerInput.actions["Look"];

            move.performed -= OnMove;
            move.canceled -= OnMove;

            look.performed -= OnLook;
            look.canceled -= OnLook;
        }
    }
}
