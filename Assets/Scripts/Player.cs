using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Move")]
    public float forwardSpeed = 9f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float doubleJumpForce = 12f;

    private bool isGrounded;
    private bool canDoubleJump;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) Debug.LogError("Rigidbody2D not found");

        rb.freezeRotation = true;
    }

    void Update()
    {
        // 오른쪽 클릭 = 점프
        if (Input.GetMouseButtonDown(1))
        {
            TryJump();
        }
    }

    void FixedUpdate()
    {
        // 항상 앞으로 이동
        rb.velocity = new Vector2(forwardSpeed, rb.velocity.y);

        // GroundCheck 없이 y속도로 착지 판정
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.05f;

        // 땅이면 다시 2단점프 가능
        if (isGrounded)
            canDoubleJump = true;
    }

    private void TryJump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
        }
    }
}
