using UnityEngine;

public class Guard : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float speed = 2f;
    public float patrolDistance = 3f;

    [Header("Combat Settings")]
    public int maxHealth = 3;

    private int currentHealth;
    private int direction = 1;
    private float leftLimit;
    private float rightLimit;
    private float startX;

    void Start()
    {
        startX = transform.position.x;
        leftLimit = startX - patrolDistance;
        rightLimit = startX + patrolDistance;

        currentHealth = maxHealth;
    }

    void Update()
    {
        transform.Translate(
            Vector2.right * direction * speed * Time.deltaTime,
            Space.World
        );

        if (transform.position.x >= rightLimit)
        {
            transform.position = new Vector3(
                rightLimit,
                transform.position.y,
                transform.position.z
            );

            direction = -1;
        }
        else if (transform.position.x <= leftLimit)
        {
            transform.position = new Vector3(
                leftLimit,
                transform.position.y,
                transform.position.z
            );

            direction = 1;
        }
        
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log("Guard took damage! Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}

