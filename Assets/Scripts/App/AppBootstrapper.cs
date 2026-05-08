using App.Timer;
using App.Timer.Back.Services;
using App.Timer.States;
using PT.Tools.Debugging;
using PT.Tools.Http;
using UnityEngine;
using Zenject;

namespace App
{
    public class AppBootstrapper : IInitializable
    {
        [Inject] private AuthService _authService; 
        [Inject] private RunService _runService; 
        [Inject] private TimerSettingsService _timerSettingsService; 
        [Inject] private AppState _appState; 
        
        public async void Initialize()
        {
            var userResponseResult = await _authService.GetCurrentUser();

            if (!userResponseResult.IsSuccess)
            {
                DebugManager.Log(DebugCategory.Points, $"User is not Authenticated");

                _appState.CurrentUser.Value = null;
            }
        }
    }
}