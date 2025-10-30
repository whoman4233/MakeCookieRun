using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TitleUI : BaseUI
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button characterBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        startBtn.onClick.AddListener(OnClickStartBtn);
        characterBtn.onClick.AddListener(OnClickCharacterBtn);
        exitBtn.onClick.AddListener(OnClickExitBtn);
    }

    public void OnClickStartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickCharacterBtn()
    {
        SceneManager.LoadScene("CharacterScene");
    }

    public void OnClickExitBtn()
    {
        Application.Quit();
    }

    protected override UIState GetUIState()
    {
        return UIState.Title;
    }
}
