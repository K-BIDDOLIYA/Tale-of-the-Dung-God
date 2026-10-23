using UnityEngine;

public class Guard : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float speed = 2f;
    public float leftLimit = -5f;
    public float rightLimit = 5f;

    private int direction = 1;

    void Update()
    {
        // Move the guard horizontally
        transform.Translate(Vector2.right * direction * speed * Time.deltaTime);

        // Reverse direction at the patrol limits
        if (transform.position.x >= rightLimit)
        {
            direction = -1;
        }
        else if (transform.position.x <= leftLimit)
        {
            direction = 1;
        }

        // Flip the guard's appearance to face its movement direction
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}

