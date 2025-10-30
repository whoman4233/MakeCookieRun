using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;

    [Header("Move")]
    public float forwardSpeed = 9f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float doubleJumpForce = 12f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool canDoubleJump;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (animator == null) Debug.LogError("Animator not found");
        if (rb == null) Debug.LogError("Rigidbody2D not found");

        transform.rotation = Quaternion.identity;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 스페이스바 또는 마우스 왼쪽 클릭 시 점프
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            TryJump();
        }

        // 애니메이터 파라미터 (선택)
        if (animator != null)
        {
            animator.SetBool("Grounded", isGrounded);
            animator.SetFloat("VerticalSpeed", rb.velocity.y);
            animator.SetFloat("SpeedX", Mathf.Abs(rb.velocity.x));
        }
    }

    void FixedUpdate()
    {
        // 항상 앞으로 이동
        rb.velocity = new Vector2(forwardSpeed, rb.velocity.y);

        // 지상 판정
        if (groundCheck != null)
        {
            isGrounded = Physics2D.OverlapCircle(
                groundCheck.position, groundCheckRadius, groundLayer
            );
        }
        else
        {
            isGrounded = rb.velocity.y == 0f; // 임시 대체
        }

        // 착지하면 2단 점프 가능
        if (isGrounded)
            canDoubleJump = true;
    }

    private void TryJump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            animator?.SetTrigger("Jump");
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
            animator?.SetTrigger("DoubleJump");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
