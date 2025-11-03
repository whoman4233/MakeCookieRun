using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Assets.Scripts.Manager;

public class Items : MonoBehaviour
{ 
    public enum ItemType
    {
        HP,
        Score
    }
    
    public ItemType itemType;   // HP / Score
    private int hpEffect = 10;
    private int scoreEffect = 100;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyItemEffect(collision.gameObject);
            ReturnToPool();
        }
    }
    
    private void ReturnToPool()
    {
        ItemPool pool = FindObjectOfType<ItemPool>();
        
        if (pool != null)
        {
            pool.ReturnObstacle(gameObject, itemType);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void ApplyItemEffect(GameObject player)
    {
        // 여기서 플레이어 상태 적용
        switch (itemType)
        {
            case ItemType.HP:
                Debug.Log($"플레이어 HP +{hpEffect}");
                GameManager.Instance.PlayerHP += hpEffect;
                EventManager.RequestSfxPlay("AddHP");
                break;
            case ItemType.Score:
                Debug.Log($"플레이어 Score +{scoreEffect}");
                GameManager.Instance.Score += scoreEffect;
                EventManager.RequestSfxPlay("AddScore");
                break;
        }
    }
}
