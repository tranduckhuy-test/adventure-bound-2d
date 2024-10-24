using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public string target = "Player";
    Rigidbody2D rb;
    Vector2 direction;
    bool playerDetected;

    Vector3 targetPos;
    Animator animator;
    SpriteRenderer render;
    float moveSpeed = 1f;

    [SerializeField] bool attackType; // Only need to check attack range if this is checked

    Monster monster;

    public float MoveSpeed { get { return moveSpeed; } set { moveSpeed = value; } }

    public bool canApproach = true; // Used to handle cooldown when knocked back

    Vector3 lTemp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        animator = GetComponentInParent<Animator>();
        render = GetComponentInParent<SpriteRenderer>();
        monster = GetComponentInParent<Monster>();
        lTemp = monster.gameObject.GetComponent<Transform>().localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Handle monster movement
        if (canApproach) // Only move if not on cooldown from being hit
        {
            if (playerDetected && targetPos != Vector3.zero)
            {
                direction = (targetPos - transform.position);
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

                animator.SetFloat("XInput", direction.x);
                animator.SetFloat("YInput", direction.y);
                animator.SetBool("isMoving", true);

                // Flip sprite when moving left or right
                if (direction.x > 0)
                {
                    lTemp.x = 1;
                    monster.gameObject.GetComponent<Transform>().localScale = lTemp;
                }
                else if (direction.x < 0)
                {
                    lTemp.x = -1;
                    monster.gameObject.GetComponent<Transform>().localScale = lTemp;
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
        {
            float distance = Vector3.Distance(collision.transform.position, transform.position);

            playerDetected = true;
            targetPos = collision.transform.position;

            if (distance <= monster.AttackRange)
            {
                animator.SetBool("isAttack", true);
            }
            else if (distance > monster.AttackRange)
            {
                animator.SetBool("isAttack", false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(target))
        {
            targetPos = Vector3.zero;
            playerDetected = false;
            direction = Vector2.zero;
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttack", false);
        }
    }
}
