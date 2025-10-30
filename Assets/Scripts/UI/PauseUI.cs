using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class PauseUI : BaseUI
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button replayBtn;
    [SerializeField] private Button quitBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        restartBtn.onClick.AddListener(OnClikRestartBtn);
        replayBtn.onClick.AddListener(OnClikReplayBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
    }

    public void OnClikRestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClikReplayBtn()
    {
        Time.timeScale = 1.0f;
        uiManager.ChangeState(UIState.Play);
    }

    public void OnClickQuitBtn()
    {
        //SceneManager.LoadScene(TitleScene.SceneName);
    }

    protected override UIState GetUIState()
    {
        return UIState.Pause;
    }
}
