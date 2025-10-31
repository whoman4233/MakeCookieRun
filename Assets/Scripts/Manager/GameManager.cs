using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(GameManager).Name;
                    _instance = obj.AddComponent<GameManager>();
                }
            }

            return _instance;
        }
    }

    private bool isDanger = false;
    [SerializeField] private float dangerPercentage = 0.3f;

    private bool isGameOver = false;


    public int Score;

    [SerializeField] private float initialPlayerHP = 100f;
    public float PlayerMaxHP { get; private set; }
    private float _playerHP;    // PlayerHP의 실제 값을 저장할 비공개 변수
    public float PlayerHP       // 프로퍼티로 변경
    {
        get { return _playerHP; }
        set
        {
            _playerHP = Mathf.Clamp(value, 0, PlayerMaxHP);

            float percentage = (PlayerMaxHP >0) ? (_playerHP / PlayerMaxHP) : 0f;

            EventManager.RequestPlayerHPChange(percentage);

            if (PlayerHP <= dangerPercentage && !isDanger)          // BGM 변경 코드 이동
            {
                isDanger = true;
                EventManager.RequestBgmPlay("DangerTheme");
            }
            else if (PlayerHP > dangerPercentage && isDanger)       // BGM 변경 코드 이동
            {
                isDanger = false;
                EventManager.RequestBgmPlay("GameTheme");
            }
            
            if (_playerHP <= 0 && !isGameOver)
            {
                GameOver();
            }
        }
    }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = this as GameManager;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
        PlayerMaxHP = initialPlayerHP;
        PlayerHP = initialPlayerHP;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1.0f;
        isGameOver = false;
        isDanger = false;
        string sceneName = scene.name;

        if (sceneName == "TitleScene")
        {
            EventManager.RequestBgmPlay("TitleTheme");
            EventManager.RequestUIStateChange(UIState.Title);
        }
        else if (sceneName == "CharacterScene")
        {
            EventManager.RequestUIStateChange(UIState.Character);
        }
        else if (sceneName == "GameScene")
        {
            EventManager.RequestBgmPlay("GameTheme");
            EventManager.RequestUIStateChange(UIState.Play);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "GameScene" || isGameOver || Time.timeScale == 0f)
        {
            return;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // 게임 일시 정지
        EventManager.RequestBgmPause();
        EventManager.RequestUIStateChange(UIState.Pause);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        EventManager. RequestBgmResume();
        EventManager.RequestUIStateChange(UIState.Play);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0f;
        EventManager.RequestBgmPlay("GameOverTheme");
        EventManager.RequestUIStateChange(UIState.GameOver);
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
