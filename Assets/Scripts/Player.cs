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
        rb.freezeRotation = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 오른쪽 클릭
        {
            TryJump();
        }
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(forwardSpeed, rb.velocity.y);

        isGrounded = Mathf.Abs(rb.velocity.y) < 0.05f;

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
