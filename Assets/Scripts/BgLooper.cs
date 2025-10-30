using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public Camera cam;                     // ���� ī�޶� (������ Start���� MainCamera��)
    public SpriteRenderer tile1, tile2;    // ���� ���� ��� Ÿ�� 2��
    [Range(0f, 1.5f)]
    public float parallax = 1f;            // 1=ī�޶�� ���� �ӵ�(����), 0.3~0.6=����

    float width;          // Ÿ�� �� (������/PPU �ݿ�)
    float halfViewW;      // ī�޶� ���� ������
    float lastCamX;       // ���� ī�޶� X
    Transform t1, t2;

    void Start()
    {
        if (!cam) cam = Camera.main;
        if (!tile1 || !tile2 || !cam) { enabled = false; return; }

        t1 = tile1.transform;
        t2 = tile2.transform;

        // Ÿ�� ��(�����ϱ��� �ݿ�) + ī�޶� ���� ����
        width = tile1.bounds.size.x;
        halfViewW = cam.orthographicSize * cam.aspect;

        // ���� �� BG2�� BG1 �����ʿ� ��Ȯ�� ����(�ʱ� ���� ����)
        t2.position = new Vector3(t1.position.x + width, t1.position.y, t1.position.z);

        lastCamX = cam.transform.position.x;
    }

    void LateUpdate()
    {
        float camX = cam.transform.position.x;
        float dxCam = camX - lastCamX;

        // 1) ����� 'ī�޶� �̵��� * �Ķ������' ��ŭ �̵�
        if (dxCam != 0f)
        {
            Vector3 move = new Vector3(dxCam * parallax, 0f, 0f);
            t1.position += move;
            t2.position += move;
            lastCamX = camX;
        }

        // 2) ī�޶� ���� ȭ�� ��(Ÿ�� �߽� ���� ���� ����)���� ������ ��� Ÿ�ϸ� ������ �� �ڷ� ����
        float leftEdge = camX - halfViewW - (width * 0.5f);

        // �� ���ʿ� �ִ� Ÿ�Ϻ��� ó��(���� ���� �� ���� �Ѿ�� ����)
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
