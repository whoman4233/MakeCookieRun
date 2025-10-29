using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UIState
{
    Play,
    Pause,
    GameOver
}

public class UIManager : MonoBehaviour
{
    PlayUI playUI;
    PauseUI pauseUI;
    GameOverUI gameOverUI;
    private UIState currentState;

    private void Awake()
    {
        playUI = GetComponentInChildren<PlayUI>(true);
        playUI.Init(this);
        pauseUI = GetComponentInChildren<PauseUI>(true);
        pauseUI.Init(this);
        gameOverUI = GetComponentInChildren<GameOverUI>(true);
        gameOverUI.Init(this);
    }

    void ToCharacterScene()
    {
        SceneManager.LoadScene(CharacterScene.sceneName);
    }

    void ToGameScene()
    {
        SceneManager.LoadScene(GameScene.sceneName);
        setPlay();
    }
    void ExitGame()
    {
        Application.Quit();
    }

    private void setPlay()
    {
        Time.timeScale = 1f;
        ChangeState(UIState.Play);
    }

    private void setPause()
    {
        Time.timeScale = 0f; // 게임 일시 정지
        ChangeState(UIState.Pause);
    }

    private void setGameOver()
    {
        Time.timeScale = 0f;
        ChangeState(UIState.GameOver);
    }

    public void ChangeState(UIState state)
    {
        currentState = state;
        playUI.SetActive(currentState);
        pauseUI.SetActive(currentState);
        gameOverUI.SetActive(currentState);
    }


    void JumpBtn()
    {
        //playerController.Jump();
    }

    void SlideBtn()
    {
        //playerController.Slide();
    }
}

