using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Assets.Scripts.Manager;

public enum UIState
{
    Title,
    Character,
    Play,
    Pause,
    GameOver
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    
    TitleUI titleUI;
    CharacterUI characterUI;
    PlayUI playUI;
    PauseUI pauseUI;
    GameOverUI gameOverUI;
    private UIState currentState;

    private bool isDanger = false;
    [SerializeField] private float dangerPercentage = 0.3f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            titleUI = GetComponentInChildren<TitleUI>(true);
            titleUI.Init(this);
            characterUI = GetComponentInChildren<CharacterUI>(true);
            characterUI.Init(this);
            playUI = GetComponentInChildren<PlayUI>(true);
            playUI.Init(this);
            pauseUI = GetComponentInChildren<PauseUI>(true);
            pauseUI.Init(this);
            gameOverUI = GetComponentInChildren<GameOverUI>(true);
            gameOverUI.Init(this);

            ChangeState(UIState.Title); // UIManager 시작 시 기본 UI 상태 설정

            SceneManager.sceneLoaded += OnSceneLoaded;

            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (currentState != UIState.Play)
        {
            isDanger = false;
            return;
        }

        float currentHP = GameManager.Instance.PlayerHP;

        if (currentHP <= dangerPercentage && !isDanger)
        {
            isDanger= true;
            EventManager.RequestBgmPlay("DangerTheme");
        }
        else if (currentHP > dangerPercentage && isDanger)
        {
            isDanger= false;
            EventManager.RequestBgmPlay("GameTheme"); 
        }
    }

    

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1.0f;
        
        string sceneName = scene.name;

        if (sceneName == "TitleScene")
        {
            ChangeState(UIState.Title);
            EventManager.RequestBgmPlay("TitleTheme");
        }
        else if (sceneName == "CharacterScene")
        {
            ChangeState(UIState.Character);
            EventManager.RequestBgmPlay("CharacterTheme");
        }
        else if (sceneName == "GameScene")
        {
            ChangeState(UIState.Play); // GameScene은 Play 상태에서 시작
            EventManager.RequestBgmPlay("GameTheme");
        }
    }

    public void setPlay()
    {
        ChangeState(UIState.Play);
        Time.timeScale = 1f;
        EventManager.RequestBgmResume();
    }

    public void setPause()
    {
        Time.timeScale = 0f; // 게임 일시 정지
        ChangeState(UIState.Pause);
        EventManager.RequestBgmPause();
    }

    public void setGameOver()
    {
        Time.timeScale = 0f;
        ChangeState(UIState.GameOver);
        EventManager.RequestBgmPlay("GameOverTheme");
    }

    public void ChangeState(UIState state)
    {
        currentState = state;
        titleUI.SetActive(currentState);
        characterUI.SetActive(currentState);
        playUI.SetActive(currentState);
        pauseUI.SetActive(currentState);
        gameOverUI.SetActive(currentState);
    }

    public void JumpBtn()
    {
        //playerController.Jump();
    }

    public void SlideBtn()
    {
        //playerController.Slide();
    }

    public void UpdateHPSlider(float percentage)
    {
        if (playUI != null)
        {
            playUI.UpdateHPSlider(percentage);
        }
    }
}

