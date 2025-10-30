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

    // 캐릭터 이미지 변경을 어떤 방식으로 처리할 것인가 고민해봐야 함
}
