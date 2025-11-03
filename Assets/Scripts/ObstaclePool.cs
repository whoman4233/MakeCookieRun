using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePool : MonoBehaviour
{
    [SerializeField] private GameObject[] topObstaclePrefabs;
    [SerializeField] private GameObject[] bottomObstaclePrefabs;
    [SerializeField] private GameObject[] doubleObstaclePrefabs;
    
    private Queue<GameObject> topPool = new Queue<GameObject>();
    private Queue<GameObject> bottomPool = new Queue<GameObject>();
    private Queue<GameObject> doublePool = new Queue<GameObject>();
    
    private int poolSizePerPrefab = 15; // 각 프리펩당 15개씩
    
    private void Start()
    {
        InitializePool();
    }
    private void InitializePool()
    {
        // Top 풀 생성 + 섞기
        List<GameObject> tempTopList = new List<GameObject>();
        foreach (GameObject prefab in topObstaclePrefabs)
        {
            for (int i = 0; i < poolSizePerPrefab; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                tempTopList.Add(obj);
            }
        }
        ShuffleList(tempTopList);
        foreach (GameObject obj in tempTopList)
        {
            topPool.Enqueue(obj);
        }
    
        // Bottom 풀 생성 + 섞기
        List<GameObject> tempBottomList = new List<GameObject>();
        foreach (GameObject prefab in bottomObstaclePrefabs)
        {
            for (int i = 0; i < poolSizePerPrefab; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                tempBottomList.Add(obj);
            }
        }
        ShuffleList(tempBottomList);
        foreach (GameObject obj in tempBottomList)
        {
            bottomPool.Enqueue(obj);
        }
    
        // Double 풀 생성 + 섞기
        List<GameObject> tempDoubleList = new List<GameObject>();
        foreach (GameObject prefab in doubleObstaclePrefabs)
        {
            for (int i = 0; i < poolSizePerPrefab; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                tempDoubleList.Add(obj);
            }
        }
        ShuffleList(tempDoubleList);
        foreach (GameObject obj in tempDoubleList)
        {
            doublePool.Enqueue(obj);
        }
    }

    // 프리펩 리스트 셔플
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
    
    // 만든 Obstacle을 Pool에서 꺼낸다
    public GameObject GetObstacle(ObstacleType type)
    {
        Queue<GameObject> targetPool = null;
        GameObject[] prefabs = null;
        
        switch (type)
        {
            case ObstacleType.Top:
                targetPool = topPool;
                prefabs = topObstaclePrefabs;
                break;
            case ObstacleType.Bottom:
                targetPool = bottomPool;
                prefabs = bottomObstaclePrefabs;
                break;
            case ObstacleType.Double:
                targetPool = doublePool;
                prefabs = doubleObstaclePrefabs;
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
            GameObject randomPrefab = prefabs[Random.Range(0, prefabs.Length)];
            obj = Instantiate(randomPrefab);
        }
        
        obj.SetActive(true);
        return obj;
    }
    
    // 화면 밖으로 나가면 풀로 반환
    // 랜덤이 아니라 제일 마지막에 있는 걸로
    
    public void ReturnObstacle(GameObject obj, ObstacleType type)
    {
        obj.SetActive(false);
        
        switch (type)
        {
            case ObstacleType.Top:
                topPool.Enqueue(obj);
                break;
            case ObstacleType.Bottom:
                bottomPool.Enqueue(obj);
                break;
            case ObstacleType.Double:
                doublePool.Enqueue(obj);
                break;
        }
    }
}

public enum ObstacleType
{
    Top,      // 위에서 아래
    Bottom,   // 아래에서 위 (점프)
    Double    // 아래에서 위 (더블점프)
}