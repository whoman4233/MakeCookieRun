using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class PauseUI : BaseUI
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button quitBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        restartBtn.onClick.AddListener(OnClikRestartBtn);
        resumeBtn.onClick.AddListener(OnClikResumeBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
    }

    public void OnClikRestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClikResumeBtn()
    {
        uiManager.setPlay();
    }

    public void OnClickQuitBtn()
    {
        SceneManager.LoadScene("TitleScene");
    }

    protected override UIState GetUIState()
    {
        return UIState.Pause;
    }
}
