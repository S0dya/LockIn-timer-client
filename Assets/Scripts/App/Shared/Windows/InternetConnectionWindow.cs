using System;
using PT.Tools.Windows;
using PT.UI.Buttons;
using UnityEngine;

namespace App.Shared.Windows
{
    public class InternetConnectionWindow : WindowBase
    {
        [SerializeField] private BasicButton retryButton;
        
        public event Action OnRetry;

        private void Start()
        {
            retryButton.SetOnClick(OnRetryClicked); //could add setting button interactable for specified amount of seconds in config
        }

        private void OnRetryClicked()
        {
            OnRetry?.Invoke();
        }
    }
}