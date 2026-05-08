using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using TMPro;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using Zenject;

namespace App.Timer.Windows
{
    public class RequestErrorWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI errorText;
        [Space]
        [SerializeField] private Button closeButton;
        [SerializeField] private float autoCloseDelay = 10f;
        
        [Inject] private AppWindowsState _appWindowsState;
        [Inject (Id = "Menu")] private WindowsManager _windowsManager;
        
        private bool isManuallyClosed = false;
        private CancellationTokenSource autoCloseCancellationToken;

        private void Awake()
        {
            closeButton.onClick.AddListener(OnCloseClicked);
            
            _appWindowsState.Error.Subscribe(OnErrorOccured).AddTo(this);
        }

        private void OnCloseClicked()
        {
            isManuallyClosed = true;
            autoCloseCancellationToken?.Cancel();
            _windowsManager.Close<RequestErrorWindow>().Forget();
        }
        
        private void OnErrorOccured(string errorMessage)
        {
            if (!string.IsNullOrEmpty(errorMessage))
            {
                errorText.text = errorMessage;
            }
            else
            {
                errorText.text = "Unknown error occurred";
            }

            _windowsManager.Open<RequestErrorWindow>().Forget();
            StartAutoClose();
        }
        
        private void StartAutoClose()
        {
            autoCloseCancellationToken?.Cancel();
            autoCloseCancellationToken = new CancellationTokenSource();
            
            AutoCloseAfterDelay(autoCloseCancellationToken.Token).Forget();
        }
        
        private async UniTaskVoid AutoCloseAfterDelay(CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(autoCloseDelay), cancellationToken: cancellationToken);
                
                if (!isManuallyClosed && !cancellationToken.IsCancellationRequested)
                {
                    _windowsManager.Close<RequestErrorWindow>().Forget();
                }
            }
            catch (OperationCanceledException)
            {
                // Auto-close was cancelled by manual close
            }
        }
    }
}
