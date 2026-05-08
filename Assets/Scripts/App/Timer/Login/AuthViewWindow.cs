using System;
using PT.Tools.Helper;
using PT.Tools.Windows;
using PT.UI.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace App.Timer.Login
{
    public class AuthViewWindow : WindowBase
    {
        [SerializeField] private TextField nameField;
        [SerializeField] private TextField passwordField;
        [Space]
        [SerializeField] private BasicButton loginButton;
        [SerializeField] private CanvasGroup loginCg;
        [Space]
        [SerializeField] private BasicButton registerButton;
        [SerializeField] private CanvasGroup registerCg;
        [Space]
        [SerializeField] private TextMeshProUGUI errorText;

        public event Action<string, string> OnLogin;
        public event Action<string, string> OnRegister;
        
        private void Start()
        {
            nameField.RegisterValueChangedCallback(e => CheckInteractableButtons());
            passwordField.RegisterValueChangedCallback(e => CheckInteractableButtons());
            
            loginButton.SetOnClick(LoginButtonClicked);
            registerButton.SetOnClick(RegisterButtonClicked);

            loginCg.interactable = false;
            registerCg.interactable = false;
        }
        
        private void OnEnable()
        {
            nameField.value = "";
            passwordField.value = "";
            loginCg.interactable = false;
            registerCg.interactable = false;
            
            errorText.SetActive(false);
            errorText.text = "";
        }

        public void SetErrorText(string text)
        {
            errorText.SetActive(true);
            errorText.text = text;
        }
        
        private void LoginButtonClicked()
        {
            if (!ButtonsClickable()) return;
            
            OnLogin?.Invoke(nameField.value, passwordField.value);
        }

        private void RegisterButtonClicked()
        {
            if (!ButtonsClickable()) return;
            
            OnRegister?.Invoke(nameField.value, passwordField.value);
        }

        private void CheckInteractableButtons()
        {
            loginCg.interactable = registerCg.interactable = ButtonsClickable();
        }

        private bool ButtonsClickable() => nameField.value != null 
                                          && passwordField.value != null 
                                          && nameField.value.Length > 3
                                          && passwordField.value.Length > 3;
    }
}