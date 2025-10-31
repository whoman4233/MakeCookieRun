using UnityEngine;

public class ObstacleReturner : MonoBehaviour
{
    private ObstacleType obstacleType;
    private Camera cam;
    private void Awake()
    {
        cam = Camera.main;

        if (cam == null)
            Debug.LogWarning("Awake 시점에 Camera.main이 null입니다!");
    }
    
    private void Start()
    {
        string objName = gameObject.name.Replace("(Clone)", "").Trim();
        
        if (objName.Contains("Top"))
            obstacleType = ObstacleType.Top;
        else if (objName.Contains("Double"))
            obstacleType = ObstacleType.Double;
        else if (objName.Contains("Bottom"))
            obstacleType = ObstacleType.Bottom;
    }
    
    // 화면 밖으로 나가면 pool로 return
    private void OnBecameInvisible()
    {
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
            {
                Debug.Log("Camera is Null");
                return;
            }
        }

        // 2️⃣ 카메라가 존재한다면 안전하게 계산
        float leftEdge = cam.ViewportToWorldPoint(new Vector3(0, 0.5f, cam.nearClipPlane)).x;

        if (transform.position.x < leftEdge - 2f)
        {
            ReturnToPool();
        }
    }
    
    public void ReturnToPool()
    {
        // cam = Camera.main;
        
        ObstaclePool pool = FindObjectOfType<ObstaclePool>();
        if (pool != null)
        {
            pool.ReturnObstacle(gameObject, obstacleType);
        }
    }
}