using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : BaseUI
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button toTitleBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        restartBtn.onClick.AddListener(OnClickRestartBtn);
        toTitleBtn.onClick.AddListener(OnClickTotitleBtn);
    }

    public void OnClickRestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickTotitleBtn()
    {
        //SceneManager.LoadScene(TitleScene.SceneName);
    }

    protected override UIState GetUIState()
    {
        return UIState.GameOver;
    }
}
