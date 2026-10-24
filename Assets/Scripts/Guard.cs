using System.Collections;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [Header("Patrol Settings")]
    public float speed = 2f;
    public float patrolDistance = 3f;
    public bool patrolState = true;
     Vector3 oldPosition;
    private Rigidbody2D rb;
    
    
    [Header("Combat Settings")]
    public int maxHealth = 3;

    private int currentHealth;
    private int direction = 1;
    private float leftLimit;
    private float rightLimit;
    private float startX;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            patrolState = false;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !patrolState)
        {
          transform.position = Vector2.MoveTowards(transform.position, collision.transform.position, speed * Time.deltaTime); 
        }
    } 
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !patrolState)
        {
            patrolState = true;
            transform.position = Vector2.MoveTowards(transform.position, oldPosition, speed * Time.deltaTime);
        }
    }
    
    void Start()
    {
        startX = transform.position.x;
        leftLimit = startX - patrolDistance;
        rightLimit = startX + patrolDistance;
        oldPosition = transform.position;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (patrolState)
        {
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
            transform.localScale = scale; transform.Translate(
                Vector2.right * direction * speed * Time.deltaTime,
                Space.World
            );


        }
              
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
//TODO: Make the enemy flash red when hurt for a few ms before going back to normal.
    private IEnumerator Hurt()
    {
        yield return new WaitForSeconds(0.5f);
    }
    
    void Die()
    {
        Destroy(gameObject);
    }
}

