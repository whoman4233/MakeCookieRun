using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    MapManager mapManager;
    
    [SerializeField] private GameObject _scoreItemPrefabs;
    [SerializeField] private GameObject _hpItemPrefabs;

    private Vector3 _lastItemPosition = Vector3.zero;

    void Awake()
    {
        
    }

    void Start()
    {
        InvokeRepeating("SpawnItem", 0f, 1f);
    }

    void SpawnItem()
    {
        GameObject item = Random.Range(0, 2) == 0 ? _scoreItemPrefabs : _hpItemPrefabs;
        Items itemScript = item.GetComponent<Items>();
        Vector3 spawnPosition = itemScript.SetItemPosition(_lastItemPosition);
        Instantiate(item, spawnPosition, Quaternion.identity);
        
        _lastItemPosition = spawnPosition;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Obstacles obstacle = collision.gameObject.GetComponent<Obstacles>();
        if (obstacle == null) return;
        
        string prefabName = collision.gameObject.name;
        float newSpawnY = 0;
        
        if (prefabName.Contains("top"))
        {
            newSpawnY = obstacle.transform.position.y + gameObject.transform.position.y;

        }
        else if (prefabName.Contains("bottom"))
        {
            newSpawnY = gameObject.transform.position.y - obstacle.transform.position.y;

        }
        
        // 위 / 아래 장애물을 확인 > y를 더하거나 빼야함

        // float newSpawnY = obstacle.transform.position.y + gameObject.transform.position.y;
        
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, newSpawnY, gameObject.transform.position.z);
    }
}
