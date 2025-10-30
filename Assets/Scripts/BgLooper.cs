using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public Camera cam;                     // 따라갈 카메라 (없으면 Start에서 MainCamera로)
    public SpriteRenderer tile1, tile2;    // 같은 폭의 배경 타일 2장
    [Range(0f, 1.5f)]
    public float parallax = 1f;            // 1=카메라와 동일 속도(지면), 0.3~0.6=원경

    float width;          // 타일 폭 (스케일/PPU 반영)
    float halfViewW;      // 카메라 반쪽 가로폭
    float lastCamX;       // 직전 카메라 X
    Transform t1, t2;

    void Start()
    {
        if (!cam) cam = Camera.main;
        if (!tile1 || !tile2 || !cam) { enabled = false; return; }

        t1 = tile1.transform;
        t2 = tile2.transform;

        // 타일 폭(스케일까지 반영) + 카메라 가로 반폭
        width = tile1.bounds.size.x;
        halfViewW = cam.orthographicSize * cam.aspect;

        // 시작 시 BG2를 BG1 오른쪽에 정확히 붙임(초기 오차 제거)
        t2.position = new Vector3(t1.position.x + width, t1.position.y, t1.position.z);

        lastCamX = cam.transform.position.x;
    }

    void LateUpdate()
    {
        float camX = cam.transform.position.x;
        float dxCam = camX - lastCamX;

        // 1) 배경을 '카메라 이동량 * 파라랄랙스' 만큼 이동
        if (dxCam != 0f)
        {
            Vector3 move = new Vector3(dxCam * parallax, 0f, 0f);
            t1.position += move;
            t2.position += move;
            lastCamX = camX;
        }

        // 2) 카메라 왼쪽 화면 밖(타일 중심 기준 반폭 포함)으로 완전히 벗어난 타일만 오른쪽 끝 뒤로 붙임
        float leftEdge = camX - halfViewW - (width * 0.5f);

        // 더 왼쪽에 있는 타일부터 처리(여러 장이 한 번에 넘어가도 안전)
        if (t1.position.x <= t2.position.x)
        {
            if (t1.position.x < leftEdge)
                t1.position = new Vector3(t2.position.x + width, t1.position.y, t1.position.z);
            if (t2.position.x < leftEdge)
                t2.position = new Vector3(t1.position.x + width, t2.position.y, t2.position.z);
        }
        else
        {
            if (t2.position.x < leftEdge)
                t2.position = new Vector3(t1.position.x + width, t2.position.y, t2.position.z);
            if (t1.position.x < leftEdge)
                t1.position = new Vector3(t2.position.x + width, t1.position.y, t1.position.z);
        }
    }
}
