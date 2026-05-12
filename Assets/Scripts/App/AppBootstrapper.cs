using App.Timer.States;
using App.Timer.Windows;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using Zenject;

namespace App
{
    public class AppBootstrapper : IInitializable
    {
        [Inject] private AppSynchronizationService _syncService; 
        [Inject] private RequestLoadingManager _requestLoadingManager; 
        [Inject] private InternetState _internetState; 
        
        public async void Initialize()
        {
            DebugManager.Log(DebugCategory.Points, $"AppBootstrapper Initialize");

            try
            {
                using var loadingToken = _requestLoadingManager.AddLoading();
                
                await _syncService.Synchronize();
            }
            catch{}
        }
    }
}