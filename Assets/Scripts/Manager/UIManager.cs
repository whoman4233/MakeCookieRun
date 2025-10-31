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

    private Player currentPlayer;

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

            EventManager.OnUIStateChangeRequested += ChangeState; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        EventManager.OnUIStateChangeRequested -= ChangeState;
    }

    public void ChangeState(UIState state)
    {
        titleUI.SetActive(state);
        characterUI.SetActive(state);
        playUI.SetActive(state);
        pauseUI.SetActive(state);
        gameOverUI.SetActive(state);

        if (state == UIState.Play)
        {
            currentPlayer = FindObjectOfType<Player>();
            if (currentPlayer != null)
            {
                Debug.LogWarning("UIManager: GameScene에서 Player를 찾을 수 없음");
            }
            else
            {
                currentPlayer = null;
            }
        }
    }

    public void OnJumpBtnClick()
    {
        if (currentPlayer != null)
        {
            currentPlayer.TryJump();
        }
    }

    public void OnSlideBtnPress()
    {
        if (currentPlayer != null)
        {
            currentPlayer.BeginSlide();
        }
    }

    public void OnSlideBtnRelease()
    {
        if (currentPlayer != null)
        {
            currentPlayer.EndSlide();
        }
    }

    public void UpdateHPSlider(float percentage)
    {
        if (playUI != null)
        {
            playUI.UpdateHPSlider(percentage);
        }
    }
}

