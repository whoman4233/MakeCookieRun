using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MapManager : MonoBehaviour
{
    private Random _rand = new Random();
    private GameObject[] _obstaclePrefabs;
    private SpriteRenderer _spriteRenderers;
    
    private Vector3 lastPosition = Vector3.zero;

    private float _heightPaddingMin = 1f;
    private float _heightPaddingMax = 3f;
    
    private float _widthPaddingObstacle;
    private float _paddingMin;
    private float _paddingMax;
    
    private float _spawnY;
    private float _spawnX;
    private Vector3 _spawnPositionObstacle;

    private float _widthPaddingItem;
    private float _spawnPositionItem;
    
    
    private void Awake()
    {
        
    }

    private void Start()
    {
        if (_obstaclePrefabs.Length > 0)
            GetObstacle();
    }
    
    private void GetObstacle()
    {
        int index = UnityEngine.Random.Range(0, _obstaclePrefabs.Length);
    }

    private void GetItemList()
    {
        
    }

    private void SetObstacle()
    {
        // 프리펩 불러오기
        int index = UnityEngine.Random.Range(0, _obstaclePrefabs.Length);
        GameObject obstacle = Instantiate(_obstaclePrefabs[index], _spawnPositionObstacle, Quaternion.identity);
        
        // 장애물 상하 방향 설정
        int dir = _rand.Next(0, 2) ==  0 ? -1 : 1;
        
        // 중간 패딩 사이즈
        float heightPadding = UnityEngine.Random.Range(_heightPaddingMin, _heightPaddingMax);
            
        // 장애물의 길이 확인
        SpriteRenderer spriteRenderer = obstacle.GetComponent<SpriteRenderer>();
        float halfHeight = spriteRenderer.bounds.size.y / 2;

        /*// 위에서 소환
        if (dir == 1)
        {
            // 중간 패딩 사이즈
            float heightPadding = UnityEngine.Random.Range(_heightPaddingMin, _heightPaddingMax);
            
            // 장애물의 길이 확인
            SpriteRenderer spriteRenderer = obstacle.GetComponent<SpriteRenderer>();
            float halfHeight = spriteRenderer.bounds.size.y / 2;
            
            _spawnY = halfHeight + heightPadding;
        }
        // 아래에서 소환
        else
        {
            // 중간 패딩 사이즈
            float heightPadding = UnityEngine.Random.Range(_heightPaddingMin, _heightPaddingMax);
            
            // 장애물의 길이 확인
            SpriteRenderer spriteRenderer = obstacle.GetComponent<SpriteRenderer>();
            float halfHeight = spriteRenderer.bounds.size.y / 2;

            _spawnY = (halfHeight + heightPadding) * dir;
        }*/
        
        obstacle.transform.position = _spawnPositionObstacle;

    }

    private void SetItem()
    {
        
    }
}
