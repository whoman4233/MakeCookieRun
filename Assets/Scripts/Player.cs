using UnityEngine;
using Assets.Scripts.Manager;
using System.Collections; // [ADDED] 코루틴 사용을 위한 네임스페이스

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

    private Animator playerAnim;

    // ====== Invincibility(잠깐 무적) ======
    [Header("Invincibility")]
    [SerializeField] private float invincibleTime = 1.0f;   // [ADDED] 피격 후 무적 유지 시간(초)
    private bool isInvincible = false;    // [ADDED] 현재 무적인가?

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

        playerAnim = GetComponentInChildren<Animator>();
    }

    void FixedUpdate()
    {
        // 항상 앞으로 이동
        rb.velocity = new Vector2(forwardSpeed, rb.velocity.y);

        // GroundCheck 없이 y속도로 착지 판정
        isGrounded = Mathf.Abs(rb.velocity.y) < 0.05f;

        // 땅이면 2단 점프 가능 리셋
        if (isGrounded)
        {
            playerAnim.SetBool("isGrounded", true);
            canDoubleJump = true;
            playerAnim.SetBool("CanDoubleJump", true);
        }
    }

    public void TryJump()
    {
        if (isGrounded)
        {
            EventManager.RequestSfxPlay("NormalJump");
            playerAnim.SetBool("isGrounded", false);
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
        else if (canDoubleJump)
        {
            EventManager.RequestSfxPlay("DoubleJump");
            playerAnim.SetBool("CanDoubleJump", false);
            canDoubleJump = false;
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
        }
    }

    // ----- 슬라이드 시작: 콜라이더 높이만 줄이고, 발 위치 유지 -----
    public void BeginSlide()
    {
        if (boxCol == null || isSliding) return;

        EventManager.RequestSfxPlay("Slide");
        isSliding = true;
        playerAnim.SetBool("OnSlide", true);

        float newH = boxSizeOrig.y * slideHeightScale;                 // 줄인 높이
        boxCol.size = new Vector2(boxSizeOrig.x, newH);                // 높이 축소
        boxCol.offset = boxOffsetOrig + new Vector2(0f,                // 오프셋을 내려서
                            -(boxSizeOrig.y - newH) * 0.5f);           // 발 위치(바닥) 유지
    }

    // ----- 슬라이드 종료: 원래 사이즈/오프셋 복원 -----
    public void EndSlide()
    {
        if (boxCol == null || !isSliding) return;

        playerAnim.SetBool("OnSlide", false);
        boxCol.size = boxSizeOrig;
        boxCol.offset = boxOffsetOrig;
        isSliding = false;
    }

    // 장애물과 충돌 시 Trigger 변경
    public void TakeHit()
    {
        if (isInvincible) return; // 무적이면 무시

        // HP 깎기
        GameManager.Instance.PlayerHP -= 10; // ← 상수로 10! (나중에 바꾸면 됨)

        // 맞은 애니메이션
        if (playerAnim != null)
            playerAnim.SetTrigger("OnHit");

        // 무적 시작
        StartCoroutine(InvincibleCoroutine());
    }

    // [ADDED] invincibleTime 동안 추가 피격 무시
    private IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}
