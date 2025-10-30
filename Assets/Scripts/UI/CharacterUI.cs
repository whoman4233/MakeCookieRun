using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterUI : BaseUI
{
    [SerializeField] private Button toTitleBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        toTitleBtn.onClick.AddListener(OnClickTotitleBtn);
    }

    public void OnClickTotitleBtn()
    {
        SceneManager.LoadScene("TitleScene");
    }

    protected override UIState GetUIState()
    {
        return UIState.Character;
    }

    // ĳ���� �̹��� ������ � ������� ó���� ���ΰ� ����غ��� ��
}
