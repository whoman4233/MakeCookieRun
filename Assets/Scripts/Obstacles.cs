using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    public ObstacleType obstacleType;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 체력 감소
            if (GameManager.Instance != null)
            {
                GameManager.Instance.PlayerHP -= damage;
            }
        }
    }
    
    // 화면 밖으로 나가면 풀로 반환
    private void OnBecameInvisible()
    {
        ReturnToPool();
    }
    
    public void ReturnToPool()
    {
        ObstaclePool pool = FindObjectOfType<ObstaclePool>();
        if (pool != null)
        {
            pool.ReturnObstacle(gameObject, obstacleType);
        }
    }
}
