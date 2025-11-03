using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ItemPool itemPool;
    [SerializeField] private MapManager mapManager;

    [Header("Initial Spawn Settings")] 
    [SerializeField] private int initialItemSpawnCount = 25;
    [SerializeField] private float initialItemSpawnStartX = 10f;
    
    [Header("Spawn Settings")]
    // [SerializeField] private float minSpawnInterval = 3f;
    // [SerializeField] private float maxSpawnInterval = 7f;
    [SerializeField] private float spawnYRange = 3f;            // 중심 기준 위아래 랜덤 범위
    // [SerializeField] private float spawnXPaddingMin = 3f;
    // [SerializeField] private float spawnXPaddingMax = 3f;
    [SerializeField] private float obstaclePadding = 0.5f; 
    [SerializeField] private float spawnRepeatCycle = 2f;// Obstacle과의 간격
    
    [Header("Follow Player")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Vector3 offset = new Vector3(20f, 0f, 0f);
    
    [Header("Collision Check")]
    [SerializeField] private float checkRadius = 1f;          // 충돌 검사 반경
    [SerializeField] private LayerMask obstacleLayer;           // Obstacle 레이어
    
    // private float _nextSpawnDistance = 0f;
    // private float _lastSpawnX = 0f;

    private GameObject lastItem = null;
    
    bool isInitialized = false;
    
    Vector3 spawnPosition = Vector3.zero;

    void Start()
    {
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }
        
        if (itemPool == null)
            itemPool = FindObjectOfType<ItemPool>();
        
        if (mapManager == null)
            mapManager = FindObjectOfType<MapManager>();
        
        // _lastSpawnX = transform.position.x;

        InvokeRepeating(nameof(ContinuousSpawn), 0f, spawnRepeatCycle);
    }

    // 초반 세팅
    /*IEnumerator InitialSpawn()
    {
        _lastSpawnX = initialItemSpawnStartX;

        for (int i = 0; i < initialItemSpawnCount; i++)
        {
            SpawnItem();

            if (i % 5 == 0)
                yield return null;
        }
        
        isInitialized = true;
        Debug.Log("Spawn Complete");
        
        InvokeRepeating(nameof(ContinuousSpawn), 0f, spawnRepeatCycle);
    }*/

    void ContinuousSpawn()
    {
        // if (!isInitialized) return;
        SpawnItem();
    }

    // 아이템 스폰
    void SpawnItem()
    {
        Items.ItemType itemType = GetRandomItemType();
        
        GameObject item = itemPool.GetItem(itemType);
        if (item == null) return;
        
        SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
        
        // float itemHalfWidth = spriteRenderer.bounds.size.x / 2;
        
        // float spawnX = gameObject.transform.position.x;
        // float spawnY = gameObject.transform.position.y;
        
        spawnPosition = gameObject.transform.position;
        
        Collider2D hitObstacle = Physics2D.OverlapCircle(spawnPosition, checkRadius, obstacleLayer);
        
        if (hitObstacle != null)
        {
            Debug.Log($"장애물 감지: {hitObstacle.gameObject.name}");
            spawnPosition = AdjustPositionForObstacle(spawnPosition, hitObstacle);
        }
        else
        {
            Debug.Log($"장애물 감지 X");
        }
        
        item.transform.position = spawnPosition;
        
        // _lastSpawnX = spawnX;
        // lastItem = item;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, checkRadius); // 스포너 기준 원
    }
    
    // Item의 종류가 많을 때 사용
    // Item Type 랜덤 뽑기
    public Items.ItemType GetRandomItemType()
    {
        var values = System.Enum.GetValues(typeof(Items.ItemType));
        
        int index = Random.Range(0, values.Length);
        
        return (Items.ItemType)values.GetValue(index);
    }

    private Vector3 AdjustPositionForObstacle(Vector3 position, Collider2D obstacle)
    {
        Bounds obstacleBounds = obstacle.bounds;
        Vector3 adjustedPosition = position;

        string obstacleName = obstacle.gameObject.name;

        if (obstacleName.ToLower().Contains("bottom"))
        {
            adjustedPosition.y = 1.1f;
        }
        else if (obstacleName.ToLower().Contains("double"))
        {
            adjustedPosition.y = 2.6f;
        }
        else if (obstacleName.ToLower().Contains("top"))
        {
            adjustedPosition.y = -2.5f;
        }

        return adjustedPosition;
    }
    
    // ItemSpawner 위치 업데이트
    void LateUpdate()
    {
        if (playerTransform != null)
        {
            transform.position = new Vector3(
                playerTransform.position.x + offset.x,
                transform.position.y,
                transform.position.z
            );
        }
    }
}
