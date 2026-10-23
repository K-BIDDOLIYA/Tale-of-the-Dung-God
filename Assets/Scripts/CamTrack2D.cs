using UnityEngine;

public class CamTrack2D : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Camera Settings")]
    public Vector3 offset = new Vector3(0, 0.72f, -10);
    public float smoothTime = 0.3f;

    [Header("Horizontal Limits")]
    public float minX = 0f;

    private float currentVelocityX;

    private void LateUpdate()
    {
        if (target == null)
            return;

        float targetX = target.position.x + offset.x;

        targetX = Mathf.Max(targetX, minX);

        float newX = Mathf.SmoothDamp(
            transform.position.x,
            targetX,
            ref currentVelocityX,
            smoothTime
        );

        transform.position = new Vector3(
            newX,
            transform.position.y,
            offset.z
        );
    }
}

