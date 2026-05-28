using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace App.Shared.Synchronization
{
    public class AppFocusSynchronization : MonoBehaviour
    {
        [Inject] private AppSynchronizationService _appSynchronizationService;
        
        private void OnApplicationPause(bool value)
        {
            if (!value)
                _appSynchronizationService.FocusSynchronize();
        }
        private void OnApplicationFocus(bool value)
        {
            if (value)
                _appSynchronizationService.FocusSynchronize();
        }
    }
}