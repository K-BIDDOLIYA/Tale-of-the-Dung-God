using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2.3f;

    [Header("Jump")]
    public float jumpForce = 8f;

    [Header("Combat")]
    public Transform attackPoint;
    public float attackRange = 0.7f;
    public int attackDamage = 1;
    public float attackCooldown = 0.4f;
    public LayerMask enemyLayers;

    [Header("Player Health")]
    public int maxHealth = 3;
    public GameObject[] hearts;

    private int currentHealth;
    private bool isDead;

    private Rigidbody2D rb;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;

    private bool isGrounded;
    private int facingDirection = 1;
    private float nextAttackTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");

        if (moveAction == null || jumpAction == null || sprintAction == null)
        {
            Debug.LogError(
                "Missing Input Actions! Make sure Move, Jump, and Sprint exist."
            );
        }

        UpdateHearts();
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        jumpAction?.Enable();
        sprintAction?.Enable();
    }

    private void OnDisable()
    {
        moveAction?.Disable();
        jumpAction?.Disable();
        sprintAction?.Disable();
    }

    private void Update()
    {
        if (isDead)
            return;

        // Jump
        if (jumpAction != null &&
            jumpAction.WasPressedThisFrame() &&
            isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        // Sword attack with J
        if (Keyboard.current != null &&
            Keyboard.current.jKey.wasPressedThisFrame &&
            Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + attackCooldown;
        }

        if (moveAction != null)
        {
            float horizontal = moveAction.ReadValue<Vector2>().x;

            if (horizontal > 0.01f)
                facingDirection = 1;
            else if (horizontal < -0.01f)
                facingDirection = -1;

            if (attackPoint != null)
            {
                Vector3 pointPosition = attackPoint.localPosition;
                pointPosition.x = Mathf.Abs(pointPosition.x) * facingDirection;
                attackPoint.localPosition = pointPosition;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isDead || moveAction == null)
            return;

        Vector2 move = moveAction.ReadValue<Vector2>();

        float horizontal = Mathf.Clamp(move.x, -1f, 1f);

        float sprint = sprintAction != null && sprintAction.IsPressed()
            ? sprintMultiplier
            : 1f;

        rb.linearVelocity = new Vector2(
            horizontal * moveSpeed * sprint,
            rb.linearVelocity.y
        );
    }

    private void Attack()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("Assign the Attack Point in the Inspector!");
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(
            attackPoint.position,
            attackRange,
            enemyLayers
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            Guard guard = enemy.GetComponentInParent<Guard>();

            if (guard != null)
            {
                guard.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        UpdateHearts();

        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHearts()
    {
        if (hearts == null)
            return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
            {
                hearts[i].SetActive(i < currentHealth);
            }
        }
    }

    private void Die()
    {
        isDead = true;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        Debug.Log("Player died!");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGround(collision);

        if (collision.gameObject.GetComponentInParent<Guard>() != null)
        {
            TakeDamage(1);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckGround(collision);
    }

    private void CheckGround(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}


