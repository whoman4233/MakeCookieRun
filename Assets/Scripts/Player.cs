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

    // ====== Slide: 왼쪽 클릭 눌렀을 때만 콜라이더 높이 축소 ======
    [Header("Slide")]
    public float slideHeightScale = 0.6f;   // 콜라이더 높이 비율(0~1)
    private BoxCollider2D boxCol;           // 줄일 대상
    private Vector2 boxSizeOrig;            // 원래 사이즈
    private Vector2 boxOffsetOrig;          // 원래 오프셋
    private bool isSliding = false;         // 현재 슬라이드 중?

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        // 슬라이드용 BoxCollider2D 확보 & 원본 저장
        boxCol = GetComponent<BoxCollider2D>();
        if (boxCol != null)
        {
            boxSizeOrig = boxCol.size;
            boxOffsetOrig = boxCol.offset;
        }
        else
        {
            Debug.LogWarning("BoxCollider2D가 없습니다. 슬라이드(콜라이더 축소)는 동작하지 않습니다.");
        }
    }

    void FixedUpdate()
    {
        // 항상 앞으로 이동
        rb.velocity = new Vector2(forwardSpeed, rb.velocity.y);

        // GroundCheck 없이 y속도로 착지 판정
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.05f;

        // 땅이면 2단 점프 가능 리셋
        if (isGrounded)
            canDoubleJump = true;
    }

    public void TryJump()
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

    // ----- 슬라이드 시작: 콜라이더 높이만 줄이고, 발 위치 유지 -----
    public void BeginSlide()
    {
        if (boxCol == null || isSliding) return;

        isSliding = true;

        float newH = boxSizeOrig.y * slideHeightScale;                 // 줄인 높이
        boxCol.size = new Vector2(boxSizeOrig.x, newH);                // 높이 축소
        boxCol.offset = boxOffsetOrig + new Vector2(0f,               // 오프셋을 내려서
                            -(boxSizeOrig.y - newH) * 0.5f);           // 발 위치(바닥) 유지
    }

    // ----- 슬라이드 종료: 원래 사이즈/오프셋 복원 -----
    public void EndSlide()
    {
        if (boxCol == null || !isSliding) return;

        boxCol.size = boxSizeOrig;
        boxCol.offset = boxOffsetOrig;
        isSliding = false;
    }
}
