using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Manager;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button closeBtn;

    private UIManager uiManager;

    public void Init(UIManager manager)
    {
        this.uiManager = manager;

        closeBtn.onClick.AddListener(OnClickCloseBtn);
        bgmSlider.onValueChanged.AddListener(OnBgmVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSfxVolumeChanged);
    }

    private void OnEnable()
    {
        if (SoundManager.instance != null)
        {
            bgmSlider.value = SoundManager.instance.bgmAudioSource.volume;
            sfxSlider.value = SoundManager.instance.sfxAudioSource.volume;
        }
    }

    private void OnClickCloseBtn()
    {
        EventManager.RequestSfxPlay("Button");
        uiManager.CloseSettingsPanel();
    }

    private void OnBgmVolumeChanged(float value)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.SetBgmVolume(value);
        }
    }

    private void OnSfxVolumeChanged(float value)
    {
        if (SoundManager.instance != null)
        {
            SoundManager.instance.SetSfxVolume(value);
        }
    }
}