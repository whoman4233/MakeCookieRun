using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public static class EventManager
    {
        public static event Action<UIState> OnUIStateChangeRequested;

        public static event Action<string> OnSfxPlayRequested;

        public static event Action<string> OnBgmPlayRequested;
        public static event Action OnBgmStopRequested;
        public static event Action OnBgmPauseRequested;
        public static event Action OnBgmResumeRequested;

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

    }
}
