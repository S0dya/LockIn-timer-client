using System;
using System.Threading;
using App.Timer.Back.Models;
using App.Timer.Back.Services;
using App.Timer.States;
using App.Timer.Windows;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Windows;
using Zenject;

namespace App.Timer.Run.History
{
    public class RunHistoryViewModel : IInitializable, IDisposable
    {
        [Inject] private AppState _appState;
        [Inject] private AppConfig _appConfig;
        [Inject] private RunActionsView _runActionsView;
        [Inject] private RunService _runService;
        [Inject] private RequestLoadingManager _requestLoadingManager;
        [Inject] private RequestErrorManager _requestErrorManager;
        [Inject] private RunHistoryWindow _runHistoryWindow;
        [Inject(Id = "App")] private WindowsManager _windowsManager;
        
        private int _currentOffset;
        private bool _isLoading;
        
        public void Initialize()
        {
            _runActionsView.OnOpenHistory += OnOpenHistory;
            _runHistoryWindow.TryLoadMore += OnTryLoadMore;
            _runHistoryWindow.CloseWindow += OnCloseWindow;
            
            _runHistoryWindow.Init();
        }

        private async void OnOpenHistory()
        {
            _currentOffset = 0;
            _runHistoryWindow.WipeRunHistories();
            await LoadRunHistory();
            _windowsManager.Open<RunHistoryWindow>().Forget();
        }
        
        private async void OnTryLoadMore()
        {
            if (_isLoading) return;
            await LoadRunHistory();
        }
        
        private void OnCloseWindow()
        {
            _windowsManager.Close<RunHistoryWindow>().Forget();
        }
        
        private async UniTask LoadRunHistory()
        {
            using var cts = new CancellationTokenSource();
            try
            {
                _isLoading = true;
                using var loadingToken = _requestLoadingManager.AddLoading();
                
                var result = await _runService.GetRunHistory(new RunHistoryRequest()
                {
                    Limit = _appConfig.RunHistoryLimit,
                    Offset = _currentOffset,
                }, cts.Token);

                if (result.IsSuccess)
                {
                    _runHistoryWindow.ShowRunHistories(result.Value);
                    _currentOffset += result.Value.Count;
                }
                else
                {
                    _runHistoryWindow.StopLoading();
                }
            }
            catch (OperationCanceledException)
            {
                DebugManager.Log(DebugCategory.Backend, "LoadRunHistory operation was cancelled");
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.Backend, $"LoadRunHistory failed with exception: {e.Message}");
                _requestErrorManager.ShowError($"Failed to load run history: {e.Message}");
                _runHistoryWindow.StopLoading();
            }
            finally
            {
                _isLoading = false;
            }
        }
        
        public void Dispose()
        {
            _runActionsView.OnOpenHistory -= OnOpenHistory;
            _runHistoryWindow.TryLoadMore -= OnTryLoadMore;
            _runHistoryWindow.CloseWindow -= OnCloseWindow;
        }
    }
}