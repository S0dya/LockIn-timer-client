using System;
using System.Collections.Generic;
using App.Timer.States;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Helper;
using PT.Tools.Windows;
using PT.UI.Buttons;
using UnityEngine;
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
        [Space]
        [SerializeField] private SerializableKeyValue<RunStatus, GameObject[]> runStatusToView;
        
        public event Action<TimerSettingsData> OnAcceptSettingsPressed;
        public event Action OnClosePressed;

        [Inject] private AppConfig _appConfig;

        public void UpdateView(RunStatus status)
        {
            foreach (var kvp in runStatusToView.Dictionary) kvp.Value.SetActive(false);
                DebugManager.Log(DebugCategory.TimerSettings, "Deactivated");
            
            runStatusToView.Dictionary[status].SetActive(true);
                DebugManager.Log(DebugCategory.TimerSettings, "Activated");
            
        }
        
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