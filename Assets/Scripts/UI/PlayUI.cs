using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image hpSlider;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button jumpBtn;
    [SerializeField] private HoldableButton slideBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        pauseBtn.onClick.AddListener(OnClickPauseBtn);

        if (jumpBtn != null)
        {
            jumpBtn.onClick.AddListener(UIManager.instance.OnJumpBtnClick);
        }

        if (slideBtn != null)
        {
            slideBtn.onPress.AddListener(UIManager.instance.OnSlideBtnPress);
            slideBtn.onRelease.AddListener(UIManager.instance.OnSlideBtnRelease);
        }
    }

    private void Start()
    {
        UpdateHPSlider(1);
    }
    public void OnClickPauseBtn()
    {
        GameManager.Instance.PauseGame();
    }

    public void UpdateHPSlider(float percentage)
    {
        hpSlider.fillAmount = percentage;
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = score.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.Play;
    }
}
