using System;
using System.Linq;
using System.Threading;
using App.Timer.Back.Models;
using App.Timer.Back.Services;
using App.Timer.Run;
using App.Timer.States;
using App.Timer.Windows;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Windows;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.Timer.Settings
{
    public class TimerSettingsViewModel : IInitializable, IDisposable
    {
        [Inject] private AppState _appState;
        [Inject] private RequestLoadingManager _requestLoadingManager;
        [Inject] private RequestErrorManager _requestErrorManager;
        [Inject] private AppConfig _appConfig;
        [Inject] private TimerSettingsService _timerSettingsService;
        [Inject] private TimerSettingsViewWindow _timerSettingsViewWindow;
        [Inject] private RunActionsView _runActionsView;
        [Inject(Id = "App")] private WindowsManager _windowsManager;

        private int _currentDurationIndex;
        private int _currentSessionsAmountIndex;

        private CancellationTokenSource _cts = new();
        private CompositeDisposable _disposables = new();

        public void Initialize()
        {
            _runActionsView.OnTimerSettings += OnTimerSettingsPressed;
            _timerSettingsViewWindow.OnAcceptSettingsPressed += OnAcceptSettingsPressed;
            _timerSettingsViewWindow.OnClosePressed += OnClosePressed;
            
            _appState.TimerSettingsState.Value = new TimerSettingsState()
            {
                SessionDuration = _appConfig.DefaultTimerSettingsSessionDuration,
                SessionsAmount = _appConfig.DefaultTimerSettingsSessionsAmount
            };
        }

        public async UniTask Init()
        {
            await FetchTimerSettings();
        }

        private void OnTimerSettingsPressed()
        {
            if (_windowsManager.IsOpen<TimerSettingsViewWindow>()) return;
            
            OpenTimerSettings().Forget();
        }
        private void OnAcceptSettingsPressed(TimerSettingsData settingsData)
        {
            ApproveNewSettings(settingsData).Forget(); //myb await for error case?
            
            OnClosePressed();
        }
        private void OnClosePressed()
        {
            if (!_windowsManager.IsOpen<TimerSettingsViewWindow>()) return;
            
            _windowsManager.Close<TimerSettingsViewWindow>().Forget();
        }

        private async UniTask FetchTimerSettings()
        {
            DebugManager.Log(DebugCategory.TimerSettings, "Fetching timer settings");
            try
            {
                var result = await _timerSettingsService.GetTimerSettings(_cts.Token);
                
                if (!result.IsSuccess)
                {
                    DebugManager.Log(DebugCategory.TimerSettings, "Fetch timer settings failed");
                    _requestErrorManager.ShowError(result.Error);
                }
                else
                {
                    DebugManager.Log(DebugCategory.TimerSettings, "Fetch timer settings successful");
                }

                SetSettingsResponse(result.Value);
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.TimerSettings, $"Fetch timer settings exception: {e.Message}");
                _requestErrorManager.ShowError(e.Message);
            }
        }
        
        private async UniTask OpenTimerSettings()
        {
            DebugManager.Log(DebugCategory.TimerSettings, "Preparing to open timer settings window");
            try
            {
                using var loading = _requestLoadingManager.AddLoading();
                
                await FetchTimerSettings();

                var durationsIndex = GetSettingIndex(_appConfig.TimerSettingsDurations, 
                    _appState.TimerSettingsState.Value.SessionDuration);
                var sessionsAmountIndex = GetSettingIndex(_appConfig.TimerSettingsSessionsAmounts, 
                    _appState.TimerSettingsState.Value.SessionsAmount);
                var settingsData = new TimerSettingsData(durationsIndex, sessionsAmountIndex);
                
                DebugManager.Log(DebugCategory.TimerSettings, "Updating the TimerView");
                _timerSettingsViewWindow.UpdateView(_appState.RunState.Value.RunStatus);
                _windowsManager.Open<TimerSettingsViewWindow>(settingsData).Forget();
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, $"Open timer settings failed: {e.Message}");
                _requestErrorManager.ShowError(e.Message);
            }

            int GetSettingIndex(int[] arr, int value)
            {
                if (arr.Length == 0)
                {
                    DebugManager.Log(DebugCategory.TimerSettings, "Settings array is empty", LogType.Error); return -1;
                }
                
                var closest = arr.OrderBy(x => Math.Abs(x - value)).First();

                if (Math.Abs(closest - value) > _appConfig.SettingsTolerance)
                {
                    DebugManager.Log(DebugCategory.TimerSettings, $"Closest value {closest} is far from {value}", LogType.Warning);
                }
                
                return Array.IndexOf(arr, closest);
            }
        }

        private async UniTask ApproveNewSettings(TimerSettingsData settingsData)
        {
            if (_appConfig.TimerSettingsDurations.Length < settingsData.DurationIndex
                || settingsData.DurationIndex < 0)
            {
                DebugManager.Log(DebugCategory.TimerSettings, "Invalid duration index", LogType.Error); return;
            }

            if (_appConfig.TimerSettingsDurations[settingsData.DurationIndex] == _appState.TimerSettingsState.Value.SessionDuration &&
                _appConfig.TimerSettingsSessionsAmounts[settingsData.SessionsAmountIndex] == _appState.TimerSettingsState.Value.SessionsAmount)
            {
                DebugManager.Log(DebugCategory.TimerSettings, "Settings unchanged, skipping update"); return;
            }

            DebugManager.Log(DebugCategory.TimerSettings, "Applying new timer settings");

            try
            {
                using var loadingToken = _requestLoadingManager.AddLoading();
                
                var result = await _timerSettingsService.SetTimerSettings(new SettingsRequest()
                {
                    SessionDuration = _appConfig.TimerSettingsDurations[settingsData.DurationIndex],
                    SessionsAmount = _appConfig.TimerSettingsSessionsAmounts[settingsData.SessionsAmountIndex]
                }, _cts.Token);

                if (!result.IsSuccess)
                {
                    _requestErrorManager.ShowError(result.Error); return;
                }
                    
                SetSettingsResponse(result.Value);
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, $"Apply settings exception: {e.Message}");
                _requestErrorManager.ShowError(e.Message);
            }
        }

        private void SetSettingsResponse(SettingsResponse response)
        {
            var newState = new TimerSettingsState()
            {
                SessionDuration = response.SessionDuration,
                SessionsAmount = response.SessionsAmount
            };

            if (_appState.TimerSettingsState.Value != null && _appState.TimerSettingsState.Value.Equals(newState)) return;

            DebugManager.Log(DebugCategory.TimerSettings, $"Settings updated: Duration={response.SessionDuration}, Sessions={response.SessionsAmount}");
            _appState.TimerSettingsState.Value = newState;
        }
        
        public void CancelAllRequests()
        {
            _cts.Cancel();
        }
        public void Dispose()
        {
            _cts?.Dispose();
            
            _runActionsView.OnTimerSettings -= OnTimerSettingsPressed;
            _timerSettingsViewWindow.OnAcceptSettingsPressed -= OnAcceptSettingsPressed;
            _timerSettingsViewWindow.OnClosePressed -= OnClosePressed;
        }
    }
}
