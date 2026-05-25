using System;
using System.Threading;
using App.Timer.Back.Models;
using App.Timer.Back.Services;
using App.Timer.States;
using App.Timer.Windows;
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
        [Inject] private RequestLoadingManager _requestLoadingManager;
        [Inject(Id = "App")] private WindowsManager _windowsManager;
        
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
            DebugManager.Log(DebugCategory.TimerAuth, $"Login button clicked for user: {name}");
            TryLogin(name, password).Forget();
        }

        private void OnRegisterButtonClicked(string name, string password)
        {
            DebugManager.Log(DebugCategory.TimerAuth, $"Register button clicked for user: {name}");
            TryRegister(name, password).Forget();
        }

        private async UniTask TryLogin(string name, string password)
        {
            DebugManager.Log(DebugCategory.TimerAuth, "Starting Login operation");

            try
            {
                var loginResult = await _authService.Login(new LoginRequest()
                {
                    Username = name,
                    Password = password
                }, _cts.Token);

                if (loginResult.IsSuccess)
                {
                    DebugManager.Log(DebugCategory.TimerAuth, "Login successful, fetching current user");
                    FetchCurrentUser().Forget();
                }
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.TimerAuth, "Login failed");
                _authViewWindow.SetErrorText($"Login failed: {e.Message}");
            }
        }

        private async UniTask TryRegister(string name, string password)
        {
            DebugManager.Log(DebugCategory.TimerAuth, "Starting Register operation");

            try
            {
                using var loadingToken =_requestLoadingManager.AddLoading();
                
                var registerResult = await _authService.Register(new RegisterRequest()
                {
                    Username = name,
                    Password = password
                }, _cts.Token);
                
                if (registerResult.IsSuccess)
                {
                    DebugManager.Log(DebugCategory.TimerAuth, "Registration successful, fetching current user");
                    FetchCurrentUser().Forget();
                }
            }
            catch (Exception e)
            {
                DebugManager.Log(DebugCategory.TimerAuth, $"Register failed: {e.Message}");
                _authViewWindow.SetErrorText($"Register failed: {e.Message}");
            }
        }

        public async UniTask FetchCurrentUser()
        {
            DebugManager.Log(DebugCategory.TimerAuth, "Starting FetchCurrentUser operation");
            
            var result = await _authService.GetCurrentUser(_cts.Token);

            if (result.IsSuccess)
            {
                using var loadingToken =_requestLoadingManager.AddLoading();
                
                var newUser = new UserState()
                {
                    Username = result.Value.Username,
                    UserRole = result.Value.Role,
                };

                if (_appState.CurrentUser.Value != null && _appState.CurrentUser.Value.Equals(newUser)) return;

                DebugManager.Log(DebugCategory.TimerAuth, $"Current user fetched: {result.Value.Username}, Role: {result.Value.Role}");
                _appState.CurrentUser.Value = newUser;
            }
            else
            {
                DebugManager.Log(DebugCategory.TimerAuth, $"Failed to fetch user, {result.Error}");
                _appState.CurrentUser.Value = null;
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
