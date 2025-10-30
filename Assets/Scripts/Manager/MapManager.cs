using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Items items;        // Inspector에서 할당
    public Obstacles obstacles;

    void Awake()
    {
        
    }
}
