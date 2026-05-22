using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using UniRx;
using Zenject;

namespace App.Timer.Windows
{
    public class RequestErrorManager : IInitializable, IDisposable
    {
        [Inject] private RequestErrorWindow _requestErrorWindow;
        [Inject(Id = "Menu")] private WindowsManager _windowsManager;
        
        private CancellationTokenSource _cts;
        
        public void Initialize()
        {
            _requestErrorWindow.OnClose += ManualClose;
        }
        
        public void ShowError(string errorMessage, bool autoClose = true)
        {
            if (string.IsNullOrEmpty(errorMessage))
            {
                errorMessage = "Unknown error occurred";
            }
            
            _windowsManager.Open<RequestErrorWindow>(errorMessage).Forget();
            if (autoClose) StartAutoClose();
        }
        
        private void StartAutoClose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = new ();
            
            AutoCloseAfterDelay(_cts.Token).Forget();
        }
        
        private async UniTaskVoid AutoCloseAfterDelay(CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(10f), cancellationToken: cancellationToken);
                
                if (_windowsManager.IsOpen<RequestErrorWindow>() && !cancellationToken.IsCancellationRequested)
                {
                    _windowsManager.Close<RequestErrorWindow>().Forget();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        private void ManualClose()
        {
            _cts?.Cancel();
            _windowsManager.Close<RequestErrorWindow>().Forget();
        }
        
        public void Dispose()
        {
            _cts?.Cancel();
            _requestErrorWindow.OnClose -= ManualClose;
        }
    }
}
