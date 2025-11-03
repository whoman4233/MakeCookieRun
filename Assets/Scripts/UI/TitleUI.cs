using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Assets.Scripts.Manager;

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
        EventManager.RequestSfxPlay("Button");
        SceneManager.LoadScene("GameScene");
    }

    public void OnClickExitBtn()
    {
        EventManager.RequestSfxPlay("Button");
        Application.Quit();
    }

    public void OnClickCharacterBtn()
    {
        EventManager.RequestSfxPlay("Button");
        SceneManager.LoadScene("CharacterScene");
    }

    public void OnClickSettingBtn()
    {
        EventManager.RequestSfxPlay("Button");
        uiManager.OpenSettingsPanel();
    }

    protected override UIState GetUIState()
    {
        return UIState.Title;
    }
}
