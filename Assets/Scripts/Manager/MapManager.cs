using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private ObstaclePool obstaclePool;
    [SerializeField] private TextAsset patternJsonFile; // JSON 파일
    [SerializeField] private Player player;
    
    private ObstaclePatternData patternData;
    private float lastSpawnX = 5f;
    private float patternPadding; // 패턴 간 간격
    private float patternPaddingMin = 5f;
    private float patternPaddingMax = 7f;
    [SerializeField] private float currentPatternPaddingMin = 5f;
    [SerializeField] private float currentPatternPaddingMax = 10f;
    
    [Header("Obstacle Y Location")]
    [SerializeField] private float topObstacleY = 3f;    // 위에서 아래 Y 위치
    [SerializeField] private float bottomObstacleY = -3.3f; // 아래에서 위 Y 위치
    [SerializeField] private float doubleObstacleY = -2f; // 더블점프 Y 위치

    private int topPatternCount = 0;
    private int bottomPatternCount = 0;
    private int patternCountMax = 3;

    private int spawnSpeed;
    
    private void Start()    
    {
        LoadPatterns();
        StartCoroutine(PreSpawnPatterns());
    }
    
    // JSON에서 패턴 로드
    private void LoadPatterns()
    {
        if (patternJsonFile != null)
        {
            patternData = JsonUtility.FromJson<ObstaclePatternData>(patternJsonFile.text);
            Debug.Log("패턴 로드 완료!");
        }
    }
    
    // 지속적인 패턴 생성
    private IEnumerator PreSpawnPatterns()
    {
        for (int i = 0; i < 25; i++)
        {
            SpawnRandomPattern();
            yield return null;
        }
    
        // ✅ 대기 없이 바로 시작
        StartCoroutine(ContinuousSpawn());
    }

    private IEnumerator ContinuousSpawn()
    {
        while (true)
        {
            float speedRatio = player.forwardSpeed / player.maxSpeed;
            float spawnInterval = Mathf.Lerp(2f, 1f, Mathf.Pow(speedRatio, 2f));

            SpawnRandomPattern();
            yield return new WaitForSeconds(spawnInterval);
        }
    }
    
    // 랜덤 패턴 생성
    private void SpawnRandomPattern()
    {
        // 1개, 2개, 3개 패턴 중 랜덤 선택
        int patternCount = Random.Range(0, 3);
        ObstaclePattern pattern = GetRandomPattern();
        
        if (pattern != null)
        {
            SpawnPattern(pattern);
        }
    }
    
    // 패턴 생성
    private void SpawnPattern(ObstaclePattern pattern)
    {
        float speedRatio = Mathf.Clamp01(player.forwardSpeed / player.maxSpeed);
        
        currentPatternPaddingMin = patternPaddingMin + 15f * speedRatio;
        currentPatternPaddingMax = patternPaddingMax + 15f * speedRatio;
        
        patternPadding = Random.Range(currentPatternPaddingMin, currentPatternPaddingMax); 
        float currentX = lastSpawnX + patternPadding;
        
        foreach (ObstacleType type in pattern.obstacles)
        {
            GameObject obstacle = obstaclePool.GetObstacle(type);
            
            // 타입에 따라 Y 위치 결정
            float yPos = 0f;
            switch (type)
            {
                case ObstacleType.Top:
                    yPos = topObstacleY;
                    break;
                case ObstacleType.Bottom:
                    yPos = bottomObstacleY;
                    break;
                case ObstacleType.Double:
                    yPos = doubleObstacleY;
                    break;
            }
            
            SpriteRenderer renderer = obstacle.GetComponent<SpriteRenderer>();
            float width = renderer.bounds.size.x;
            
            obstacle.transform.position = new Vector3(currentX, yPos, 0f);
            currentX += width + pattern.xSpacing;
        }
        
        lastSpawnX = currentX;
    }
    
    private ObstaclePattern GetRandomPattern()
    {
        // 1. 패턴 개수 선택 (1개/2개/3개 중 랜덤)
        int patternType = Random.Range(0, 3);
        List<ObstaclePattern> patterns = null;
    
        switch (patternType)
        {
            case 0:
                patterns = patternData.singlePatterns;
                break;
            case 1:
                patterns = patternData.doublePatterns;
                break;
            case 2:
                patterns = patternData.triplePatterns;
                break;
        }
    
        // 2. 위/아래 방향 선택 (50% 확률)
        bool useTopPattern;

        if (topPatternCount >= patternCountMax)
        {
            useTopPattern = false;
        }
        else if (bottomPatternCount >= patternCountMax)
        {
            useTopPattern = true;
        }
        else
        {
            useTopPattern = Random.Range(0, 2) == 0;
        }
    
        // 3. 선택된 방향에 맞는 패턴만 필터링
        List<ObstaclePattern> validPatterns = new List<ObstaclePattern>();
    
        foreach (ObstaclePattern pattern in patterns)
        {
            bool isValid = true;
        
            foreach (ObstacleType obstacle in pattern.obstacles)
            {
                if (useTopPattern)
                {
                    // Top 패턴을 원하는데 Top이 아니면 탈락
                    if (obstacle != ObstacleType.Top)
                    {
                        isValid = false;
                        break;
                    }
                }
                else
                {
                    // Bottom 패턴을 원하는데 Top이 있으면 탈락
                    if (obstacle == ObstacleType.Top)
                    {
                        isValid = false;
                        break;
                    }
                }
            }
        
            if (isValid)
            {
                validPatterns.Add(pattern);
            }
        }
    
        // 4. 필터링된 패턴에서 랜덤 선택
        ObstaclePattern selectedPattern = null;
        
        if (validPatterns.Count > 0)
        {
            int randomIndex = Random.Range(0, validPatterns.Count);
            selectedPattern = validPatterns[randomIndex];
        }
        else
        {
            // 해당 방향의 패턴이 없으면 아무거나
            Debug.LogWarning($"{(useTopPattern ? "Top" : "Bottom")} 패턴이 없습니다!");
            selectedPattern = patterns[Random.Range(0, patterns.Count)];
        }

        if (useTopPattern)
        {
            topPatternCount++;
            bottomPatternCount = 0;
        }
        else
        {
            bottomPatternCount++;
            topPatternCount = 0;
        }

        return selectedPattern;
    }
}