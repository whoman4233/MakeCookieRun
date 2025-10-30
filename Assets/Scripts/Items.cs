using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Items : MonoBehaviour
{
    // [SerializeField] private GameObject _scoreItemPrefabs;
    // [SerializeField] private GameObject _hpItemPrefabs;

    private SpriteRenderer _spriteRenderers;

    private float _spawnItemYMin = 1f;
    private float _spawnItemYMax = 3f;

    private float _widthPaddingItem;
    private float _widthPaddingMinItem = 1f;
    private float _widthPaddingMaxItem = 2f;

    private float _spawnItemX;                              // 장애물 스폰 X 좌표
    private float _spawnItemY;
    private Vector3 _spawnPositionItem;
    // private List<Item> items = new List<Item>();

    // public enum ItemType { Score, Hp }*

    /*public struct Item
    {
        public GameObject itemObject;
        public ItemType type;
        public float value;

        public Item(GameObject obj, ItemType t, int v)
        {
            itemObject = obj;
            type = t;
            value = v;
        }
    }*/

    // 아이템 랜덤 생성
    public Vector3 SetItemPosition(Vector3 itemLastPosition)
    {
        // Item item = items[Random.Range(0, _itemPrefabs.Length)];

        // y 축
        _spriteRenderers = gameObject.GetComponent<SpriteRenderer>();
        _spawnItemY = Random.Range(_spawnItemYMin, _spawnItemYMax);

        float itemHalfHeight = _spriteRenderers.bounds.size.x;

        _widthPaddingItem = Random.Range(_widthPaddingMinItem, _widthPaddingMaxItem);
        _spawnItemX = itemLastPosition.x + itemHalfHeight + Random.Range(_widthPaddingItem, _widthPaddingMaxItem);

        _spawnPositionItem = new Vector3(_spawnItemX, _spawnItemY, 0);

        return _spawnPositionItem;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player == null) return;

        if (collision.gameObject.name.Contains("Score"))
        {
            GameManager.Instance.Score += 100;
        }
        else if (collision.gameObject.name.Contains("Hp"))
        {
            GameManager.Instance.PlayerHP += 10;
        }

        Destroy(collision.gameObject);
    }

    // 장애물과 충돌 시
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        Obstacles obstacle = collision.gameObject.GetComponent<Obstacles>();
        if (obstacle != null) return;

        float newSpawnY = obstacle.transform.position.y + gameObject.transform.position.y;
        
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, newSpawnY, gameObject.transform.position.z);
    }*/
}
