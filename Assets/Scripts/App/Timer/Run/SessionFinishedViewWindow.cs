using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using PT.UI.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

namespace App.Timer.Run
{
    public class SessionFinishedViewWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI sessionResultText;
        [Space]
        [SerializeField] private Button closeButton;
        [SerializeField] private float autoCloseDelay = 3f;
        
        private bool isManuallyClosed = false;
        private CancellationTokenSource autoCloseCancellationToken;
        
        public event Action OnClosePressed;
        
        private void Start()
        {
            closeButton.onClick.AddListener(CloseButtonClicked);
        }
        
        private void OnDestroy()
        {
            autoCloseCancellationToken?.Cancel();
            autoCloseCancellationToken?.Dispose();
        }
        
        protected override async UniTask OnOpen()
        {
            isManuallyClosed = false;
            
            if (Payload is SessionFinishedData sessionFinishedData)
            {
                sessionResultText.text = $"{sessionFinishedData.CompletedSessions} / {sessionFinishedData.PlannedSessionsAmount}";
            }
            
            await base.OnOpen();
            
            StartAutoClose();
        }
        
        private void StartAutoClose()
        {
            autoCloseCancellationToken?.Cancel();
            autoCloseCancellationToken = new();
            
            AutoCloseAfterDelay(autoCloseCancellationToken.Token).Forget();
        }
        
        private async UniTaskVoid AutoCloseAfterDelay(CancellationToken cancellationToken)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(autoCloseDelay), cancellationToken: cancellationToken);
                
                if (!isManuallyClosed && !cancellationToken.IsCancellationRequested)
                {
                    OnClosePressed?.Invoke();
                }
            }
            catch (OperationCanceledException)
            {
            }
        }
        
        private void CloseButtonClicked()
        {
            isManuallyClosed = true;
            autoCloseCancellationToken?.Cancel();
            OnClosePressed?.Invoke();
        }
    }

    public class SessionFinishedData
    {
        public int CompletedSessions { get; }
        public int PlannedSessionsAmount { get; }
        
        public SessionFinishedData(int completedSessions, int plannedSessionsAmount)
        {
            CompletedSessions = completedSessions;
            PlannedSessionsAmount = plannedSessionsAmount;
        }
    }
}