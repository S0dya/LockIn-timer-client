using System;
using Cysharp.Threading.Tasks;
using PT.Tools.Windows;
using PT.UI.Buttons;
using UnityEngine;

namespace App.Timer.Run
{
    public class RunSubmitViewWindow : WindowBase
    {
        [SerializeField] private BasicButton submitButton;
        [SerializeField] private BasicButton closeButton;
        
        public event Action<string> OnSubmit;
        
        private void Start()
        {
            submitButton.SetOnClick(SubmitButtonClicked);
            closeButton.SetOnClick(CloseClicked);
        }
        
        private void SubmitButtonClicked()
        {
            OnSubmit?.Invoke("");
        }

        private void CloseClicked()
        {
            
        }
    }
}