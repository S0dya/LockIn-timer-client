using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace App.Timer.Windows
{
    public class RequestErrorWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI errorText;
        [Space]
        [SerializeField] private Button closeButton;
        
        public event Action OnClose;

        private void Awake()
        {
            closeButton.onClick.AddListener(OnCloseClicked);
        }

        protected override async UniTask OnOpen()
        {
            await base.OnOpen();
            
            if (Payload is string errorMessage)
            {
                errorText.text = errorMessage;
            }
            else
            {
                errorText.text = "Unknown error occurred";
            }
        }

        private void OnCloseClicked()
        {
            OnClose?.Invoke();
        }
    }
}
