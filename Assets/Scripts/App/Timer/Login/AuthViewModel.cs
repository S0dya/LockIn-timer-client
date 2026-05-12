using System;
using System.Threading;
using App.Timer.Back.Models;
using App.Timer.Back.Services;
using App.Timer.States;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Windows;
using UniRx;
using Zenject;

namespace App.Timer.Login
{
    public class AuthViewModel : IInitializable, IDisposable
    {
        [Inject] private AppState _appState;
        [Inject] private AuthViewWindow _authViewWindow;
        [Inject] private AuthService _authService;
        [Inject (Id = "Menu")] private WindowsManager _windowsManager;
        
        private readonly CancellationTokenSource _cts = new();

        private readonly CompositeDisposable _disposables = new();

        public void Initialize()
        {
            _appState.CurrentUser
                .Subscribe(CurrentUserChanged)
                .AddTo(_disposables);
            
            _authViewWindow.OnLogin += OnLoginButtonClicked;
            _authViewWindow.OnRegister += OnRegisterButtonClicked;
        }

        public void Init()
        {
            FetchCurrentUser().Forget();
        } 
        
        private void CurrentUserChanged(UserState user)
        {
            if (user == null || user.Username == null)
            {
                _windowsManager.Open<AuthViewWindow>().Forget();
            }
            else
            {
                _windowsManager.Close<AuthViewWindow>().Forget();
            }
        }

        private void OnLoginButtonClicked(string name, string password)
        {
            TryLogin(name, password).Forget();
        }

        private void OnRegisterButtonClicked(string name, string password)
        {
            TryRegister(name, password).Forget();
        }

        private async UniTask TryLogin(string name, string password)
        {
            var loginResult = await _authService.Login(new LoginRequest()
            {
                Username = name,
                Password = password
            }, _cts.Token);

            if (loginResult.IsSuccess)
            {
                FetchCurrentUser().Forget();
                return;
            }
            
            DebugManager.Log(DebugCategory.Backend, $"Login failed, {loginResult.Error}");
            _authViewWindow.SetErrorText($"Login failed: {loginResult.Error}");
        }

        private async UniTask TryRegister(string name, string password)
        {
            var registerResult = await _authService.Register(new RegisterRequest()
            {
                Username = name,
                Password = password
            }, _cts.Token);

            if (registerResult.IsSuccess)
            {
                FetchCurrentUser().Forget();
                return;
            }
            
            DebugManager.Log(DebugCategory.Backend, $"Register failed, {registerResult.Error}");
            _authViewWindow.SetErrorText($"Register failed: {registerResult.Error}");
        }

        public async UniTask FetchCurrentUser()
        {
            var result = await _authService.GetCurrentUser(_cts.Token);

            if (result.IsSuccess)
            {
                _appState.CurrentUser.Value = new UserState()
                {
                    Username = result.Value.Username,
                    UserRole = result.Value.Role,
                };
            }
            else
            {
                _appState.CurrentUser.Value = null;
                
                DebugManager.Log(DebugCategory.Backend, $"Failed to fetch user, {result.Error}");
            }
        }
        
        public void CancelAllRequests()
        {
            _cts?.Cancel();
        }

        public void Dispose()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _disposables.Dispose();
        }
    }
}
