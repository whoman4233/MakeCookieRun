using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _topObstaclePrefabs;
    [SerializeField] private GameObject[] _bottomObstaclePrefabs;
    
    private Random _rand = new Random();
    private SpriteRenderer _spriteRenderers;
    
    private float _widthPaddingObstacle;                // 장애물 좌우 패딩값
    private float _widthPaddingMinObstacle = 1f;                // 장애물 좌우 패딩 최소값
    private float _widthPaddingMaxObstacle = 1.5f;              // 장애물 좌우 패딩 최대값

    private float _heightPaddingObstacle;               // 장애물 상하 패딩값
    private float _heightPaddingMinObstacle = 1f;               // 장애물 상하 패딩 최소값
    private float _heightPaddingMaxObstacle = 1.5f;             // 장애물 상하 패딩 최대값
    
    private float _spawnX;                              // 장애물 스폰 X 좌표
    private float _spawnY;                              // 장애물 스폰 Y 좌표
    private Vector3 _spawnPositionObstacle;             // 장애물 스폰 좌표

    private float _widthPaddingItem;
    private float _widthPaddingMinItem = 1f;
    private float _widthPaddingMaxItem = 3f;
    
    private float _spawnPositionItem;
    
    
    private void Awake()
    {
        
    }

    private void Start()
    {
        // if (_obstaclePrefabs.Length > 0)
        //     GetObstacle();
    }
    
    private void GetObstacle()
    {
        // int index = UnityEngine.Random.Range(0, _obstaclePrefabs.Length);
    }

    private void GetItemList()
    {
        
    }
    
    // 
    public Vector3 SetObstacle(Vector3 lastPosition)
    {
        // 장애물 상하 방향 설정
        int dir = _rand.Next(0, 2) ==  0 ? -1 : 1;
        
        // 위 / 아래 장애물 랜덤 소환
        GameObject obstacle;
        
        if (dir == 1)
            obstacle = _topObstaclePrefabs[_rand.Next(0, _topObstaclePrefabs.Length)];
        else
            obstacle = _bottomObstaclePrefabs[_rand.Next(0, _bottomObstaclePrefabs.Length)];
        
        // 중간 패딩 사이즈
        _heightPaddingObstacle = UnityEngine.Random.Range(_heightPaddingMinObstacle, _heightPaddingMaxObstacle);
            
        // 장애물의 길이 확인
        SpriteRenderer spriteRenderer = obstacle.GetComponent<SpriteRenderer>();
        float halfHeight = spriteRenderer.bounds.size.y / 2;
        
        float screenHalfHeight = Camera.main.orthographicSize;
        
        // 오브젝트의 끝이 튀는 것을 방지
        if (halfHeight + _heightPaddingObstacle > screenHalfHeight)
            _spawnY = dir * (halfHeight + _heightPaddingObstacle);
        else
            _spawnY = dir * (halfHeight + _heightPaddingObstacle * 1.5f);
        
        // 장애물 X축 위치 확인
        _widthPaddingObstacle = UnityEngine.Random.Range(_widthPaddingMinObstacle, _widthPaddingMaxObstacle);
        _spawnX = lastPosition.x + _widthPaddingObstacle;
        
        // 장애물 position 지정
        _spawnPositionObstacle = new Vector3(_spawnX, _spawnY, 0);
        
        obstacle.transform.position = _spawnPositionObstacle;
        
        GameObject newObstacle = Instantiate(obstacle, _spawnPositionObstacle, Quaternion.identity);
        
        // 마지막 생성 위치 저장
        return newObstacle.transform.position;
    }

    private void SetItem()
    {
        
    }
}
