using System;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using PT.UI.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App.Timer.Run
{
    public class RunSubmitViewWindow : WindowBase
    {
        [SerializeField] private TMP_InputField inputField;
        [Space]
        [SerializeField] private BasicButton submitButton;
        [SerializeField] private BasicButton closeButton;
        
        public event Action<string> OnSubmit;
        public event Action OnCloseClicked;
        
        private void Start()
        {
            submitButton.SetOnClick(SubmitButtonClicked);
            closeButton.SetOnClick(CloseClicked);
        }

        protected override async UniTask OnOpen()
        {
            await base.OnOpen();

            inputField.text = "";
        }
        
        private void SubmitButtonClicked()
        {
            OnSubmit?.Invoke(inputField.text);
        }

        private void CloseClicked()
        {
            OnCloseClicked?.Invoke();
        }
    }
}