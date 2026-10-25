using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    [Header("Movement")]
    public float jumpForce = 8f;
    public float speedMultiplier = 2f;
    
    
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    [Header("Animation")]
    public Animator anim;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction pauseAction;
    private InputAction attackAction;
    public bool isGrounded = true;

    [Header("Death UI")]
    public GameObject deathPanel;

    [Header("Player Health")]
    public int maxHealth = 3;
    public GameObject[] hearts;

    private int currentHealth;
    private bool isDead;

    [Header("Combat")]
    public int attackDamage = 1;
    public float attackCooldown = 0.4f;

    private float nextAttackTime;
    private int facingDirection = 1;
    private bool isSprinting;

    public float attackDuration = 0.3f;

    private bool isAttacking;

    private readonly HashSet<Guard> hitGuards = new HashSet<Guard>();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        if (anim == null)
            anim = GetComponent<Animator>();

        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        pauseAction = InputSystem.actions.FindAction("Pause");
        attackAction = InputSystem.actions.FindAction("Attack");
    }

    private void OnEnable()
    {
        moveAction?.Enable();
        jumpAction?.Enable();
        sprintAction?.Enable();
        attackAction?.Enable();
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (deathPanel != null)
            deathPanel.SetActive(false);


        UpdateHearts();

        Time.timeScale = 1f;

        if (moveAction == null || jumpAction == null || sprintAction == null)
        {
            Debug.LogError(
                "Missing Input Actions! Make sure Move, Jump, and Sprint exist."
            );
        }
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

        float move = moveAction != null
            ? moveAction.ReadValue<Vector2>().x
            : 0f;

        // Jump
        if (jumpAction != null &&
            jumpAction.WasPressedThisFrame() &&
            isGrounded)
        {
            rb.AddForce(
                Vector2.up * jumpForce,
                ForceMode2D.Impulse
            );

            isGrounded = false;
        }
        

        // Update facing direction
        if (move < -0.01f)
        {
            facingDirection = -1;
        }
        else if (move > 0.01f)
        {
            facingDirection = 1;
        }

        if (sr != null)
            sr.flipX = facingDirection < 0;

        // Keep the knife in front of the player.

        // Right-click to attack.
        if (Mouse.current != null &&
            Mouse.current.rightButton.wasPressedThisFrame &&
            Time.time >= nextAttackTime)
        {
            StartCoroutine(Attack());
            nextAttackTime = Time.time + attackCooldown;
        }

        // Sprint
        if (sprintAction != null &&
            sprintAction.IsPressed() &&
            !isSprinting)
        {
            StartCoroutine(Sprint());
        }

        // Animation
        if (anim != null)
        {
            anim.SetInteger(
                "Movement",
                Mathf.RoundToInt(Mathf.Abs(rb.linearVelocity.x))
            );

            anim.SetBool("IsJumping", !isGrounded);
        }

        if (pauseAction.IsPressed())
        {
            Time.timeScale = 0f;
            GameObject.Find("PauseScreen").GetComponent<Canvas>().enabled = true;
        }

        if (attackAction.IsPressed())
        {
            anim.SetTrigger("IsAttacking");
        }
    }

    private void FixedUpdate()
    {
        if (isDead || moveAction == null)
            return;

        float move = Mathf.Clamp(
            moveAction.ReadValue<Vector2>().x,
            -1f,
            1f
        );

        rb.linearVelocity = new Vector2(
            move * speedMultiplier,
            rb.linearVelocity.y
        );
    }

    private IEnumerator Sprint()
    {
        isSprinting = true;

        float originalSpeed = speedMultiplier;
        speedMultiplier = 3f;

        yield return new WaitForSeconds(1.2f);

        speedMultiplier = originalSpeed;
        isSprinting = false;
    }

    // KNIFE ATTACK
    private IEnumerator Attack()
    {
        if (isDead || isAttacking)
            yield break;

        isAttacking = true;
        hitGuards.Clear();


        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isAttacking || isDead )
            return;

        TryDamageGuard(other);
    }

    private void TryDamageGuard(Collider2D other)
    {
        if (!isAttacking || other == null)
            return;

        // Ignore the player's own colliders.
        if (other.transform.root == transform.root)
            return;

        Guard guard = other.GetComponentInParent<Guard>();

        if (guard == null || hitGuards.Contains(guard))
            return;

        hitGuards.Add(guard);
        guard.TakeDamage(attackDamage);

        Debug.Log("Knife hit guard!");
    }

    
    // PLAYER HEALTH
    private void TakeDamage(int damage)
    {
        if (isDead)
           SceneManager.LoadScene("GameOver"); 
            
        currentHealth = Mathf.Clamp(
            currentHealth - damage,
            0,
            maxHealth
        );

        UpdateHearts();

        Debug.Log("Player health: " + currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void UpdateHearts()
    {
        if (hearts == null)
            return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] != null)
                hearts[i].SetActive(i < currentHealth);
        }
    }

    private void Die()
    {

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        
        
        Time.timeScale = 0f;

        Debug.Log("The mummy has been defeated!");
        SceneManager.LoadScene("GameOver");
    }

    // PLAYER COLLISIONS
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            TakeDamage(1);

        CheckGround(collision);
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

}

