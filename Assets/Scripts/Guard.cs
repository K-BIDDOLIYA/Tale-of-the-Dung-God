using UnityEngine;

public class Guard : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float speed = 2f;
    public float patrolDistance = 3f;

    [Header("Combat Settings")]
    public int maxHealth = 2;

    private int currentHealth;
    private int direction = 1;
    private float leftLimit;
    private float rightLimit;
    private bool isDead;

    private void Start()
    {
        leftLimit = transform.position.x - patrolDistance;
        rightLimit = transform.position.x + patrolDistance;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (isDead)
            return;

        // Patrol left and right only.
        float nextX = transform.position.x
                      + direction * speed * Time.deltaTime;

        if (nextX >= rightLimit)
        {
            nextX = rightLimit;
            direction = -1;
        }
        else if (nextX <= leftLimit)
        {
            nextX = leftLimit;
            direction = 1;
        }

        transform.position = new Vector3(
            nextX,
            transform.position.y,
            transform.position.z
        );

        // Face the direction of movement.
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        Debug.Log("Guard health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }
}

