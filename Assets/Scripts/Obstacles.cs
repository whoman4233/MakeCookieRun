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
                Debug.Log("hit!");
                GameManager.Instance.PlayerHP -= damage;
            }
        }
    }
}
