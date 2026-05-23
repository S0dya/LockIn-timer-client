using System;
using App.Timer;
using App.Timer.Login;
using App.Timer.Run;
using App.Timer.Settings;
using App.Timer.States;
using App.Timer.Windows;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using UniRx;
using Zenject;

namespace App
{
    public class AppSynchronizationService : IInitializable, IDisposable
    {
        [Inject] private AppState _appState;
        [Inject] private AuthViewModel _authViewModel;
        [Inject] private RequestLoadingManager _requestLoadingManager;
        [Inject] private TimerSettingsViewModel _timerSettingsViewModel;
        [Inject] private RunViewModel _runViewModel;
        [Inject] private AppConfig _appConfig;

        private readonly CompositeDisposable _disposables = new();
        private IDisposable _autoSyncDisposable;

        private DateTime _lastAvailableManualSyncStamp;

        public void Initialize()
        {
            _appState.CurrentUser
                .Skip(1)
                .Subscribe(OnCurrentUserChanged)
                .AddTo(_disposables);
        }

        private void OnCurrentUserChanged(UserState userState)
        {
            if (userState == null) return;
            
            SynchronizeTimer().Forget();
        }

        public void FocusSynchronize()
        {
            if ((DateTime.UtcNow - _lastAvailableManualSyncStamp).TotalSeconds < _appConfig.ManualSyncOffsetSeconds) return;
            _lastAvailableManualSyncStamp = DateTime.UtcNow;
            
            SynchronizeTimer().Forget();
        }

        public async UniTask Synchronize()
        {
            DebugManager.Log(DebugCategory.Points, $"Synchronization started");
            
            try
            {
                using var loadingToken = _requestLoadingManager.AddLoading();

                ResetSync();
                
                await _authViewModel.FetchCurrentUser();
            }
            catch
            {
                DebugManager.Log(DebugCategory.Points, $"Synchronization failed");
            }
        }

        private async UniTask SynchronizeTimer()
        {
            try
            {
                using var loadingToken = _requestLoadingManager.AddLoading();
                
                ResetSync();
                
                await LoadStates();
            }
            catch
            {
                DebugManager.Log(DebugCategory.Points, $"Synchronization of timer failed");
            }
        }

        private async UniTask LoadStates()
        {
            if (_appState.CurrentUser.Value != null)
            {
                await _timerSettingsViewModel.Init();
                await _runViewModel.Init();
            }
        }

        private void ResetSync()
        {
            _lastAvailableManualSyncStamp = DateTime.UtcNow;
            
            StopAutoSync();
            ScheduleNextSync();
        }

        private void ScheduleNextSync()
        {
            _autoSyncDisposable = Observable
                .Timer(TimeSpan.FromSeconds(_appConfig.AutoSyncIntervalSeconds))
                .Subscribe(_ =>
                {
                    if (_appState.CurrentUser.Value != null)
                    {
                        DebugManager.Log(DebugCategory.Misc, "Auto-sync triggered");
                        SynchronizeTimer().Forget();
                    }
                });
        }
        private void StopAutoSync()
        {
            _autoSyncDisposable?.Dispose();
            _autoSyncDisposable = null;
        }

        public void Dispose()
        {
            StopAutoSync();
            _disposables?.Dispose();
        }
    }
}