using App.Timer.Login;
using App.Timer.States;
using App.Timer.Windows;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using Zenject;

namespace App
{
    public class AppSynchronizationService
    {
        [Inject] private AppState _appState; 
        [Inject] private AuthViewModel _authViewModel; 
        [Inject] private RequestLoadingManager _requestLoadingManager; 
        
        public async UniTask Synchronize()
        {
            DebugManager.Log(DebugCategory.Points, $"Synchronization started");

            try
            {
                using var loadingToken = _requestLoadingManager.AddLoading();

                await _authViewModel.FetchCurrentUser();
            }
            catch
            {
                DebugManager.Log(DebugCategory.Points, $"Synchronization failed");
            }
        }
    }
}