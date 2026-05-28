using System;
using PT.Tools.Helper;
using PT.Tools.Windows;
using PT.UI.Buttons;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

namespace App.Features.Login
{
    public class AuthViewWindow : WindowBase
    {
        [SerializeField] private TMP_InputField nameField;
        [SerializeField] private TMP_InputField passwordField;
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
            nameField.onValueChanged.AddListener(e => CheckInteractableButtons());
            passwordField.onValueChanged.AddListener(e => CheckInteractableButtons());
            
            loginButton.SetOnClick(LoginButtonClicked);
            registerButton.SetOnClick(RegisterButtonClicked);

            loginCg.interactable = false;
            registerCg.interactable = false;
        }
        
        private void OnEnable()
        {
            nameField.text = "";
            passwordField.text = "";
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
            
            OnLogin?.Invoke(nameField.text, passwordField.text);
        }

        private void RegisterButtonClicked()
        {
            if (!ButtonsClickable()) return;
            
            OnRegister?.Invoke(nameField.text, passwordField.text);
        }

        private void CheckInteractableButtons()
        {
            loginCg.interactable = registerCg.interactable = ButtonsClickable();
        }

        private bool ButtonsClickable() => !string.IsNullOrEmpty(nameField.text) 
                                          && !string.IsNullOrEmpty(passwordField.text) 
                                          && nameField.text.Length > 3
                                          && passwordField.text.Length > 3;
    }
}