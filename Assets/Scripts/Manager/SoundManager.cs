using Assets.Scripts.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance {  get; private set; }

    [SerializeField] private AudioSource bgmAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [SerializeField] private Sound[] bgmClips;
    [SerializeField] private Sound[] sfxClips;

    private Dictionary<string, AudioClip> bgmDictionary;
    private Dictionary<string , AudioClip> sfxDictionary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        bgmDictionary = new Dictionary<string , AudioClip>();
        foreach (Sound sound in bgmClips)
        {
            bgmDictionary[sound.name] = sound.clip;
        }

        sfxDictionary = new Dictionary<string , AudioClip>();
        foreach (Sound sound in sfxClips)
        {
            sfxDictionary[sound.name] = sound.clip;
        }

        if (bgmAudioSource != null)
        {
            bgmAudioSource.loop = true;
        }
    }

    private void OnEnable()
    {
        EventManager.OnSfxPlayRequested += PlaySfx;

        EventManager.OnBgmPlayRequested += PlayBgm;
        EventManager.OnBgmStopRequested += StopBgm;
        EventManager.OnBgmPauseRequested += PauseBgm;
        EventManager.OnBgmResumeRequested += ResumeBgm;
    }

    private void OnDisable()
    {
        EventManager.OnSfxPlayRequested -= PlaySfx;

        EventManager.OnBgmPlayRequested -= PlayBgm;
        EventManager.OnBgmStopRequested -= StopBgm;
        EventManager.OnBgmPauseRequested -= PauseBgm;
        EventManager.OnBgmResumeRequested -= ResumeBgm;
    }

    private void PlaySfx(string sfxName)
    {
        if (sfxDictionary.TryGetValue(sfxName, out AudioClip clip))
        {
            sfxAudioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log($"SoundManager: SFX '{sfxName}'를 찾을 수 없습니다.");
        }
    }

    private void PlayBgm(string bgmName)
    {
        if (bgmDictionary.TryGetValue(bgmName, out AudioClip clip))
        {
            // 현재 재생 중인 BGM과 같은 곡이 아니면 변경
            if (bgmAudioSource.clip == clip && bgmAudioSource.isPlaying)
            {
                return;
            }

            bgmAudioSource.clip = clip;
            bgmAudioSource.Play();
        }
        else
        {
            Debug.Log($"SoundManager: BGM '{bgmName}'를 찾을 수 없습니다.");
        }
    }

    private void StopBgm()
    {
        bgmAudioSource.Stop();
        bgmAudioSource.clip = null;
    }

    private void PauseBgm()
    {
        if (bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Pause();
        }
    }

    private void ResumeBgm()
    {
        if (!bgmAudioSource.isPlaying && bgmAudioSource.clip != null)
        {
            bgmAudioSource.UnPause();
        }
    }

    public void SetSfxVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }

    public void SetBgmVolume(float volume)
    {
        bgmAudioSource.volume = volume;
    }
}
