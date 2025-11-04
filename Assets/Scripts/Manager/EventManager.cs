using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public static class EventManager
    {
        // UI 변경 관련 이벤트
        public static event Action<UIState> OnUIStateChangeRequested;

        // Sound 관련 이벤트
        public static event Action<string> OnSfxPlayRequested;
        public static event Action<string> OnBgmPlayRequested;
        public static event Action OnBgmStopRequested;
        public static event Action OnBgmPauseRequested;
        public static event Action OnBgmResumeRequested;

        // 체력, 점수 변동 관련 이벤트
        public static event Action<float> OnPlayerHPChanged;
        public static event Action<int> OnScoreChanged;

        public static void RequestUIStateChange(UIState state)
        {
            OnUIStateChangeRequested?.Invoke(state);
        }

        public static void RequestSfxPlay(string soundName)
        {
            OnSfxPlayRequested?.Invoke(soundName);
        }

        public static void RequestBgmPlay(string bgmName)
        {
            OnBgmPlayRequested?.Invoke(bgmName);
        }

        public static void RequestBgmStop()
        {
            OnBgmStopRequested?.Invoke();
        }

        public static void RequestBgmPause()
        {
            OnBgmPauseRequested?.Invoke();
        }

        public static void RequestBgmResume()
        {
            OnBgmResumeRequested?.Invoke();
        }

        public static void RequestPlayerHPChange(float percentage)
        {
            OnPlayerHPChanged?.Invoke(percentage);
        }

        public static void RequestScoreChange(int newScore)
        {
            OnScoreChanged?.Invoke(newScore);
        }
    }
}
