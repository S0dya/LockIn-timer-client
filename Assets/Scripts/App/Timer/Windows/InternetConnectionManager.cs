using System;
using System.Threading;
using App.Timer.Back.Config;
using App.Timer.States;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Http;
using PT.Tools.Windows;
using UniRx;
using UnityEngine;
using Zenject;

namespace App.Timer.Windows
{
    public class InternetConnectionManager : IInitializable
    {
        [Inject] private InternetState _internetState;
        [Inject] private AppSynchronizationService _syncService;
        [Inject] private InternetConnectionWindow _internetConnectionWindow;
        [Inject] private IHttpClient _httpClient;
        [Inject] private ApiConfig _apiConfig;
        [Inject] private HttpClientConfig _httpClientConfig;
        [Inject(Id = "App")] private WindowsManager _windowsManager;

        private readonly CompositeDisposable _disposables = new();
        
        private IDisposable _retryDisposable;
        private bool _hasCompletedInitialSync;
        private bool _wasDisconnected;
        private DateTime _lastManualRetryTime = DateTime.MinValue;

        public void Initialize()
        {
            _internetState.IsConnected
                .Skip(1)
                .DistinctUntilChanged()
                .Subscribe(OnInternetChanged)
                .AddTo(_disposables);

            _internetConnectionWindow.OnRetry += TryRetryConnection;
        }
        
        private void OnInternetChanged(bool connected)
        {
            DebugManager.Log(DebugCategory.Internet, $"Internet connection changed: {connected}");
            
            if (!connected)
            {
                _wasDisconnected = true;
                DebugManager.Log(DebugCategory.Internet, "Internet disconnected - opening connection window");

                _windowsManager.Open<InternetConnectionWindow>().Forget();
                StartAutoRetry();

                return;
            }

            StopAutoRetry();
            
            if (_windowsManager.IsOpen<InternetConnectionWindow>())
            {
                DebugManager.Log(DebugCategory.Internet, "Internet reconnected - closing connection window");
                _windowsManager.Close<InternetConnectionWindow>().Forget();
            }
            
            if (_wasDisconnected)
            {
                _wasDisconnected = false;
                DebugManager.Log(DebugCategory.Internet, "Restoring connection after disconnection");

                RestoreConnection().Forget();
            }
        }

        private async UniTaskVoid RestoreConnection()
        {
            try
            {
                await _syncService.Synchronize();
                DebugManager.Log(DebugCategory.Internet, "Connection restoration completed successfully");
            }
            catch (System.Exception ex)
            {
                DebugManager.Log(DebugCategory.Internet, $"Connection restoration failed: {ex.Message}", LogType.Error);
            }
        }

        private async void TryRetryConnection()
        {
            if (!_windowsManager.IsOpen<InternetConnectionWindow>() || _internetState.IsConnected.Value) return;
            var timeSinceLastRetry = DateTime.Now - _lastManualRetryTime;
            if (timeSinceLastRetry.TotalSeconds < _httpClientConfig.ManualRetryCooldownSeconds) return;
            
            DebugManager.Log(DebugCategory.Internet, "Trying reconnection");
            
            try
            {
                _lastManualRetryTime = DateTime.Now;
                var cts = new CancellationTokenSource();
                await _httpClient.Get<object>(_apiConfig.CheckInternet, cts.Token);
            }
            catch
            {
                DebugManager.Log(DebugCategory.Internet, "Manual retry failed");
            }
        }

        private void StartAutoRetry()
        {
            StopAutoRetry();
            
            _retryDisposable = Observable
                .Interval(TimeSpan.FromSeconds(_httpClientConfig.RetryIntervalSeconds))
                .Subscribe(_ =>
                {
                    if (!_internetState.IsConnected.Value)
                    {
                        TryRetryConnection();
                    }
                });
        }

        private void StopAutoRetry()
        {
            _retryDisposable?.Dispose();
            _retryDisposable = null;
            _lastManualRetryTime = DateTime.MinValue;
        }

        public void Dispose()
        {
            StopAutoRetry();
            _disposables.Dispose();
            
            _internetConnectionWindow.OnRetry -= TryRetryConnection;
        }
    }
}