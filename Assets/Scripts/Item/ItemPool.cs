using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private GameObject hpPrefabs;
    [SerializeField] private GameObject scorePrefabs;
    
    private Queue<GameObject> hpPool = new Queue<GameObject>();
    private Queue<GameObject> scorePool = new Queue<GameObject>();
    
    private int poolSizePerPrefab = 15; // 각 프리펩당 15개씩
    
    private void Start()
    {
        InitializePool();
    }
    private void InitializePool()
    {
        // hp 풀 생성
        for (int i = 0; i < poolSizePerPrefab; i++)
        {
            GameObject obj = Instantiate(hpPrefabs);
            obj.SetActive(false);
            hpPool.Enqueue(obj);
        }
        
        // score 풀 생성
        for (int i = 0; i < poolSizePerPrefab; i++)
        {
            GameObject obj = Instantiate(scorePrefabs);
            obj.SetActive(false);
            scorePool.Enqueue(obj);
        }
    }
    
    // 만든 Obstacle을 Pool에서 꺼낸다
    public GameObject GetItem(Items.ItemType type)
    {
        Queue<GameObject> targetPool = null;
        GameObject prefabs = null;
        
        switch (type)
        {
            case Items.ItemType.HP:
                targetPool = hpPool;
                prefabs = hpPrefabs;
                break;
            case Items.ItemType.Score:
                targetPool = scorePool;
                prefabs = scorePrefabs;
                break;
        }
        
        GameObject obj;
        if (targetPool.Count > 0)
        {
            obj = targetPool.Dequeue();
        }
        else
        {
            // 풀이 비었으면 랜덤으로 하나 선택해서 생성
            GameObject randomPrefab = prefabs;
            obj = Instantiate(randomPrefab);
        }
        
        obj.SetActive(true);
        return obj;
    }
    
    public void ReturnObstacle(GameObject obj, Items.ItemType type)
    {
        obj.SetActive(false);
        
        switch (type)
        {
            case Items.ItemType.HP:
                hpPool.Enqueue(obj);
                break;
            case Items.ItemType.Score:
                scorePool.Enqueue(obj);
                break;
        }
    }
}

