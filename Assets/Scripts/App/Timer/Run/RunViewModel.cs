using System;
using System.Threading;
using App.Timer.Back.Models;
using App.Timer.Back.Services;
using App.Timer.Settings;
using App.Timer.States;
using App.Timer.Windows;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Windows;
using UniRx;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace App.Timer.Run
{
    public class RunViewModel : IInitializable, IDisposable
    {
        [Inject] private AppState _appState;
        [Inject] private AppConfig _appConfig;
        [Inject] private RunService _runService;
        [Inject] private RunSubmitViewWindow _runSubmitViewWindow;
        [Inject] private SessionFinishedViewWindow _sessionFinishedViewWindow;
        [Inject] private RunTimerClockView _runTimerClockView;
        [Inject] private RunActionsView _runActionsView;
        [Inject(Id = "Menu")] private WindowsManager _windowsManager;
        [Inject] private RequestLoadingManager _requestLoadingManager;
        [Inject] private RequestErrorManager _requestErrorManager;
        
        private readonly CompositeDisposable _disposables = new();
        private IDisposable _timerDisposable;
        private bool _isOperationInProgress = false;
        
        #region Initialization
        
        public void Initialize()
        {
            SubscribeToAppState();
            SubscribeToWindowEvents();
        }
        
        private void SubscribeToAppState()
        {
            _appState.RunState
                .Subscribe(CurrentRunChanged)
                .AddTo(_disposables);

            _appState.CurrentSessionTime
                .Subscribe(CurrentSessionTimeChanged)
                .AddTo(_disposables);
            
            _appState.TimerSettingsState
                .Subscribe(CurrentTimerSettingsChanged)
                .AddTo(_disposables);
        }
        
        private void SubscribeToWindowEvents()
        {
            _runSubmitViewWindow.OnSubmit += OnRunSubmit;
            _runSubmitViewWindow.OnCloseClicked += OnCloseSubmitRun;
            
            _sessionFinishedViewWindow.OnClosePressed += OnSessionFinishedClose;
            
            _runActionsView.OnStartSession += OnStartSession;
            _runActionsView.OnCancelRun += OnCancelRun;
            _runActionsView.OnCancelSession += OnCancelSession;
            _runActionsView.OnSubmitRun += OnOpenSubmitRun;
        }
        
        #endregion
        
        #region Public Methods

        public async UniTask Init()
        {
            await FetchRun();
        }
        
        #endregion
        
        #region AppState Handlers
        
        private void CurrentRunChanged(RunState run)
        {
            if (run == null) return;

            _runActionsView.UpdateView(run);
            _runTimerClockView.UpdateView(run.RunStatus);
        }

        private void CurrentSessionTimeChanged(int currentTime)
        {
            var run = _appState.RunState.Value;
            if (run == null) return;

            _runTimerClockView.SetTimer(currentTime, run.SessionDuration, run.RunStatus == RunStatus.Active);
        }
        
        private void CurrentTimerSettingsChanged(TimerSettingsState timerSettings)
        {
            if (_appState.RunState.Value == null) return;

            if (_appState.RunState.Value.RunStatus == RunStatus.None)
            {
                DebugManager.Log(DebugCategory.Backend, "Settings changed while run is not active so fetching run");
                
                FetchRun().Forget();
            }
        }
        
        #endregion
        private void OnOpenSubmitRun()
        {
            _windowsManager.Open<RunSubmitViewWindow>().Forget();
        }
        #region Window Event Handlers
        
        private void OnRunSubmit(string description)
        {
            _runService.FinishRun(new RunFinishRequest()
            {
                RunDescription = description,
            }).Forget();
            
            FetchRun().Forget();
            
            OnCloseSubmitRun();
        }
        private void OnCloseSubmitRun()
        {
            _windowsManager.Close<RunSubmitViewWindow>().Forget();
        }
        
        private void OnSessionFinishedClose()
        {
            _windowsManager.Close<SessionFinishedViewWindow>().Forget();
            FetchRun().Forget();
        }
        
        #endregion
        
        #region Button Event Handlers
        
        private void OnStartSession()
        {
            TryStartSession();
        }
        
        private void OnCancelRun()
        {
            TryCancelRun();
        }
        private void OnCancelSession()
        {
            TryCancelSession();
        }
        
        #endregion
        
        #region Session Management
        
        private async UniTask FetchRun()
        {
            DebugManager.Log(DebugCategory.Backend, "Starting FetchRun operation");
            
            using var cts = new CancellationTokenSource();
            try
            {
                var result = await _runService.GetCurrentRun(cts.Token);
                DebugManager.Log(DebugCategory.Backend, $"FetchRun completed successfully. Status: {(result.IsSuccess)}");

                if (!result.IsSuccess || result.Value == null)
                {
                    DebugManager.Log(DebugCategory.Backend, $"No active run fetched, creating empty");

                    var emptyRun = new RunState()
                    {
                        RunStatus = RunStatus.None,
                        SessionDuration = _appState.TimerSettingsState.Value.SessionDuration,
                        PlannedSessionsAmount = _appState.TimerSettingsState.Value.SessionsAmount,
                        PlannedSessionsAmountCompletedSessions = 0
                    };

                    if (_appState.RunState.Value != null && _appState.RunState.Value.Equals(emptyRun)) return;
                        
                    _appState.RunState.Value = emptyRun; 

                    return;
                }

                var runResponse = result.Value;
                var status = runResponse.CurrentSessionStartTime != null ? RunStatus.Active : RunStatus.Idle;
                var currentTime = runResponse.CurrentSessionStartTime == null ? runResponse.SessionDuration
                    : (int)Math.Max(0, runResponse.SessionDuration - (DateTime.UtcNow - runResponse.CurrentSessionStartTime.Value).TotalSeconds);

                var newRun = new RunState()
                {
                    RunStatus = status,
                    SessionDuration = runResponse.SessionDuration,
                    PlannedSessionsAmount = runResponse.PlannedSessionsAmount,
                    PlannedSessionsAmountCompletedSessions = runResponse.CompletedSessions
                };

                if (_appState.RunState.Value != null && _appState.RunState.Value.Equals(newRun)) return;
                
                _appState.RunState.Value = newRun;
                _appState.CurrentSessionTime.Value = currentTime;

                if (status == RunStatus.Active) StartTimerTick();
                else StopTimerTick();
                
                DebugManager.Log(DebugCategory.Backend, "FetchRun state updated successfully");
            }
            catch (OperationCanceledException)
            {
                DebugManager.Log(DebugCategory.Backend, "FetchRun operation was cancelled");
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, $"Failed to fetch run: {e.Message}");
            }
        }

        private async void TryStartSession()
        {
            if (_isOperationInProgress)
            {
                DebugManager.Log(DebugCategory.Backend, "TryStartSession ignored - operation already in progress"); return;
            }
            
            DebugManager.Log(DebugCategory.Backend, "Starting TryStartSession operation");
            _isOperationInProgress = true;
            
            using var cts = new CancellationTokenSource();
            try
            {
                using var loading = _requestLoadingManager.AddLoading();
                
                var result = await _runService.StartSession(cts.Token);
                
                if (!result.IsSuccess) _requestErrorManager.ShowError(result.Error);
                FetchRun().Forget();
            }
            catch (OperationCanceledException)
            {
                DebugManager.Log(DebugCategory.Backend, "TryStartSession operation was cancelled");
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, $"TryStartSession failed with exception: {e.Message}");
                _requestErrorManager.ShowError($"Failed to start session: {e.Message}");
            }
            finally
            {
                _isOperationInProgress = false;
            }
        }
        
        private async void TryFinishSession()
        {
            var run = _appState.RunState.Value;

            if (run == null || run.RunStatus != RunStatus.Active || _appState.CurrentSessionTime.Value >= _appConfig.MinSessionTimeThreshold)
            {
                DebugManager.Log(DebugCategory.Backend, $"TryFinishSession ignored - conditions not met. Run: {run?.RunStatus}, Time: {_appState.CurrentSessionTime.Value}"); return;
            }
            
            DebugManager.Log(DebugCategory.Backend, "Starting TryFinishSession operation");
            
            using var cts = new CancellationTokenSource();
            try
            {
                var result = await _runService.FinishSession(cts.Token);

                if (!result.IsSuccess)
                {
                    DebugManager.Log(DebugCategory.Backend, $"TryFinishSession failed: {result.Error}");
                    _requestErrorManager.ShowError($"{result.Error}");
                    return;
                }

                // await FetchRun();
                
                DebugManager.Log(DebugCategory.Backend, "TryFinishSession completed successfully");
                _windowsManager.Open<SessionFinishedViewWindow>(new SessionFinishedData(
                        run.PlannedSessionsAmountCompletedSessions, run.PlannedSessionsAmount))
                    .Forget();
            }
            catch (OperationCanceledException)
            {
                DebugManager.Log(DebugCategory.Backend, "TryFinishSession operation was cancelled");
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, $"TryFinishSession failed with exception: {e.Message}");
                _requestErrorManager.ShowError($"Failed to finish session: {e.Message}");
            }
        }

        private async void TryCancelRun()
        {
            if (_isOperationInProgress)
            {
                DebugManager.Log(DebugCategory.Backend, "TryCancelRun ignored - operation already in progress"); return;
            }
            
            DebugManager.Log(DebugCategory.Backend, "Starting TryCancelRun operation");
            _isOperationInProgress = true;
            
            using var cts = new CancellationTokenSource();
            try
            {
                using var loading = _requestLoadingManager.AddLoading();
                
                var result = await _runService.CancelRun(cts.Token);
                
                if (!result.IsSuccess) _requestErrorManager.ShowError(result.Error);
                FetchRun().Forget();
            }
            catch (OperationCanceledException)
            {
                DebugManager.Log(DebugCategory.Backend, "TryCancelRun operation was cancelled");
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, $"TryCancelRun failed with exception: {e.Message}");
                _requestErrorManager.ShowError($"Failed to cancel run: {e.Message}");
            }
            finally
            {
                _isOperationInProgress = false;
                DebugManager.Log(DebugCategory.Backend, "TryCancelRun operation completed");
            }
        }
        private async void TryCancelSession()
        {
            if (_isOperationInProgress)
            {
                DebugManager.Log(DebugCategory.Backend, "TryCancelSession ignored - operation already in progress"); return;
            }
            
            DebugManager.Log(DebugCategory.Backend, "Starting TryCancelSession operation");
            _isOperationInProgress = true;
            
            using var cts = new CancellationTokenSource();
            try
            {
                using var loading = _requestLoadingManager.AddLoading();
                
                var result = await _runService.CancelSession(cts.Token);
                
                if (!result.IsSuccess) _requestErrorManager.ShowError(result.Error);
                FetchRun().Forget();
            }
            catch (OperationCanceledException)
            {
                DebugManager.Log(DebugCategory.Backend, "TryCancelSession operation was cancelled");
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, $"TryCancelSession failed with exception: {e.Message}");
                _requestErrorManager.ShowError($"Failed to cancel session: {e.Message}");
            }
            finally
            {
                _isOperationInProgress = false;
            }
        }
        
        #endregion
        
        #region Timer Management
        
        private void StartTimerTick()
        {
            StopTimerTick();

            _timerDisposable = Observable
                .Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    if (_appState.RunState.Value == null) return;
                    if (_appState.RunState.Value.RunStatus != RunStatus.Active) return;
                    var currentTime = _appState.CurrentSessionTime.Value;

                    if (currentTime <= 0)
                    {
                        StopTimerTick();
                        TryFinishSession();
                        return;
                    }

                    _appState.CurrentSessionTime.Value = Math.Max(0, currentTime - 1);
                });
        }
        
        private void StopTimerTick()
        {
            _timerDisposable?.Dispose();
            _timerDisposable = null;
        }
        
        #endregion
        
        #region Utility Methods
        
        public void CancelAllRequests()
        {
            _isOperationInProgress = false;
        }
        
        #endregion
        
        #region Cleanup
        
        public void Dispose()
        {
            _disposables.Dispose();
            StopTimerTick();
            
            _runSubmitViewWindow.OnSubmit -= OnRunSubmit;
            _sessionFinishedViewWindow.OnClosePressed -= OnSessionFinishedClose;
            
            _runActionsView.OnStartSession -= OnStartSession;
            _runActionsView.OnCancelRun -= OnCancelRun;
            _runActionsView.OnCancelSession -= OnCancelSession;
            
            DebugManager.Log(DebugCategory.Backend, "RunViewModel disposed successfully");
        }
        
        #endregion
    }
}