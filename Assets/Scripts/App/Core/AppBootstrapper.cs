using App.Shared.Synchronization;
using App.Shared.Windows;
using Cysharp.Threading.Tasks;
using PT.Tools.Debugging;
using PT.Tools.Http;
using Zenject;

namespace App.Core
{
    public class AppBootstrapper : IInitializable
    {
        [Inject] private AppSynchronizationService _syncService; 
        [Inject] private RequestLoadingManager _requestLoadingManager; 
        [Inject] private InternetState _internetState; 
        
        public async void Initialize()
        {
            DebugManager.Log(DebugCategory.Points, $"AppBootstrapper Initialize");

            _syncService.Synchronize().Forget();
        }
    }
}