using System;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Helper;
using PT.Tools.Windows;
using PT.UI.Buttons;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Timer.Settings
{
    public class TimerSettingsViewWindow : WindowBase
    {
        [SerializeField] private TimerSettingView timerSettingDuration;
        [SerializeField] private TimerSettingView timerSettingSessionsAmount;
        [Space]
        [SerializeField] private BasicButton acceptButton;
        [SerializeField] private BasicButton closeButton;

        //add text to show that it wont update if run is alr going
        
        public event Action<TimerSettingsData> OnAcceptSettingsPressed;
        public event Action OnClosePressed;

        [Inject] private AppConfig _appConfig;

        protected override async UniTask OnOpen()
        {
            await base.OnOpen();

            if (Payload is TimerSettingsData settingsData)
            {
                timerSettingDuration.Init(_appConfig.TimerSettingsDurations, 
                    settingsData.DurationIndex, Utils.ConvertSecondsToTime);
                timerSettingSessionsAmount.Init(_appConfig.TimerSettingsSessionsAmounts,
                    settingsData.SessionsAmountIndex);

                acceptButton.SetOnClick(() => OnAcceptSettingsPressed?.Invoke(
                    new TimerSettingsData(
                        timerSettingDuration.CurrentIndex.Value, 
                        timerSettingSessionsAmount.CurrentIndex.Value)));
                closeButton.SetOnClick(() => OnClosePressed?.Invoke());
            }
            else
                DebugManager.Log(DebugCategory.Backend, "Invalid payload type. Expected TimerSettingsData, got: " + Payload.GetType().Name);
        }
    }
}