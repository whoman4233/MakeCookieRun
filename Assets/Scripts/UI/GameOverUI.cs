using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Manager;

public class GameOverUI : BaseUI
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button toTitleBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        restartBtn.onClick.AddListener(OnClickRestartBtn);
        toTitleBtn.onClick.AddListener(OnClickTotitleBtn);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    public void OnClickRestartBtn()
    {
        EventManager.RequestSfxPlay("Button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickTotitleBtn()
    {
        EventManager.RequestSfxPlay("Button");
        SceneManager.LoadScene("TitleScene");
    }

    protected override UIState GetUIState()
    {
        return UIState.GameOver;
    }
}
