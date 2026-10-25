    using System;
    using System.Collections;
    using UnityEngine;
    using UnityEngine.InputSystem;

    public class Hitbox : MonoBehaviour
    {
        private BoxCollider2D collider;
        private InputAction attackAction;
        public GameObject player;
        public SpriteRenderer spriteRenderer1;
        private void Start()
        {
            
            collider = GetComponent<BoxCollider2D>();
            collider.isTrigger = true;
            collider.enabled = false;
            attackAction = InputSystem.actions.FindAction("Attack");
        }

        private void Update()
        {
            if (attackAction.IsPressed())
            {
                StartCoroutine(attackCollider());
            }
        }

        private IEnumerator attackCollider()
        {
            collider.enabled = true;
            if (spriteRenderer1.flipX)
            {
                collider.offset = new Vector2(-0.53f, 0f);
            }
            else
            {
            }

            yield return new WaitForSeconds(1.2f);
            collider.enabled = false;
        } 
    }