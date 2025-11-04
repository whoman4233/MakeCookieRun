using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ObstaclePattern
{
    public List<ObstacleType> obstacles; // 장애물 타입 리스트
    public float xSpacing = 0.5f;          // 소환된 패턴 내 장애물 간 X 간격
}

[Serializable]
public class ObstaclePatternData
{
    public List<ObstaclePattern> singlePatterns;  // 1개 패턴
    public List<ObstaclePattern> doublePatterns;  // 2개 패턴
    public List<ObstaclePattern> triplePatterns;  // 3개 패턴
}
