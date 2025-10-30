using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

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
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void setPlay()
    {
        ChangeState(UIState.Play);
        Time.timeScale = 1f;
    }

    public void setPause()
    {
        Time.timeScale = 0f; // 게임 일시 정지
        ChangeState(UIState.Pause);
    }

    public void setGameOver()
    {
        Time.timeScale = 0f;
        ChangeState(UIState.GameOver);
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

