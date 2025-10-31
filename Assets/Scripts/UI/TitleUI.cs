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
    [SerializeField] private Button settingBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        startBtn.onClick.AddListener(OnClickStartBtn);
        characterBtn.onClick.AddListener(OnClickCharacterBtn);
        exitBtn.onClick.AddListener(OnClickExitBtn);
        settingBtn.onClick.AddListener(OnClickSettingBtn);
    }

    public void OnClickStartBtn()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickExitBtn()
    {
        Application.Quit();
    }

    public void OnClickCharacterBtn()
    {
        SceneManager.LoadScene("CharacterScene");
    }

    public void OnClickSettingBtn()
    {
        uiManager.OpenSettingsPanel();
    }

    protected override UIState GetUIState()
    {
        return UIState.Title;
    }
}
