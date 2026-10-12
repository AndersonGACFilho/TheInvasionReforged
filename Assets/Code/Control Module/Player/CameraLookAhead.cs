using UnityEngine;

namespace Control_Module.Player
{
    /// <summary>
    /// Moves a camera tracking target ahead of the player,
    /// in the direction the player is facing.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This component belongs on an empty "LookTarget" object used as
    /// the Cinemachine camera's TrackingTarget, not on the camera itself:
    /// the CinemachineBrain owns the camera transform.
    /// </para>
    /// <para>
    /// The CinemachineBrain runs with execution order 100, so this
    /// LateUpdate runs first and the camera sees the target of the
    /// current frame.
    /// </para>
    /// </remarks>
    public class CameraLookAhead : MonoBehaviour
    {
        [Header("References")]

        [Tooltip(
            "Player whose facing direction drives the look-ahead. " +
            "Its rotation offset is reused to recover the heading."
        )]
        [SerializeField]
        private PlayerInputController player;

        [Header("Look Ahead")]

        [Tooltip(
            "Distance in world units the target is placed " +
            "ahead of the player."
        )]
        [SerializeField, Min(0f)]
        private float lookAheadDistance = 3f;

        [Tooltip(
            "How quickly the target catches up with its desired " +
            "position. Adds to the Cinemachine camera's own damping."
        )]
        [SerializeField, Min(0f)]
        private float smoothSpeed = 5f;

        /// <summary>
        /// Places the target at its desired position so the camera
        /// does not sweep in from the scene origin on the first frames.
        /// </summary>
        private void Start()
        {
            if (player == null)
                return;

            transform.position = GetTargetPosition();
        }

        /// <summary>
        /// Moves the target toward the point ahead of the player
        /// with frame-rate independent smoothing.
        /// </summary>
        private void LateUpdate()
        {
            if (player == null)
                return;

            float t = 1f - Mathf.Exp(
                -smoothSpeed * Time.deltaTime
            );

            transform.position = Vector3.Lerp(
                transform.position,
                GetTargetPosition(),
                t
            );
        }

        /// <summary>
        /// Calculates the point ahead of the player along its heading.
        /// </summary>
        /// <remarks>
        /// Position and rotation are both read from the player's
        /// Transform, which is interpolated every frame when the
        /// Rigidbody2D uses interpolation. Reading the rotation from
        /// the Rigidbody2D instead would only change at the physics
        /// step and make the direction move in steps.
        /// The depth is kept from this object, so only XY follows
        /// the player.
        /// </remarks>
        private Vector3 GetTargetPosition()
        {
            Transform playerTransform = player.transform;

            float headingRadians =
                (playerTransform.eulerAngles.z - player.rotationOffset)
                * Mathf.Deg2Rad;

            Vector3 direction = new Vector3(
                Mathf.Cos(headingRadians),
                Mathf.Sin(headingRadians),
                0f
            );

            Vector3 targetPosition =
                playerTransform.position + direction * lookAheadDistance;

            targetPosition.z = transform.position.z;

            return targetPosition;
        }
    }
}
