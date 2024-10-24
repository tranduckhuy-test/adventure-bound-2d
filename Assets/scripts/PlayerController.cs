using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    public float collisionOffset = 0.02f;

    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;

    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    Vector2 movementInput;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;

    bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }



    void FixedUpdate()
    {
        // canMove is used to prevent player movement while attacking
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                // First check for general movement
                // If blocked, then check for movement in a single direction
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0f));

                }

                if (!success)
                {
                    success = TryMove(new Vector2(0f, movementInput.y));

                }

                // Set sprite direction based on movement direction
                animator.SetBool("isWalking", success);

            }
            else
            {
                animator.SetBool("isWalking", false);
            }

            if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }

        }
    }

    // TryMove only moves if bool == true
    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            // Check for potential collisions, 0 means no collision
            int count = rb.Cast(
                direction, // x and y values between -1 and 1 to determine collision direction
                movementFilter, // Determines where collisions occur or which layers collide
                castCollisions, // List to store collisions that occur after casting
                moveSpeed * Time.fixedDeltaTime + collisionOffset); //

            if (count == 0)
            {
                // rb.AddForce(movementInput * moveSpeed * Time.fixedDeltaTime);
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    // Handle move input
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();

        // This if condition keeps the input value at the last inputted value
        // Prevents flipping direction from left to right when releasing left input
        // Since input value becomes 0, it will set the float to the default value (0)
        if (movementInput != Vector2.zero)
        {
            animator.SetFloat("XInput", movementInput.x);
            animator.SetFloat("YInput", movementInput.y);
        }
    }

    void OnFire()
    {
        SFXManager.instance.SlashSound();
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack()
    {
        LockMovement();

        if (animator.GetFloat("XInput") != 0)
        {
            if (spriteRenderer.flipX == true)
            {
                swordAttack.AttackLeft();
            }
            else
            {
                swordAttack.AttackRight();
            }
        }
        else if (animator.GetFloat("YInput") == 1)
        {
            swordAttack.AttackUp();
        }
        else if (animator.GetFloat("YInput") == -1)
        {
            swordAttack.AttackDown();
        }
    }

    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}
