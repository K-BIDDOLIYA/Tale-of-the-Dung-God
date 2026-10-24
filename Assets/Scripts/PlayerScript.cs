using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{
    public float jumpForce;
    public float speedMultiplier;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    public Animator anim;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private int maxHealth = 3;
    private GameObject[] hearts;
    private int currentHealth;
    public bool isGrounded = true;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
    } 
    
    
    
    void Update()
    {
        float move = moveAction.ReadValue<Vector2>().x;
        if (jumpAction.WasPressedThisFrame() && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (sprintAction.IsPressed())
        {
            StartCoroutine(Sprint());
        } 
        rb.linearVelocity = new Vector2(move * speedMultiplier, rb.linearVelocity.y);
        sr.flipX = (move < 0) || sr.flipX && move == 0;
        anim.SetInteger("Movement", Math.Abs((int)rb.linearVelocity.x));
    }

    private IEnumerator Sprint()
    {
        speedMultiplier = 3f; 
       yield return new WaitForSeconds(1.2f);
       speedMultiplier = 2f;
    }
}


