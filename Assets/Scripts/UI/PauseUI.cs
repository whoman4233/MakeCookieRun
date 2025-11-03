using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Assets.Scripts.Manager;

public class PauseUI : BaseUI
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button quitBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        restartBtn.onClick.AddListener(OnClickRestartBtn);
        resumeBtn.onClick.AddListener(OnClickResumeBtn);
        quitBtn.onClick.AddListener(OnClickQuitBtn);
    }

    public void OnClickRestartBtn()
    {
        EventManager.RequestSfxPlay("Button");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickResumeBtn()
    {
        EventManager.RequestSfxPlay("Button");
        GameManager.Instance.ResumeGame();
    }

    public void OnClickQuitBtn()
    {
        EventManager.RequestSfxPlay("Button");
        SceneManager.LoadScene("TitleScene");
    }

    protected override UIState GetUIState()
    {
        return UIState.Pause;
    }
}
