using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Slider hpSlider;
    [SerializeField] private Button pauseBtn;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);
        pauseBtn.onClick.AddListener(OnClickPauseBtn);
    }

    private void Start()
    {
        UpdateHPSlider(1);
    }
    public void OnClikPauseBtn()
    {
        Time.timeScale = 1.0f;
        uiManager.ChangeState(UIState.Pause);
    }

    public void UpdateHPSlider(float percentage)
    {
        hpSlider.value = percentage;
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
