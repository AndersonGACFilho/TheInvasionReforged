using UnityEngine;

/// <summary>
/// Makes a UI element follow a target transform with an optional offset.
/// </summary>
public class UIFollowTarget : MonoBehaviour
{
    [Tooltip("The target transform to follow.")]
    public Transform target;

    [Tooltip("Offset from the target's position.")]
    public Vector3 offset = new Vector3(0, 1.2f, 0);

    void LateUpdate()
    {
        if (!target)
            return;
        transform.position = target.position + offset;

        transform.rotation = Quaternion.identity;
    }
}